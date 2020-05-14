from w1thermsensor import W1ThermSensor
import time
import requests
import RPi.GPIO as GPIO

GPIO.setmode(GPIO.BCM)
GPIO.setwarnings(False)

GPIO.setup(14, GPIO.OUT)
GPIO.output(14, GPIO.HIGH)
heaterOn = False

url = 'https://webhook.site/3b6578b3-3716-4a7c-a509-c0f45ef4b2ff'
while True:
	temps = []
	for sensor in W1ThermSensor.get_available_sensors():
		temp = sensor.get_temperature()
		temps.append(temp)
	if temps[0] > 30:
        	GPIO.output(14, GPIO.LOW)
	else:
		GPIO.output(14, GPIO.HIGH)
	requests.post(url, json=temps)
	time.sleep(5)
