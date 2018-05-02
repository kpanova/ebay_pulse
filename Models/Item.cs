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
    public class Item
    {
        public string ItemID {get; set;}
        public long HitCount {get; private set;}
        
        public Item(string ItemId, string response){
            ItemID = ItemId;
            HitCount = Convert.ToInt64(getItemFromXml(response, "HitCount"));
        }
        public Item(){

        }
        public string getItemFromXml(string response, string itemName)
        {
            XDocument xdoc = XDocument.Load(new StringReader(response));
            return xdoc.Root.Elements().Where(y => y.Name.LocalName == this.ToString().Split('.').Last()).Last().Elements().Where(x => x.Name.LocalName == itemName).Last().Value;
        }
    }    
}