import azure.functions as func
import datetime
import json
import logging
from blob import *

# so... this is an interesting find. 
# InvokeMonitoring.realtime_monitoring converts to: folder/file.py
# so in Python, the "namespace" means a file that needs to be imported requires its path separated by '.'
from InvokeMonitoring.realtime_monitoring import *

# So this is the MAIN entrypoint... 
# and we can register functions
app = func.FunctionApp()
app.register_functions(blob) 
app.register_functions(realtime_monitoring) 

@app.route(route="HttpExample")
def HttpExample(req: func.HttpRequest) -> func.HttpResponse:
    logging.info('Python HTTP trigger function processed a request.')

    name = req.params.get('name')
    if not name:
        try:
            req_body = req.get_json()
        except ValueError:
            pass
        else:
            name = req_body.get('name')

    if name:
        return func.HttpResponse(f"Hello, {name}. This HTTP triggered function executed successfully.")
    else:
        return func.HttpResponse(
             "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response.",
             status_code=200
        )


@app.queue_trigger(arg_name="azqueue", queue_name="myqueue",
                               connection="AzureWebJobsStorage") 
def queue_trigger(azqueue: func.QueueMessage):
    logging.info('Python Queue trigger processed a message: %s',
                azqueue.get_body().decode('utf-8'))


