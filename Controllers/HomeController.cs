using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace eBayPulse.Controllers
{
    public class HomeController : Controller
    {
        // 
        // GET: /HelloWorld/
        public IActionResult Index()
        {
            return View();
        }
        

        [HttpPost]
        public string Index(string msg)
        {
            HttpResponseReceiver hit = new HttpResponseReceiver(){id = msg};
            string response = hit.Response;
            Item item = new Item(msg, response);   
            return (item.HitCount < 0 ? HttpResponseReceiver.ExceptionsList[item.HitCount] : item.HitCount.ToString());
        }
    }
}