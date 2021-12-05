/*
 *  Program Name :  Avata Robot
 *  
 *  Name  : Avata Robot
 *  PIN   : 1234
 */
#include <SoftwareSerial.h> 
#include <Servo.h> 

SoftwareSerial BTserial(12, 13); // TX, RX

//----------------------------------------------------------------------------------
//
//----------------------------------------------------------------------------------
#define ACTDATA_MAX                 16
#define ACTDATA_TIME                15  // Column of Time

#define SERVO_MAX                   12


//----------------------------------------------------------------------------------
//  Servo Pin
//----------------------------------------------------------------------------------
#define SERVO_LEFT_SHOLDER_ROLL     2   //  L Sholder roll
#define SERVO_LEFT_SHOLDER_PITCH    3   //  L Sholder pitch
#define SERVO_LEFT_HAND_GRIP        4   //  L Hand grip
#define SERVO_LEFT_FOOT_YAW         5   //  L Foot yaw
#define SERVO_LEFT_FOOT_PITCH       6   //  L Foot pitch

#define SERVO_RIGHT_SHOLDER_ROLL    A5  //  R Sholder roll
#define SERVO_RIGHT_SHOLDER_PITCH   A4  //  R Sholder pitch
#define SERVO_RIGHT_HAND_GRIP       A3  //  R Hand grip

#define SERVO_RIGHT_FOOT_YWA        A2  //  R Foot yaw
#define SERVO_RIGHT_FOOT_PITCH      A1  //  R Foot pitch

#define SERVO_HEAD_YAW              7  //  Head yaw
#define SERVO_WAIST_YAW             8  //  Waist yaw


//----------------------------------------------------------------------------------
//  12  13
//----------------------------------------------------------------------------------

//----------------------------------------------------------------------------------
//  LED Pin
//----------------------------------------------------------------------------------
#define LED_RED                     9
#define LED_GREEN                   10
#define LED_BLUE                    11


//**********************************************************************************
//  Action
//**********************************************************************************

//----------------------------------------------------------------------------------
//  Default
//----------------------------------------------------------------------------------
int g_ActionDefault[][ACTDATA_MAX] =                                    
{
  //            -   -   -        
  { 90, 90, 0, 90,  45, 180, 0,  0,  90, 90, 90, 90, 0,0,  0,  3000},
};

//----------------------------------------------------------------------------------
//  Command
//----------------------------------------------------------------------------------
int g_ActionCommand[][ACTDATA_MAX] =                                    
{
  //            -   -   -        
  { -1, -1, -1, -1,  -1, -1, -1, -1, -1, -1, -1, -1, 0,0, 0,  3000},
};


//----------------------------------------------------------------------------------
//  Left Hand Up
//----------------------------------------------------------------------------------
int g_ActionLeftHandUp[][ACTDATA_MAX] =                               
{
  //            -   -    - 
  { -1, -1,  0, 90, 45,   0, 50, 45, -1, -1, -1, -1,  255,  0,0, 3000}, 
};

//----------------------------------------------------------------------------------
//  Right Hand Up
//----------------------------------------------------------------------------------
int g_ActionRightHeadUp[][ACTDATA_MAX] =                              
{
  //             -  -    - 
  { -1, -1, 180, 50, 0,  180, 0, 0, -1, -1, -1, -1,  0,  255,0, 3000}, 
};


//----------------------------------------------------------------------------------
//  Hurrah
//----------------------------------------------------------------------------------
int g_ActionHurrah[][ACTDATA_MAX] =                                   
{
  //             -  -    - 
  { -1, -1, 180, 50, 0,  0, 50, 45, -1, -1, -1, -1,  255,  255,0, 3000}, 
};


//----------------------------------------------------------------------------------
//  Hand Front
//----------------------------------------------------------------------------------
int g_ActionHeadFront[][ACTDATA_MAX] =                               
{
  //            -   -   -        
  { -1, -1, 90, 90, 45, 90, 0, 0, -1, -1, -1, -1,  0,  0,255, 3000},
};

//----------------------------------------------------------------------------------
//  Hand Side
//----------------------------------------------------------------------------------
int g_ActionHandSide[][ACTDATA_MAX] =                                  
{
  //            -   -   -        
  { -1, -1, 0, 30, 45, 180, 90, 0, -1, -1, -1, -1,  0,  0,255, 3000},
};


//----------------------------------------------------------------------------------
//   Fine angle adjustments (degrees)
//
//                                    Trim  Min Max
//----------------------------------------------------------------------------------
int g_nTrimMinMax[SERVO_MAX][3] = { 
                                    { -3,   10, 155 },    //  Head yaw
                                    { 0,    0,  180 },    //  Waist yaw
                                    { 0,    0,  180 },    //  R Sholder roll
                                    { 4,    0,  90  },    //  R Sholder pitch   (90 ~ 0 )
                                    { 0,    0,  45  },    //  R Hand grip       (45 ~ 0 )                                    
                                    { 0,    0,  180 },    //  L Sholder roll    (180~ 0 )
                                    { 0,    0,  90  },    //  L Sholder pitch   (0  ~ 90)
                                    { 0,    0,  45  },    //  L Hand grip       (0  ~ 45)
                                    { 0,    0,  180 },    //  R Foot yaw
                                    { 0,    0,  180 },    //  R Foot pitch
                                    { 0,    0,  180 },    //  L Foot yaw
                                    { 0,    0,  180 }     //  L Foot pitch
                               };


