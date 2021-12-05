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


## Folder List

### JetsonNano
  + Web Interface를 통한 카메라 확인 및 로봇 실시간 제어
  + 아바타 로봇과 Serial통신

### Arduino
  + 아바타 로봇 관절 (서보 7개) 제어
  + 아바타 로보트 Serial 통신 / Bluetooth 통신 규약
  
### Unity
  + 아바타 로봇 시뮬레이터
  + 사용자 동작 인식 및 아바타 로봇 제어

### AI 이미지
  + 이미지 Size Up
  

 ## 보드 
 
  * Jetson Nano
  * Arduino
