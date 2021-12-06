using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

namespace Mediapipe.Unity
{
	public class PoseWorldLandmarkListAnnotationController : AnnotationController<PoseLandmarkListAnnotation>
	{
		[SerializeField] float hipHeightMeter = 0.9f;
		[SerializeField] Vector3 scale = new Vector3(100, 100, 100);
		[SerializeField] bool visualizeZ = true;

		IList<Landmark> currentTarget;

		[SerializeField] GameObject robotGO;
		[SerializeField] GameObject headGO;
		[SerializeField] GameObject SRRollGO;
		[SerializeField] GameObject SRPitchGO;
		[SerializeField] GameObject RHandGO;
		[SerializeField] GameObject SLRollGO;
		[SerializeField] GameObject SLPitchGO;
		[SerializeField] GameObject LHandGO;

		[SerializeField] GameObject L;
		[SerializeField] GameObject R;

		[SerializeField] GameObject L2;
		[SerializeField] GameObject R2;

		SerialPort sp;
		float next_time; int ii = 0;
		// Use this for initialization
		void Awake()
		{
			string the_com = "COM4";
			next_time = Time.time;

			sp = new SerialPort("\\\\.\\" + the_com, 9600);
			if (!sp.IsOpen)
			{
				print("Opening " + the_com + ", baud 9600");
				sp.Open();
				sp.ReadTimeout = 100;
				sp.Handshake = Handshake.None;
				if (sp.IsOpen)
				{
					print("Open");
					sp.Write("#ALL");
				}
			}
		}

