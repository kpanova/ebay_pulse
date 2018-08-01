using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using eBayPulse.Models;
using System.Threading.Tasks;

namespace eBayPulse.Tools
{
    public class eBayItemDataHelper
    {
        public string ItemID {get; set;}
        public string Name {get; set;}
        public int HitCount {get; set;}
        public int WatchCount {get; set;}
        public long ResponseTime {get; set;}
        private string response;       
        public eBayItemDataHelper(string ItemId){
            this.ItemID = ItemId;
        }
        public async void GeteBayItemDataHelperAsync()
        {
            await GeteBayItemDataHelper();
        }
        public Task GeteBayItemDataHelper()
        {
            HttpRequestReceiver httpRequest = new HttpRequestReceiver() { id = ItemID };
            response = httpRequest.StringResponse;
            ResponseTime = httpRequest.ResponseTime;
            ItemID = GetItem("ItemID");
            HitCount = Convert.ToInt32(GetItem("HitCount"));
            Name = GetItem("Title");
            return Task.CompletedTask;
        }
        public string GetItem(string itemName)
        {
            try{
                XDocument xdoc = XDocument.Load(new StringReader(response));
                return ReqGetItem(xdoc.Root?.Elements().ToList(), itemName);
            }
            catch(InvalidOperationException)
            {
                return "-1";
            }
        }
        public string ReqGetItem(List<XElement> xElements, string itemName)
        {
            try{
                foreach(var xElement in xElements)
                {
                    if(xElement.Name.LocalName == itemName)
                    {
                        return xElement.Value;
                    }
                    else if (xElement.HasElements)
                    {
                        var res = ReqGetItem(xElement.Elements().ToList(), itemName);
                        if(res != string.Empty)
                        {
                            return res;
                        }
                    }
                }
                return string.Empty;
            }
            catch(InvalidOperationException)
            {
                return "-1";
            }
        }
    }
}
