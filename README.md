# eBay Pulse

You can open GUI of eBayPulse in the browser using the following addresses:

* [https://localhost:5001/](https://localhost:5001/)
* [https://127.0.0.1:5001/](https://127.0.0.1:5001/)

eBayPulse uses HTTPS protocol. Therefore, the address should start with "https" (not "http"). The browser can issue a warning about an invalid certificate. To avoid this, import **BravikovCertificateAuthorityCertificate.pem** file to list of Certificate Authorities in browser settings. On Windows you should import the file into "Trusted Root Certification Authorities". You can find this file in **Certificate** folder. Or you can use own certificate. Just replace **eBayPulseCertificate.pfx** file in **Certificate** folder. The pfx file must have a blank password.

## Tests

1. Install the .NET Core Test Explorer extension
2. Go to .NET Test Explorer of Explorer view, all the tests will be automatically detected, and you are able to run all tests or a certain test
