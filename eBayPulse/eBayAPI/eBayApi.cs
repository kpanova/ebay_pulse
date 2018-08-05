using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using eBayPulse.Tools;

namespace eBayPulse.eBayApi
{
    public enum Gateway
    {
        Sandbox,
        Production,
    }

    public enum CallName
    {
        FetchToken,
        GeteBayOfficialTime,
        GetItem,
        GetSessionID,
    }

    public enum SiteId
    {
        Australia               =  15,
        Austria                 =  16,
        BelgiumDutch            = 123,
        BelgiumFrench           =  23,
        Canada                  =   2,
        CanadaFrench            = 210,
        France                  =  71,
        Germany                 =  77,
        HongKong                = 201,
        India                   = 203,
        Ireland                 = 205,
        Italy                   = 101,
        Malaysia                = 207,
        Netherlands             = 146,
        Philippines             = 211,
        Poland                  = 212,
        Russia                  = 215,
        Singapore               = 216,
        Spain                   = 186,
        Switzerland             = 193,
        UK                      =   3,
        US                      =   0,
    }

    public class Keys
    {
        public Keys(string dev, string app, string cert)
        {
            Dev = dev;
            App = app;
            Cert = cert;
        }

        public bool Empty()
        {
            return Dev == null || App == null || Cert == null;
        }

        public string Dev { get; private set; }
        public string App { get; private set; }
        public string Cert { get; private set; }
    }

    public class Headers
    {
        public Headers(int compatibilityLevel, CallName callName, SiteId siteId)
        {
            CallName = callName;
            SiteId = siteId;
            CompatibilityLevel = compatibilityLevel;
        }

        public Headers(
            int compatibilityLevel,
            CallName callName,
            SiteId siteId,
            Keys keys
        )
            : this (compatibilityLevel, callName, siteId)
        {
            Keys = keys;
        }

        public CallName CallName { get; private set; }
        public SiteId SiteId { get; private set; }
        public int CompatibilityLevel { get; private set; }
        public Keys Keys { get; private set; }
    }

    public class Context
    {
        public Gateway Gateway = Gateway.Sandbox;
        public int CompatibilityLevel = 1067;
        public SiteId SiteId = SiteId.US;
        public string RuName = null;
        public Keys Keys = null;
        public string Token = null;
    }

    public class SignInPage
    {
        static public string GetUrl(Gateway gateway, string ruName, string sessionId)
        {
            if (string.IsNullOrEmpty(ruName) || string.IsNullOrEmpty(sessionId))
            {
                return null;
            }

            var domen = Request.GatewayDomen(gateway);
            if (string.IsNullOrEmpty(domen)) {
                return null;
            }

            return $"https://signin.{domen}/ws/eBayISAPI.dll?SignIn&RuName={ruName}&SessID={sessionId}";
        }

        static public string GetUrl(Context context, string sessionId)
        {
            return GetUrl(context.Gateway, context.RuName, sessionId);
        }
    }

    public class Request
    {
        public Request(Gateway gateway, Headers header, string inputXml)
        {
            Gateway = gateway;
            Headers = header;
            InputXml = inputXml;
        }

        public async Task exec()
        {
            fillHttpHeader();
            ByteArrayContent byteArrayContent
                = new ByteArrayContent(Encoding.ASCII.GetBytes(InputXml));
            var stringTask = Client.PostAsync(GetApiUrl(), byteArrayContent);
            response = await stringTask;
        }

        public HttpResponseMessage response { get; private set; }
        HttpClient Client = new HttpClient();

        private Gateway Gateway;
        private Headers Headers;
        private string InputXml;

        private string GetApiUrl()
        {
            var domen = GatewayDomen(Gateway);
            if (string.IsNullOrEmpty(domen)) {
                return null;
            }
            return $"https://api.{domen}/ws/api.dll";
        }

        static public string GatewayDomen(Gateway gateway)
        {
            switch (gateway)
            {
                case Gateway.Sandbox:
                    return "sandbox.ebay.com";
                case Gateway.Production:
                    return "ebay.com";
            }
            return null;
        }

        private void fillHttpHeader()
        {
            Client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("text/xml")
            );
            Client.DefaultRequestHeaders.Add(
                "X-EBAY-API-COMPATIBILITY-LEVEL",
                Headers.CompatibilityLevel.ToString()
            );
            Client.DefaultRequestHeaders.Add(
                "X-EBAY-API-CALL-NAME",
                Headers.CallName.ToString()
            );
            Client.DefaultRequestHeaders.Add(
                "X-EBAY-API-SITEID",
                Convert.ToString((int)Headers.SiteId)
            );

            if (Headers.Keys != null)
            {
                Client.DefaultRequestHeaders.Add(
                    "X-EBAY-API-DEV-NAME",
                    Headers.Keys.Dev
                );
                Client.DefaultRequestHeaders.Add(
                    "X-EBAY-API-APP-NAME",
                    Headers.Keys.App
                );
                Client.DefaultRequestHeaders.Add(
                    "X-EBAY-API-CERT-NAME",
                    Headers.Keys.Cert
                );
            }
        }
    }

    namespace Call {
        public abstract class Call
        {
            public Call(Context context, CallName callName)
            {
                Context = context;
                CallName = callName;
            }

            public bool exec()
            {
                Headers headers = new Headers(
                    Context.CompatibilityLevel,
                    CallName,
                    Context.SiteId,
                    Context.Keys
                );

                string inputXml = string.Format(
                    baseXml(),
                    headers.CallName.ToString(),
                    RequesterCredentials(),
                    CallSpecificInputXml
                );

                var request = new Request(Context.Gateway, headers, inputXml);
                request.exec().Wait();
                var response = request.response;

                if (response == null)
                {
                    return false;
                }

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                var output = response.Content.ReadAsStringAsync().Result;

                if (!ParseOutput(new XmlReader(output)))
                {
                    ErrorMessage
                        = $"Error when parsing output. Output: '{output}'.";
                    return false;
                }

                return true;
            }

            public string ErrorMessage { get; protected set; }

            protected abstract bool ParseOutput(XmlReader xmlReader);

            protected string CallSpecificInputXml { private get; set; }

            private CallName CallName;
            private Context Context;

            private string baseXml()
            {
                return ""
                    + "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n"
                    + "<{0}Request xmlns=\"urn:ebay:apis:eBLBaseComponents\">\n"
                    + "{1}\n"
                    + "{2}\n"
                    + "</{0}Request>\n";
            }

            private string RequesterCredentials()
            {
                if (Context.Token != null) {
                    var token = $"<eBayAuthToken>{Context.Token}</eBayAuthToken>";
                    return $"<RequesterCredentials>{token}</RequesterCredentials>";
                }
                return string.Empty;
            }
        }
    }
}
