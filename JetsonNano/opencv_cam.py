import cv2
import time
import numpy as np

# gstreamer_pipeline returns a GStreamer pipeline for capturing from the CSI camera
# Defaults to 1280x720 @ 60fps
# Flip the image by setting the flip_method (most common values: 0 and 2)
# display_width and display_height determine the size of the window on the screen
def gstreamer_pipeline(
    capture_width=1280,
    capture_height=720,
    display_width=320,
    display_height=240,
    framerate=5,
    flip_method=0,
):
    return (
        "nvarguscamerasrc ! "
        "video/x-raw(memory:NVMM), "
        "width=(int)%d, height=(int)%d, "
        "format=(string)NV12, framerate=(fraction)%d/1 ! "
        "nvvidconv flip-method=%d ! "
        "video/x-raw, width=(int)%d, height=(int)%d, format=(string)BGRx ! "
        "videoconvert ! "
        "video/x-raw, format=(string)BGR ! appsink"
        % (
            capture_width,
            capture_height,
            framerate,
            flip_method,
            display_width,
            display_height,
        )
    )

class CobitOpenCVCam:
    
    # OpenCV and camera init
    def __init__(self):
        
        self.frame = None
        self.ret = False
        #self.cap = cv2.VideoCapture( 0 )

        self.cap = cv2.VideoCapture(gstreamer_pipeline(flip_method=0), cv2.CAP_GSTREAMER)

        #self.cap.set(3, 320)
        #self.cap.set(4, 240)
        #self.jpeg = None
        
    #   Del Open CV    
    def __del__(self):
        if self.cap.isOpened():
           self.cap.release()

    def get_jpeg(self):
        return self.jpeg.tobytes()

    # send jpeg image
    def run(self):
        while True:
            ret, self.frame = self.cap.read()
            ret, self.jpeg = cv2.imencode('.jpg', self.frame)
            
        self.cap.release()
           
    def set_lane_detect(self, value):
        self.lane_detect = value

    def get_lane_detect(self):
        return self.lane_detect

    def set_recording_status(self, value):
        print(value)
        self.recording = value

    def get_recording_status(self):
        return self.recording


if __name__ == '__main__':
    cam = CobitOpenCVCam()
    cam.update() 

