using System.Collections.Generic;
using System;
using System.IO;
using eBayPulse.Tools;

namespace eBayPulse.Models
{
    public partial class Item
    {
        /**
        public int Id {get; set;}
        public string eBayId {get; set;}
        public string Name {get; set;}
        public int UpdatePeriod_Sec {get; set;}

        [InverseProperty("Item")]
        public virtual ICollection<Pulse> Pulses {get; set;}

        [InverseProperty("Item")]
        public virtual ICollection<Note> Notes {get; set;}
         * */
        public Item(eBayItemData eBayItemData)
        {
            this.eBayId = eBayItemData.ItemID;
            this.Name = eBayItemData.Name;
            this.UpdatePeriod_Sec = 60;
        }

        
    }
}
