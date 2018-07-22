using System.Collections.Generic;
using System;
using System.IO;
using eBayPulse.Tools;

namespace eBayPulse.Models
{
    public partial class Pulse
    {
        /*
        public int Id {get; set;}
        public int? ItemId {get; set;}
        public int Unix_Time {get; set;}
        public int Views {get; set;}
        public int Watchers {get; set;}

        [InverseProperty("Pulses")]
        public virtual Item Item {get; set;}
         */
        public Pulse(eBayItemData eBayItemData, Item item)
        {
            Item = item;
            Watchers = eBayItemData.WatchCount;
            Views = (int)eBayItemData.HitCount;
            Unix_Time = eBayItemData.responseTime;
        }

    }
}
