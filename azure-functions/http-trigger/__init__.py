# renamed to __init__.py
# does this work like startup.cs / program.cs? inherently functioning as first-tier initialization?
# TODO: figure out the reason for __init__.py 


# From google...
"""
The __init__.py file serves to mark a directory as a Python package. 
When the Python interpreter encounters a directory containing this file, it recognizes the directory as a package, 
allowing modules within it to be imported using dot notation. 

In its simplest form, __init__.py can be an empty file. However, it can also contain initialization code that executes when the package is imported. 
This may include setting package-level variables, importing submodules, or running setup operations. 
It can also control what symbols are exported from the package, which is useful for selectively importing symbols and avoiding namespace clutter. 

While __init__.py was historically required for all packages, Python 3.3 introduced implicit namespace packages, making it optional in some cases. 
However, it remains best practice to include it for clarity and compatibility, especially when needing to define package-level initialization or control imports.
"""

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
def main(req:func.HttpRequest) -> str:   # req --> Request object. in this case, should the Azure function HttpRequest.
    print(req.params.get(''))

    t = "foo" 
    x = "bar"
    # so f works as string interpolation. e.g. $"{t}";
    # explicitly formatting would be: "{0} {1}".format(t, x)
    return f"{t} {x}"

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

