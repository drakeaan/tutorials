import azure.functions as func
import datetime
import json
import logging
import os

monitor = func.Blueprint()


@monitor.timer_trigger(schedule="0 * * * * *", arg_name="myTimer", run_on_startup=False,
              use_monitor=False) 
def realtime_monitoring(myTimer: func.TimerRequest) -> None:
    if myTimer.past_due:
        logging.info('The timer is past due!')

    logging.info('Python timer trigger function executed.')

    datadog_api_key = os.environ["Datadog.ApiKey"]

