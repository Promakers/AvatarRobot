#!/usr/bin/env python
from threading import Thread
import json
from flask import Flask, render_template, Response
from flask_socketio import SocketIO, emit
import cv2

from opencv_cam import CobitOpenCVCam


import serial


serial_port = serial.Serial(
    port="/dev/ttyUSB0",
    baudrate=9600,
    bytesize=serial.EIGHTBITS,
    parity=serial.PARITY_NONE,
    stopbits=serial.STOPBITS_ONE,
)

def gen_frames():  # generate frame by frame from camera
   
    while True:
        yield (b'--frame\r\n'
                b'Content-Type: image/jpeg\r\n\r\n' + cam.get_jpeg() + b'\r\n')  # concat frame one by one and show result

app = Flask(__name__)
app.config['SECRET_KEY'] = 'secret!'
socketio = SocketIO(app, async_mode=None)

angle       = 0.0
throttle    = 0.0
command     = 'none'

@app.route('/')
def index():
    return render_template('index.html', async_mode=socketio.async_mode)

@app.route('/video_feed',methods = ['GET'])
def video_feed():
    return Response(gen_frames(), mimetype='multipart/x-mixed-replace; boundary=frame')


@socketio.event
def my_event(message):
    global angle, throttle
    data = json.loads(message)
    angle = float(data['angle']) * 100 + 90
    throttle = float(data['throttle']) * 100 
    print(str(angle)+"  "+str(throttle))
#    vc.motor_control(angle, throttle)
   
    command = data['command']
    print( command );

    #
    #   Command Pasing
    #
    if command == "stop":   
        serial_port.write('#S\n'.encode())
    elif command == "all":
        serial_port.write('#ALL\n'.encode())
    elif command == "handhurrah":
        serial_port.write('#HU\n'.encode())
    elif command == "handleft":
        serial_port.write('#HLU\n'.encode())        
    elif command == "handright":
        serial_port.write('#HF\n'.encode())
    elif command == "handside":
        serial_port.write('#HS\n'.encode())
    elif command == "headleft":
        serial_port.write('#HEADLEFT\n'.encode())
    elif command == "headright":
        serial_port.write('#HEADRIGHT\n'.encode())
    else:        
        print( "none" )

@socketio.event
def my_connect(message):
    print(message)




if __name__ == '__main__':
    cam = CobitOpenCVCam()
    t = Thread(target=cam.run, args=())
    t.daemon = True
    t.start()
  
    socketio.run(app, host='192.168.10.102', port=5000)
