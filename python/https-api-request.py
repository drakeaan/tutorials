import requests
import json
import ssl

# The API endpoint
url = "https://api.drawloop.com/v5.0/rest/runs"

# Required headers
headers = {
    "Content-Type": "application/json",
    "Authorization": "userId=testing.framework@nintex.com,password=r3e51Jk0xyWr,key=lqx1hk3teozqzyijhln5"
}

# Request body
data = '''
{
    "locale" : "en-US",
    "currency" : "USD",
    "timezone" : "US/Pacific",
    "files" : [{
        "id" : "b09b604f-ea74-46a7-b8b8-406225f723c1",
        "source" : {
          "url" : "https://www.smartsheet.com/file/smart-goals-template.docx",
          "name" : "2b966dd8-79b3-441f-b54f-528d9686bf8c"
        },
        "mergeWithPrevious" : false,
        "wordBreakType" : null
      }
    ],
    "outputOptions" : {
      "outputFilename" : "2b966dd8-79b3-441f-b54f-528d9686bf8c",
      "pdfOptions" : "Enhanced",
      "outputType" : "pdf",
      "stamps" : []
    },
    "data" : [
        {
        "name": "StartEvent",
        "copyType": "None",
        "columns": [{
            "name": "text_short_1_8jaSN5pNL",
            "dataType": null
            }, {
            "name": "text_long_1_abYG41cFP",
            "dataType": null
            }
        ],
        "rows": [{
            "values": ["form short 2", "form long 2"],
            "children": null
            }
        ],
        "groupByRules": []
        },
        {
        "name": "Variable",
        "copyType": "None",
        "columns": [{
            "name": "textvar",
            "dataType": null
            }, {
            "name": "text2var",
            "dataType": null
            }
        ],
        "rows": [{
            "values": ["hello", "Hello 2"],
            "children": null
            }
        ],
        "groupByRules": []
        },
        {
            "name": "Case",
            "copyType": "Row",
            "columns": [{
                "name": "Header1",
                "dataType": null
                }, {
                "name": "Header2",
                "dataType": null
                }
            ],
            "rows": [{
                "values": ["value for header1", "value for header2"],
                "children": null
                },
                {
                "values": ["another header1 val", "another header2 val"],
                "children": null
                },
                {
                "values": ["another header1 row", "another header2 row"],
                "children": null
                },
                {
                "values": ["another header1 row2", "another header2 row2"],
                "children": null
                }
            ],
            "groupByRules": []
        }
    ],
    "statusNotification" : {
      "url" : "https://8c8049b880a6c0acbe381395c5542527.m.pipedream.net",
      "format" : "JSON"
    }
}
'''

# Get reqeust body as a JSON object
body = json.loads(data)

# A GET request to the API
response = requests.post(url,headers=headers,json=body,verify=True)

# Get the TLS Version used
print(requests.get('https://www.howsmyssl.com/a/check',verify=True).json()['tls_version'])
# Get the OpenSSL library version installed
print(ssl.OPENSSL_VERSION)

# Print the response
response_json = response.json()

print(response_json)