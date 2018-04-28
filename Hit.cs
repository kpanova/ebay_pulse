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

namespace eBayPulse
{
    public class Hit
    {
        private readonly HttpClient client = new HttpClient();
        private HttpResponseMessage response;
        public string Response{
            get{                
                if(client!=null)
                {                    
                    ProcessRepositories().Wait();
                }
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        private async Task ProcessRepositories()
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("text/xml"));
            client.DefaultRequestHeaders.Add("X-EBAY-API-COMPATIBILITY-LEVEL", "967");
            client.DefaultRequestHeaders.Add("X-EBAY-API-DEV-NAME", "fb50a92e-3411-45b9-b3be-f9f68e73a5b3");
            client.DefaultRequestHeaders.Add("X-EBAY-API-APP-NAME", "DmitryBr-bravikov-PRD-351ca6568-5bc3fc72");
            client.DefaultRequestHeaders.Add("X-EBAY-API-CERT-NAME", "PRD-51ca6568f1c6-5224-494f-8381-fdf0");
            client.DefaultRequestHeaders.Add("X-EBAY-API-CALL-NAME", "GetItem");
            client.DefaultRequestHeaders.Add("X-EBAY-API-SITEID", "0");
            string xml =               
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>/n"+
                "<GetItemRequest xmlns=\"urn:ebay:apis:eBLBaseComponents\">/n"+
                "<RequesterCredentials>/n"+
                "<eBayAuthToken>AgAAAA**AQAAAA**aAAAAA**1splWg**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6wNmIagD5aLqQudj6x9nY+seQ**HhsEAA**AAMAAA**nZ6wYJ100NeoJUeiS74XakKjvLZpXpgZJFS+zq3vi48Fltx2miuS7bWGb/XVoHF7paS36l5P/rHKpGlKWftFrHjzp47KTG+mJVTb1jqXbaUSSK0bn9krHBDWHr6PP+gvlm4TXRdNSlTIF5XRjVnhmshuyEdgv0vde02koXgWZX5/sy+nT/WLnztoeYex6iukzHZBw+r2Mz7XRJ2n0DfLRqyBGJQmrOO4qywRjAAoeCLrqm+RDCcDiZS2lVh097eGPgTUnRmAyb57xHH4dhIUUDvceHCVDe4kjWkE0GaRSVWWBmubodq2lZP4M8CHIv5VxtV3F7GvXSRGtpKHgJQrWaPJivfoELpXKIMkyls+6ZIyml+oNGmyFNX5oZpWnFsuregiIGAcHTJRUMPq1pSHvVHvLTtdQzPJ0uGDQu8a8O43nteBpbsIIN9oW2FkJVaWHMvPL7BM/q+oSk76vj/qX0FnLxO4J9y4Mpo2gXCkGwYjgaJpyNOOEcjrqt5fVRZwzr7vzkH2dDguYgKgjXheqrnDAKfP63Gy9ksWzfVcJzsHvM7KUNpccLmy7Ug2mDJPwjAdfr0jerzoL7zuIXh31m505TcqdfGdkay7PBfQo7HTKaIZxuOvjM1ZxZsjagRmMenvoi7fPfWzgaWNdjAODv5ns4KKwzDRAVz3+ifPWlE3BK7XsOJROI7x+XU62WaN5RwA3xNymZqDqtwET3UVwkFB5bWj0GLExzpBq5YG6P3wbK/M5amQsXc6rWvrDlFu</eBayAuthToken>/n"+
                "</RequesterCredentials>/n"+
                "<ItemID>282569628293</ItemID>/n"+
                "</GetItemRequest>";

            ByteArrayContent byteArrayContent = new ByteArrayContent(Encoding.ASCII.GetBytes(xml));

            var stringTask = client.PostAsync("https://api.ebay.com/ws/api.dll", byteArrayContent);

            response = await stringTask;
        }
    }
}