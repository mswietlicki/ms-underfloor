from w1thermsensor import W1ThermSensor
import time
import requests
import RPi.GPIO as GPIO

GPIO.setmode(GPIO.BCM)
GPIO.setwarnings(False)

GPIO.setup(14, GPIO.OUT)
GPIO.output(14, GPIO.HIGH)
heaterOn = False

url = 'https://ms-underfloor.azurewebsites.net/api/temps'
while True:
    temps = []
    for sensor in W1ThermSensor.get_available_sensors():
        temp = sensor.get_temperature()
        temps.append(temp)
    if temps[0] > 30:
        heaterOn = True
        GPIO.output(14, GPIO.LOW)
    else:
        heaterOn = False
        GPIO.output(14, GPIO.HIGH)

    requests.post(url, json={'temps': temps,
                             'state': 'ON' if heaterOn else 'OFF'})
    time.sleep(5)