		protected override void Start()
		{
			base.Start();
			transform.localPosition = new Vector3(0, hipHeightMeter * scale.y, 0);

			// TODO: YK 
			var pointList = annotation.GetPointList();
			var connectionList = annotation.GetConnectionList();

			for (int i = 0; i < pointList.count; i++)
			{
				if (i > 24)
					pointList[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
			}
			for (int i = 0; i < connectionList.count; i++)
			{
				//if (i > 24)
				connectionList[i].gameObject.GetComponent<LineRenderer>().enabled = false;
			}

			Vector3 headRight = pointList[8].transform.position - pointList[7].transform.position;
			Vector3 bodyRight = pointList[12].transform.position - pointList[11].transform.position;

			//headGO = GameObject.Find()
		}

		public void DrawNow(IList<Landmark> target)
		{
			currentTarget = target;
			SyncNow();
		}

		public void DrawNow(LandmarkList target)
		{
			DrawNow(target?.Landmark);
		}

		public void DrawLater(IList<Landmark> target)
		{
			UpdateCurrentTarget(target, ref currentTarget);
		}

		public void DrawLater(LandmarkList target)
		{
			DrawLater(target?.Landmark);
		}

		protected override void SyncNow()
		{
			isStale = false;
			annotation.Draw(currentTarget, scale, visualizeZ);

			// TODO: YK Draw 
			var pointList = annotation.GetPointList();
			var connectionList = annotation.GetConnectionList();

			for (int i = 0; i < pointList.count; i++)
			{
				if (i > 24)
					pointList[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
			}
			for (int i = 0; i < connectionList.count; i++)
			{
				if (i > 24)
					connectionList[i].gameObject.GetComponent<LineRenderer>().enabled = false;
			}

			Vector3 HL = pointList[7].transform.position;
			Vector3 HR = pointList[8].transform.position;
			Vector3 BL = pointList[11].transform.position;
			Vector3 BR = pointList[12].transform.position;

			Vector3 headRight = (HR - HL).normalized;
			Vector3 bodyRight = (BR - BL).normalized;

			Debug.DrawRay(HL, headRight * 20f, Color.red);
			Debug.DrawRay(BL, bodyRight * 30f, Color.magenta);
			Debug.DrawRay(transform.position, headRight * 20f, Color.red, 0.1f);
			//Debug.DrawRay(transform.position, bodyRight * 30f, Color.magenta, 0.1f);

			Vector3 headF = Vector3.Cross(headRight, Vector3.up).normalized;
			Vector3 bodyF = Vector3.Cross(bodyRight, Vector3.up).normalized;

			Vector3 bodyUp = pointList[0].transform.position - (BL + bodyRight * 0.5f);

			Debug.DrawRay(HL, headF * 20f, Color.blue);
			Debug.DrawRay(BL, bodyF * 30f, Color.cyan);
			Debug.DrawRay(transform.position, headF * 20f, Color.black, 0.1f);


			Vector3 headFP = Vector3.ProjectOnPlane(headF, bodyUp).normalized;

			//Vector3 bodyFP = Vector3.back;
			Vector3 bodyFP = Vector3.ProjectOnPlane(bodyF, bodyUp).normalized;

			Debug.DrawRay(transform.position, headFP * 20f, Color.blue, 0.1f);
			Debug.DrawRay(transform.position, bodyFP * 30f, Color.cyan, 0.1f);

			float headAngle = Vector3.SignedAngle(bodyFP, headFP, Vector3.up);
			
			//Debug.Log($"ha: {headAngle}");
			//headGO.SendMessage("SetAngle", headAngle);
			headGO.SendMessage("SetDirection", headAngle);

			if (delayCount < delay)
			{
				delayCount += Time.deltaTime;
			}
			else
			{
				MoveHead(headAngle);
				delayCount = 0f;
			}
			
			// Right Arm 12->14, (14 - 12)
			Vector3 SRU = pointList[12].transform.position;
			Vector3 SRD = pointList[16].transform.position;
			
			Vector3 SR = (SRD - SRU).normalized;
			//Debug.DrawRay(SRU, SR * 20f, Color.blue);

			Vector3 SRRoll = Vector3.ProjectOnPlane(SR, bodyRight).normalized;
			float SRRollAngle = Vector3.SignedAngle(Vector3.down, SRRoll, bodyRight);

			//Debug.Log($"SRR: {SRRollAngle}");
			SRRollGO.SendMessage("SetAngle", SRRollAngle);
			
			// Right Arm Pitch
			Vector3 RSUp = Vector3.Cross(-bodyRight, SR).normalized;
			Vector3 bodyFPRS = Vector3.ProjectOnPlane(bodyF, RSUp).normalized;
			float SRPitchAngle = Vector3.SignedAngle(bodyFPRS, SR, RSUp);

			//Debug.Log($"SRP: {SRPitchAngle}");
			SRPitchGO.SendMessage("SetAngle", SRPitchAngle);

			// Left Arm 11->13, (13 - 11)
			Vector3 SLU = pointList[11].transform.position;
			Vector3 SLD = pointList[15].transform.position;

			Vector3 SL = (SLD - SLU).normalized;
			//Debug.DrawRay(SLU, SL * 20f, Color.red);

			Vector3 SLRoll = Vector3.ProjectOnPlane(SL, bodyRight).normalized;
			float SLRollAngle = Vector3.SignedAngle(Vector3.down, SLRoll, bodyRight);

			//Debug.Log($"SLR: {SLRollAngle}");
			SLRollGO.SendMessage("SetAngle", SLRollAngle);

			// Right Arm Pitch
			Vector3 LSUp = Vector3.Cross(bodyRight, SL).normalized;
			Vector3 bodyFPLS = Vector3.ProjectOnPlane(bodyF, LSUp).normalized;
			float SLPitchAngle = -Vector3.SignedAngle(bodyFPLS, SL, LSUp);

			//Debug.Log($"SLP: {SLPitchAngle}");
			SLPitchGO.SendMessage("SetAngle", SLPitchAngle);

			if (headAngle < -1)
			{
				L2.transform.localScale = new Vector3(Mathf.Abs(headAngle) / 100f, L2.transform.localScale.y, L2.transform.localScale.z);
				R2.transform.localScale = new Vector3(0.01f, R2.transform.localScale.y, R2.transform.localScale.z);
			}
			else if (headAngle > 1)
			{
				L2.transform.localScale = new Vector3(0.01f, L2.transform.localScale.y, L2.transform.localScale.z);
				R2.transform.localScale = new Vector3(Mathf.Abs(headAngle) / 100f, R2.transform.localScale.y, R2.transform.localScale.z);
			}
			else
			{
				L2.transform.localScale = new Vector3(0.01f, L2.transform.localScale.y, L2.transform.localScale.z);
				R2.transform.localScale = new Vector3(0.01f, R2.transform.localScale.y, R2.transform.localScale.z);
			}
		}


		public int MoveRobot(float head, float sr1, float sr2, float sl1, float sl2)
		{
			if (sp.IsOpen)
			{
				print("Writing ");
				string cmd = $"#{head.ToString("000")}000{sr1.ToString("000")}{sr2.ToString("000")}000{sl1.ToString("000")}{sl2.ToString("000")}000000000000000";

				sp.Write(cmd);
			}

			return 0;
		}

		float delay = 0.3f;
		float delayCount = 0f;
		public int MoveHead(float dir)
		{
			
			if (sp.IsOpen)
			{
				if ( dir > 8f)
				{
					sp.Write("#HEADRIGHT");
					R.transform.localScale = new Vector3(dir / 100f, R.transform.localScale.y, R.transform.localScale.z);
					R.SetActive(true);
					L.SetActive(false);
				}
				else if (dir < -8f)
				{
					sp.Write("#HEADLEFT");
					L.transform.localScale = new Vector3(dir / 100f, L.transform.localScale.y, L.transform.localScale.z);
					L.SetActive(true);
					R.SetActive(false);
				}
				else
				{
					L.SetActive(false);
					R.SetActive(false);
				}
			}
			return 0;
		}
	}
}
