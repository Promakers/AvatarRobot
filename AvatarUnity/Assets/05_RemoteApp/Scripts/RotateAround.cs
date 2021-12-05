using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RotateAround : MonoBehaviour
{
   public enum RotMode
	{
      EulerLocalX,
      EulerLocalY,
      EulerLocalZ,
      XAxisCW,
      YAxisCW,
      ZAxisCW,
      ZAxisCCW,
   }

   [SerializeField] RotMode mode = RotMode.EulerLocalX;
   float startAngle{get; set; }
   
   Quaternion startQuat;

   [SerializeField] float angle = 0f;
   [SerializeField] float minDirAngleThreshold = 5f;
   [SerializeField] float minAngle = -90f;
   [SerializeField] float maxAngle = 90f;

   // Start is called before the first frame update
   void Start()
    {
		switch (mode)
		{
			case RotMode.EulerLocalX:
            startAngle = transform.localRotation.eulerAngles.x;
            break;
			case RotMode.EulerLocalY:
            startAngle = transform.localRotation.eulerAngles.y;
            break;
			case RotMode.EulerLocalZ:
            startAngle = transform.localRotation.eulerAngles.z;
            break;
         case RotMode.XAxisCW:
            break;
         case RotMode.YAxisCW:
            break;
         case RotMode.ZAxisCCW:
				break;
			case RotMode.ZAxisCW:
				break;
			default:
				break;
		}
		   startQuat =  transform.localRotation;  // Quarternion
   }

    // Update is called once per frame
    void Update()
    {
      switch (mode)
      {
         case RotMode.EulerLocalX:
            transform.localEulerAngles = new Vector3(startAngle + angle, transform.localEulerAngles.y, transform.localEulerAngles.z);
            break;
         case RotMode.EulerLocalY:
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, startAngle + angle, transform.localEulerAngles.z);
            break;
         case RotMode.EulerLocalZ:
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, startAngle + angle);
            break;
         case RotMode.XAxisCW:
            Quaternion rotx = Quaternion.AngleAxis(angle, Vector3.right);
            transform.localRotation = startQuat * rotx;
            break;
         case RotMode.YAxisCW:
            Quaternion roty = Quaternion.AngleAxis(angle, Vector3.up);
            transform.localRotation = startQuat * roty;
            break;         
         case RotMode.ZAxisCW:
            Quaternion rotz = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.localRotation = startQuat * rotz;
            break;
         case RotMode.ZAxisCCW:
            Quaternion rotN = Quaternion.AngleAxis(-angle, Vector3.forward);
            transform.localRotation = startQuat * rotN;
            break;
         default:
            break;
      }
   }

   public float speed = 5f;
   public void SetAngle(float angle)
	{
      angle = Mathf.Clamp(angle, minAngle, maxAngle);
      this.angle = Mathf.Lerp(this.angle, angle, speed * Time.deltaTime);
      //this.angle = angle;
   }

   public void SetDirection(float angle)
   {
      if(Mathf.Abs(angle) > minDirAngleThreshold)
      angle = angle = Mathf.Clamp(this.angle + angle * 0.3f, minAngle, maxAngle);
      this.angle = Mathf.Lerp(this.angle, angle, speed * Time.deltaTime);
      //this.angle = angle;
   }
}