int g_NowAngle[SERVO_MAX] =  { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};  // Initialize array to -1

                     
Servo servo[SERVO_MAX];


//**********************************************************************************
//
//**********************************************************************************
void ServoWrite( int pServoNum, int pAngle )
{
    int nServValue = pAngle + g_nTrimMinMax[pServoNum][0];

    if( g_NowAngle[pServoNum] ==  nServValue) return;

    nServValue = min( max( nServValue, g_nTrimMinMax[pServoNum][1] ), g_nTrimMinMax[pServoNum][2] );   
      
    servo[pServoNum].write( nServValue );  
    
    g_NowAngle[pServoNum] = nServValue;    
}


//**********************************************************************************
//
//**********************************************************************************
void RobotServoDefault()
{        
  for( int i = 0; i < SERVO_MAX; i ++ ) 
  {  
    int nServValue = g_ActionDefault[0][i];
        
    ServoWrite( i, nServValue );

    delay( 300 );
  }  
}

//**********************************************************************************
//
//**********************************************************************************
void RobotServo( int pSDataSize, int pServoData[][ACTDATA_MAX] ) 
{  
  int nFrame =  pSDataSize  / ( ACTDATA_MAX *  sizeof( int ) );

  int nowBright[3]= {0,0,0};
  int nMillisDelay  = 0;

#ifdef DEF_DEBUG
  Serial.print( "Frame = ");
  Serial.println( nFrame );  
#endif  

  for( int i = 0 ; i < nFrame ; i++ ) 
  {

    nowBright[0] = 255 - pServoData[i][SERVO_MAX];
    nowBright[1] = 255 - pServoData[i][SERVO_MAX+1];
    nowBright[2] = 255 - pServoData[i][SERVO_MAX+2];    

    //
    //  LED Write 
    //
    analogWrite(LED_RED,    nowBright[0] );
    analogWrite(LED_GREEN,  nowBright[1] );
    analogWrite(LED_BLUE,   nowBright[2] );
    
    //
    //
    //
    nMillisDelay = pServoData[i][ACTDATA_TIME];


    double startTime = millis();
    double endTime = startTime + nMillisDelay;

    //
    //  Compute Servo Angle Data
    //
    int nAngleStart[SERVO_MAX] = {0,};
    int nAngleDiff[SERVO_MAX] = {0,};
    
    
    for( int j = 0; j < SERVO_MAX; j ++ ) 
    {
      nAngleStart[j] = g_NowAngle[j];

      nAngleDiff[j] = pServoData[i][j] - nAngleStart[j];
    }
    
    while( endTime > millis() )
    {      

      float fDeta =  1.0 -  max( ( ( endTime - millis() - 5 ) / nMillisDelay ), 0.0 ) ;
                      
      for( int j = 0; j < SERVO_MAX; j ++ ) 
      {
        if( pServoData[i][j] == -1 ) continue;

        int nServValue = nAngleStart[j] + nAngleDiff[j] * fDeta;

#ifdef DEF_DEBUG
        Serial.print( nServValue ); 
        Serial.print( "  " );
#endif

        if( Serial.available() )  return;
          
        ServoWrite( j, nServValue );       
      }

#ifdef DEF_DEBUG
      Serial.println( " ");
#endif
    }

#ifdef DEF_DEBUG    
    Serial.println( "***** Frame Delay ******" );
 #endif
  } 
  
}

//**********************************************************************************
//
//  Setup
//
//**********************************************************************************
void setup() 
{ 
  //
  //  Servo Attach
  //
  servo[0].attach(SERVO_HEAD_YAW              );  // Head yaw
  servo[1].attach(SERVO_WAIST_YAW             );  // Waist yaw
  servo[2].attach(SERVO_RIGHT_SHOLDER_ROLL    );  // R Sholder roll
  servo[3].attach(SERVO_RIGHT_SHOLDER_PITCH   );  // R Sholder pitch
  servo[4].attach(SERVO_RIGHT_HAND_GRIP       );  // R Hand grip
    
  servo[5].attach(SERVO_LEFT_SHOLDER_ROLL     );  // L Sholder roll
  servo[6].attach(SERVO_LEFT_SHOLDER_PITCH    );  // L Sholder pitch
  servo[7].attach(SERVO_LEFT_HAND_GRIP        );  // L Hand grip

  servo[8].attach(SERVO_RIGHT_FOOT_YWA        );  // R Foot yaw
  servo[9].attach(SERVO_RIGHT_FOOT_PITCH      );  // R Foot pitch
  
  servo[10].attach(SERVO_LEFT_FOOT_YAW        );  // L Foot yaw
  servo[11].attach(SERVO_LEFT_FOOT_PITCH      );  // L Foot pitch
  
  RobotServoDefault();

  Serial.begin( 9600 );
  BTserial.begin( 9600 );
  
  Serial.println( "*************************************" );
  Serial.println( " Pro Makers Avata Robot     Ver 1.0  " );  
  Serial.println( "*************************************" );
}

