


import azure.functions as func

app = func.FunctionApp()

# so python differs here from C#...
# @ defines the decorator attribute...
# the main function specifies the return type after the signature using ->. in this case, a str return type.
# TODO: figure out async as well as multiple return types (if at all).
# e.g. (str, str) returnVals(str abc){}. What would that look like in python
# also to do: get a keyboard where I and O are swopped so I can stop typing pythin
@app.function_name(name="HttpTrigger1")
def main(req:func.HttpRequest) -> str:
