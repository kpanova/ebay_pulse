using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Net;
using System.IO;

namespace eBayPulse.Models
{
    public class HttpRequestReceiver
    {
        private readonly HttpClient client = new HttpClient();
        private HttpResponseMessage response;
        private Dictionary<string, string> authData;
        public string id;
        public static Dictionary<long,string> ExceptionsList = new Dictionary<long, string>(){
            {-1, "This item was not found."}
        };
        public string stringResponse
        {
            get
            {
                if(client != null)
                {
                    ProcessRepositories().Wait();
                }
                if(response.IsSuccessStatusCode){
                    var result = response.Content.ReadAsStringAsync().Result;
                    ResponseTime = (new DateTimeOffset(DateTime.Now)).ToUnixTimeSeconds();
                    return result;
                }
                else
                {
                    return "Code: " + response.StatusCode.ToString() + " is " + (int)response.StatusCode;
                }
            }
        }
        public long ResponseTime {get; private set;}
        private async Task ProcessRepositories( )
        {
            getToken();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("text/xml"));
            client.DefaultRequestHeaders.Add("X-EBAY-API-COMPATIBILITY-LEVEL", "967");
            client.DefaultRequestHeaders.Add("X-EBAY-API-DEV-NAME", "fb50a92e-3411-45b9-b3be-f9f68e73a5b3");
            client.DefaultRequestHeaders.Add("X-EBAY-API-APP-NAME", "DmitryBr-bravikov-PRD-351ca6568-5bc3fc72");
            client.DefaultRequestHeaders.Add("X-EBAY-API-CERT-NAME", authData["CERT-NAME"]);
            client.DefaultRequestHeaders.Add("X-EBAY-API-CALL-NAME", "GetItem");
            client.DefaultRequestHeaders.Add("X-EBAY-API-SITEID", "0");
            string xml =               
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>/n" +
                "<GetItemRequest xmlns=\"urn:ebay:apis:eBLBaseComponents\">/n" +
                    "<IncludeWatchCount>True</IncludeWatchCount>/n" +
                    "<RequesterCredentials>/n" +
                    "<eBayAuthToken>" + authData["eBayAuthToken"] + "</eBayAuthToken>/n" +
                    "</RequesterCredentials>/n" +
                    "<ItemID>" + id + "</ItemID>/n" +
                    "</GetItemRequest>";

            ByteArrayContent byteArrayContent = new ByteArrayContent(Encoding.ASCII.GetBytes(xml));
            var stringTask = client.PostAsync("https://api.ebay.com/ws/api.dll", byteArrayContent);

            response = await stringTask;
        }
        private void getToken(){
            authData = new Dictionary<string, string>();
            using (StreamReader  token = new StreamReader("Token.txt"))
            {
                authData.Add("CERT-NAME", token.ReadLine());
                authData.Add("eBayAuthToken", token.ReadLine());
            }
        }
    }
}