//**********************************************************************************
//
//  Loop
//
//**********************************************************************************
void loop() 
{
  String strCommand = "";   
      
  if( Serial.available() ) 
  {
    if( Serial.read() != '#') return;  

    strCommand = Serial.readStringUntil('\n');       
  }
  else if( BTserial.available() )
  {   
    if( BTserial.read() != '#') return;

    strCommand = BTserial.readStringUntil('\n');
  }
  else
  {
    return;
  }
    
  Serial.print( "Command : " );
  Serial.println( strCommand );
      
  //----------------------------------------------------------------------------
  //  Stop
  //----------------------------------------------------------------------------
  if( strCommand == "S" )
  {
    RobotServo( sizeof( g_ActionDefault ), g_ActionDefault );
  }
  //----------------------------------------------------------------------------
  //  All
  //----------------------------------------------------------------------------
  else if( strCommand == "ALL" )
  {
    RobotServo( sizeof( g_ActionLeftHandUp ), g_ActionLeftHandUp );  
    
    RobotServo( sizeof( g_ActionRightHeadUp ), g_ActionRightHeadUp );  
    
    RobotServo( sizeof( g_ActionHurrah ), g_ActionHurrah );  
    
    RobotServo( sizeof( g_ActionHeadFront ), g_ActionHeadFront ); 

    RobotServo( sizeof( g_ActionHandSide ), g_ActionHandSide ); 
          
    RobotServo( sizeof( g_ActionDefault ), g_ActionDefault ); 
  }      
  //----------------------------------------------------------------------------
  //  Head Left Up
  //----------------------------------------------------------------------------
  else if( strCommand == "HLU" )
  {
    RobotServo( sizeof( g_ActionLeftHandUp ), g_ActionLeftHandUp );   
  }
  //----------------------------------------------------------------------------
  //  Head Right Up
  //----------------------------------------------------------------------------
  else if( strCommand == "HRU" )
  {
    RobotServo( sizeof( g_ActionRightHeadUp ), g_ActionRightHeadUp );      
  }      
  //----------------------------------------------------------------------------
  //  Head Hurrah
  //----------------------------------------------------------------------------
  else if( strCommand == "HU" )
  {
    RobotServo( sizeof( g_ActionHurrah ), g_ActionHurrah );       
  }
  //----------------------------------------------------------------------------
  //  Hand Front
  //----------------------------------------------------------------------------
  else if( strCommand == "HF" )
  {
    RobotServo( sizeof( g_ActionHeadFront ), g_ActionHeadFront );       
  }
  //----------------------------------------------------------------------------
  //  Hand Side
  //----------------------------------------------------------------------------
  else if( strCommand == "HS" )
  {
    RobotServo( sizeof( g_ActionHandSide ), g_ActionHandSide );
  }   
  //----------------------------------------------------------------------------
  //  Head
  //----------------------------------------------------------------------------

  //
  //  Move Left
  //
  else if( strCommand == "HEADLEFT" )
  {
      g_ActionCommand[0][0] = g_NowAngle[0] + 15;
      g_ActionCommand[0][ ACTDATA_MAX-1 ] = 500;
    
      RobotServo( sizeof( g_ActionCommand ), g_ActionCommand );     
  }
  //
  //  Move Right
  //
  //
  else if( strCommand == "HEADRIGHT" )
  {
      g_ActionCommand[0][0] = g_NowAngle[0] - 15;
      g_ActionCommand[0][ ACTDATA_MAX-1 ] = 500;
      
      RobotServo( sizeof( g_ActionCommand ), g_ActionCommand );        
  }
  //----------------------------------------------------------------------------
  //  Motor Control
  //----------------------------------------------------------------------------
  else
  {

    if( strCommand.charAt(0) != 'C' ) return;
    
    bool bCommandRun = false;

    for( int i=1 ; i < (int)strCommand.length(); i += 3 )
    {  
      String strMotorAngle = strCommand.substring(i, i+3);
      int nIndex = i / 3;
      
      Serial.print( nIndex );
      Serial.print( ":" );
      Serial.println( strMotorAngle );

      int nAngle = strMotorAngle.toInt();

      if( nAngle >= g_nTrimMinMax[nIndex][1] &&
          nAngle <= g_nTrimMinMax[nIndex][2] ) 
      {
        g_ActionCommand[0][nIndex] = nAngle;

        bCommandRun = true;
      }
      else
      {
        g_ActionCommand[0][nIndex] = -1;
      }
    }

    if( bCommandRun )
    {
      RobotServo( sizeof( g_ActionCommand ), g_ActionCommand );      
    } 
  }

}
