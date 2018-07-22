using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using eBayPulse.Tools;
using eBayPulse.Models;


namespace eBayPulse.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            DBConnector.getConnection().context.Database.EnsureCreated();
            return View();
        }       

        [HttpPost]
        public string Index(string msg)
        {
            eBayItemIdCleaner eBayItemId = new eBayItemIdCleaner(msg);
            if(eBayItemId.IsValid)
            {                
                eBayItemData eBayItem = new eBayItemData(eBayItemId.Value);
                Item item = new Item(eBayItem);
                DBConnector.getConnection().context.Item.Add(item);
                DBConnector.getConnection().context.SaveChanges();
                Pulse newPulse = new Pulse(eBayItem, item);
                DBConnector.getConnection().context.Pulse.Add(newPulse);
                DBConnector.getConnection().context.SaveChanges();
                return item.Pulses.FirstOrDefault().Views.ToString();// < 0 ? HttpRequestReceiver.ExceptionsList[item.HitCount] : item.HitCount.ToString());
            }
            else
            {
                return string.Empty;
            }
        }

    }
}