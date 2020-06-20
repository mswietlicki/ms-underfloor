from w1thermsensor import W1ThermSensor
import time
import requests
import json
import RPi.GPIO as GPIO

GPIO.setmode(GPIO.BCM)
GPIO.setwarnings(False)

GPIO.setup(14, GPIO.OUT)
GPIO.output(14, GPIO.HIGH)
heaterOn = False


def read_config():
    try:
        r = requests.get('https://ms-underfloor.azurewebsites.net/api/config')
        if r.status_code == 200:
            config = r.json()
            with open('config.json', 'w') as f:
                json.dump(config, f)
        else:
            raise Exception('Invalid status code '+ str(r.status_code))
    except Exception as e:
        print('Config download exception: ' + str(e))
        with open('config.json', 'r') as f:
            config = json.load(f)
    print('config: ' + json.dumps(config))
    return config


def post_temps(report):
    print('temps: ' + json.dumps(report))
    requests.post(
        'https://ms-underfloor.azurewebsites.net/api/temps', json=report)


while True:
    try:
        config = read_config()
        temps = []
        for sensor in W1ThermSensor.get_available_sensors():
            temp = sensor.get_temperature()
            temps.append(temp)
        if temps and config and temps[0] < config['heaterLimitTemp']:
            heaterOn = True
            GPIO.output(14, GPIO.LOW)
        else:
            heaterOn = False
            GPIO.output(14, GPIO.HIGH)

        post_temps({'temps': temps, 'state': 'ON' if heaterOn else 'OFF'})
        time.sleep(60)
    except Exception as e:
        print('Exception: ' + str(e))
