using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using eBayPulse.Tools;


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
            eBayItemIdCleaner eBayItemIdCleaner = new eBayItemIdCleaner(msg);
            if(eBayItemIdCleaner.IsValid)
            {
                HttpResponseReceiver hit = new HttpResponseReceiver(){id = eBayItemIdCleaner.Value};
                string response = hit.Response;
                Item item = new Item(msg, response);
                return (item.HitCount < 0 ? HttpResponseReceiver.ExceptionsList[item.HitCount] : item.HitCount.ToString());
            }
            else
            {
                return string.Empty;
            }
        }

    }
}