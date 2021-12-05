# ## 프로젝트 제목

* 아바타 로봇 (학교 출석에 어려운 학생을 위한 대신 출석 로봇)

## 시연 동영상

[![Watch the video](https://tistory2.daumcdn.net/tistory/2946431/skin/images/main_key_events_4005.png)](https://youtu.be/493mAYvdQsk)
* 이미지 클릭시 동영상이 재생 됩니다.


## Pro Makers (김도혁/유수엽/최윤규/권준호/강진호)

* 권준호 : Jetson Nano 카메라 모듈 연동
* 최윤규 : 유니티 시뮬레이션 및 로봇 연동
* 김도혁 : 아바타 로봇 제작
* 유수엽 : 아바타 로봇 제작
* 우성우 : AI를 활용한 이미지 Size Up
* 문규식 : 프로젝트 기획 및 관련 영상 제작

## 프로젝트 배경 혹은 목적

  <img src="https://user-images.githubusercontent.com/46912845/131213363-608d0e82-baed-48f6-909f-dc7a446c14c9.JPG" width="70%"></img>
  <img src="https://user-images.githubusercontent.com/46912845/131213365-8ead6984-0a1f-40b9-9af9-dfd34c7bd5ac.JPG" width="70%"></img>  
  <img src="https://user-images.githubusercontent.com/46912845/131219197-695edbd9-3f0d-40a3-be3b-1dedb05d3cd5.jpg" width="70%"></img>
  
  + MLX90614 비접촉 온도센서 연동 성공
  + HC-SR04 (초음파 센서) - 정확한 Timer 10u Sec제어와 Start Time / End Time 방법을 찾지 못하여 다음번에 도전
  + 카메라 연동
    
  <img src="https://user-images.githubusercontent.com/46912845/131221167-89724ca0-befc-4e6a-9ab2-796677422601.png" width="70%"></img>
   + server_restapi는 Upload된 Image의 마스크 유무 판단하여 JOSN형태로 결과 반환


## 파일 리스트


### UI (최윤규)

  + KioskCafeteriaTutorial 예제를 활용한 키오스크 구현
  + https://github.com/Promakers/TopMaker2021/tree/main/KioskCafeteriaTutorial 참조

### server_restapi (권준호)
  + Web API Server
  + Upload된 이미지의 마스크 유무를 판단하여 JSON형태로 결과 반환
  
### Tizen_respapi_test (권준호)
  + 타이젠에서 Res API 이미지 확인 테스트 코드

### MLX90614 비접촉 온도 센서 (김도혁)

  + 아두이노 https://github.com/adafruit/Adafruit-MLX90614-Library 참조하여 타이젠 C# 클래스 제작  

 ## 보드 
 
  * Jetson Nano
  * Arduino
