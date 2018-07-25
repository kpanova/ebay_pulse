import http.client

cert_id = ''
token = ''

with open('../eBayPulse/Token.txt', 'r', encoding='utf-8') as f:
    certId = f.readline()
    token = f.readline()

class Header:
    def __init__(self, name, value):
        self.name = name
        self.value = value


headers = (
    Header('X-EBAY-API-COMPATIBILITY-LEVEL', '967'),
    #Header('X-EBAY-API-DEV-NAME', 'fb50a92e-3411-45b9-b3be-f9f68e73a5b3'),
    #Header('X-EBAY-API-APP-NAME', 'DmitryBr-bravikov-PRD-351ca6568-5bc3fc72'),
    #Header('X-EBAY-API-CERT-NAME', cert_id),
    Header('X-EBAY-API-CALL-NAME', 'GetItem'),
    Header('X-EBAY-API-SITEID', '0'),
)

xml = '''
    <?xml version="1.0" encoding="utf-8"?>
    <GetItemRequest xmlns="urn:ebay:apis:eBLBaseComponents">
        <IncludeWatchCount>True</IncludeWatchCount>
        <RequesterCredentials>
            <eBayAuthToken>{TOKEN}</eBayAuthToken>
        </RequesterCredentials>
        <ItemID>{ITEM_ID}</ItemID>
    </GetItemRequest>
'''

filled_xml = xml.format(TOKEN=token, ITEM_ID='263834698919')

host = 'api.ebay.com'
url = '/ws/api.dll'

connection_headers = {'Content-type': 'text/xml'}

for header in headers:
    connection_headers[header.name] = header.value

connection = http.client.HTTPSConnection(host)
connection.request("POST", url, filled_xml, connection_headers)
res = connection.getresponse()
print(res.status, res.reason)
data = res.read()
print(data)
