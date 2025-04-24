# renamed to __init__.py
# does this work like startup.cs / program.cs? inherently functioning as first-tier initialization?
# TODO: figure out the reason for __init__.py 

import logging 
import azure.functions as func

app = func.FunctionApp()

# so python differs here from C#...
# @ defines the decorator attribute...
# the main function specifies the return type after the signature using ->. in this case, a str return type.
# TODO: figure out async as well as multiple return types (if at all).
# e.g. (str, str) returnVals(str abc){}. What would that look like in python
# also to do: get a keyboard where I and O are swopped so I can stop typing pythin
@app.function_name(name="HttpTrigger1")
# req --> Request object. in this case, should the Azure function HttpRequest.
def main(req:func.HttpRequest) -> str:
    print(req.params.get(''))

    return "foo"

# so same async keyword as C# and also same await keyword when calling an async method.
# e.g. await main(blah)
# TODO: Also, do overloads function the same? TBD
async def main(req:func.HttpRequest, ctx:func.ExecutionContex) -> func.HttpResponse:
    logging.info('Python http trigger')
    # can construct an object inline...
    # TODO: determine new()
    return func.HttpResponse( 
            "This HTTP-triggered function executed successfully. " 
            "Some more text.", 
            status_code=200 
        )

# overall, the structure is similar... just syntactically slightly different (but not that much).
# need to just figure out imports compared to usings (which objects are where..)
# also remember which lines of code ends with : compared to C# everything ends with ;