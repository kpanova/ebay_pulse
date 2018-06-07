using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using eBayPulse.Tools;
using eBayPulse.Models;
using Microsoft.EntityFrameworkCore;

namespace eBayPulse.Controllers
{
    public class HomeController : Controller
    {
        eBayPulseContext context => DBConnector.getConnection().context;
        public IActionResult Index()
        {
            context.Database.EnsureCreated();
            ViewData["items"] = context.Item.Include(c => c.Pulses);
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
                context.Item.Add(item);
                context.SaveChanges();
                Pulse newPulse = new Pulse(eBayItem, item);
                context.Pulse.Add(newPulse);
                context.SaveChanges();
                return (item.Id +";"+ item.Name + ";"+ item.Pulses.LastOrDefault().Unix_Time.ConvertFromUnixTimestamp() +";"+ item.Pulses.LastOrDefault().Views.ToString());
            }
            else
            {
                return string.Empty;
            }
        }

    }
}