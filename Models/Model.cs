using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace eBayPulse.Models
{
    public class Item{
        public int Id {get; set;}
        public string eBayId {get; set;}
        public string ItemName {get; set;}
        public bool IsTracked {get; set;}
    }
}