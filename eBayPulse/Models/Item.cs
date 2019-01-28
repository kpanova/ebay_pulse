using eBayPulse.Tools;

namespace eBayPulse.Models
{
    public partial class Item
    {
        public Item(eBayItemDataHelper eBayItemData)
        {
            this.eBayId = eBayItemData.ItemID;
            this.Name = eBayItemData.Name;
            this.UpdatePeriod_Sec = 60;
        }

        
    }
}
