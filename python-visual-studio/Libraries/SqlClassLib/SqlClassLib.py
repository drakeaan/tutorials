
import os
import pyodbc, struct
from azure import identity

from typing import Union
from fastapi import FastAPI
from pydantic import BaseModel

# This is the equivalent of a C# property declaration...
class sqlTable(object):
    def __init__(self):
        self._x = 0

    @property
    def x(self):
        return self._x

    @x.setter
    def x(self, value):
        if value <= 0:
            raise ValueError("Value must be positive")
        self._x = value

# This is the equivalent of a C# model object relating to the structure of the table being queried.
class Person(BaseModel):
    first_name: str
    last_name: Union[str, None] = None

connection_string = os.environ["AZURE_SQL_CONNECTIONSTRING"]

app = FastAPI()

# just using a cursor?? 
# cursor.execute() seems to encompass queries, sprocs, views etc...

def getData():
    rows = []
    conn = get_conn()
    cursor = conn.cursor()
    cursor2 = conn.cursor()

    cursor.execute("SELECT * FROM Persons")

    # not sure I like this... but ok.
    values = ("a", 1, 2, "b")
    sproc = "EXEC @RC = sprocName @a, @b, @c, @d"
    cursor2.execute(sproc, values)

    for row in cursor.fetchall():
        print(row.FirstName, row.LastName)
        rows.append(f"{row.ID}, {row.FirstName}, {row.LastName}")

    rc = cursor2.fetchval()
    return rows


# This is using the Managed Identity / Entra to connect... just as an example.
# at the end of the day, connection_string could be username/password combo as well.
def get_conn():
    credential = identity.DefaultAzureCredential(exclude_interactive_browser_credential=False)
    token_bytes = credential.get_token("https://database.windows.net/.default").token.encode("UTF-16-LE")
    token_struct = struct.pack(f'<I{len(token_bytes)}s', len(token_bytes), token_bytes)
    SQL_COPT_SS_ACCESS_TOKEN = 1256  # This connection option is defined by microsoft in msodbcsql.h
    conn = pyodbc.connect(connection_string, attrs_before={SQL_COPT_SS_ACCESS_TOKEN: token_struct})
    return conn