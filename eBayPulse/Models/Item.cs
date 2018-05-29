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
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using System.Xml.Linq;

namespace eBayPulse
{
    public partial class Item
    {       
        public long HitCount {get; set;}
        public Item(string ItemId, string response){
            eBayId = getItem(response, "ItemID");
            HitCount = Convert.ToInt64(getItem(response, "HitCount"));
        }
        public string getItem(string response, string itemName)
        {
            try{
                XDocument xdoc = XDocument.Load(new StringReader(response));
                return reqGetItem(xdoc.Root?.Elements().ToList(), itemName);
            }
            catch(InvalidOperationException /*e*/)
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
