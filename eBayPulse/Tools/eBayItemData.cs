using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using eBayPulse.Models;

namespace eBayPulse.Tools
{
    public class eBayItemData
    {
        public string ItemID {get; set;}
        public string Name {get; set;}
        public int HitCount {get; set;}
        public int WatchCount {get; set;}
        public long responseTime {get; set;}
        private string response;       
        public eBayItemData(string ItemId){
            HttpRequestReceiver httpRequest = new HttpRequestReceiver(){id = ItemId};
            response = httpRequest.stringResponse;
            responseTime = httpRequest.ResponseTime;
            ItemID = getItem("ItemID");
            HitCount = Convert.ToInt32(getItem("HitCount"));
            Name = getItem("Title");
            //WatchCount = Convert.ToInt32(getItem("WatchCount"));
        }
        public string getItem(string itemName)
        {
            try{
                XDocument xdoc = XDocument.Load(new StringReader(response));
                return reqGetItem(xdoc.Root?.Elements().ToList(), itemName);
            }
            catch(InvalidOperationException)
            {
                return "-1";
            }
        }
        public string reqGetItem(List<XElement> xElements, string itemName)
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
                        var res = reqGetItem(xElement.Elements().ToList(), itemName);
                        if(res != string.Empty)
                        {
                            return res;
                        }
                    }
                }
                return string.Empty;
            }
            catch(InvalidOperationException /*e*/)
            {
                return "-1";
            }
        }
    }
}
