using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using eBayPulse.Tools;
using eBayPulse.Models;
using Microsoft.EntityFrameworkCore;
using Highsoft.Web.Mvc.Stocks;

namespace eBayPulse.Controllers
{
    public class HomeController : Controller
    {
        eBayPulseContext context => DBConnector.getConnection().context;
        public IActionResult Index()
        {
            context.Database.EnsureCreated();
            ViewData["items"] = context.Item.Include(c => c.Pulses);
            ViewData["Title"] = "eBayPulse";
            return View();
        }       

        [HttpPost]
        public string Index(string msg)
        {
            eBayItemIdCleaner eBayItemId = new eBayItemIdCleaner(msg);
            if(eBayItemId.IsValid)
            {
                eBayItemDataHelper eBayItem = new eBayItemDataHelper(eBayItemId.Value);
                eBayItem.GeteBayItemDataHelperAsync();
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

        public IActionResult Item(string eBayId)
        {
            var items = DBConnector.getConnection().context.Item.Where(x => x.eBayId.Equals(eBayId));
            ViewData["Title"] = items.FirstOrDefault().Name;
            ViewData["ItemeBayId"] = eBayId;
            List<LineSeriesData> viewsData = new List<LineSeriesData>();
            items.FirstOrDefault().Pulses.ToList().ForEach(x => viewsData.Add(new LineSeriesData { X = Convert.ToDouble(x.Unix_Time)*1000, Y = x.Views }));
            ViewData["viewsData"] = viewsData;
            return View();
            //return HtmlEncoder.Default.Encode(items.FirstOrDefault().Pulses.LastOrDefault().Views.ToString());
        }
    }
}