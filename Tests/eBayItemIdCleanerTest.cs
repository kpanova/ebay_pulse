using System;
using Xunit;
using eBayPulse.Tools;

namespace eBayPulse.Tests
{
    public class eBayItemIdCleanerTest
    {
        [Fact]
        public void Lenght()
        {
            Assert.True(eBayItemIdCleaner.Length == 12);
        }

        [Fact]
        public void BadId()
        {
            String [] dirtyStrings = {
                null,
                String.Empty,
                "",
                "12345678901a",
                "123456789012a",
                "a123456789012",
                "itm/123456789012a",
            };

            foreach (var dirtyString in dirtyStrings) {
                var id = new eBayItemIdCleaner(dirtyString);
                Assert.False(id.IsValid, dirtyString);
                Assert.True(id.Value == String.Empty, dirtyString);
            }
        }

        [Fact]
        public void RightId()
        {
            const String rigthId = "123456789012";

            String [] dirtyStrings = {
                "123456789012",
                "https://www.ebay.com/itm/123456789012",
                "itm/123456789012",
                "https://www.ebay.com/itm/123456789012?_trksid=p2050601.m570.l5999&_trkparms=gh1g%3DI231988476336.N36.S1.R1.TR1",
                "itm/123456789012?_trksid=p2050601.m570.l5999",
                "https://www.ebay.com/itm/VELO-FAT-BIKE-26-x-4-00-ALUMINIUM-SHIMANO-21V-DOUBLE-DISQUE/123456789012?hash=item1c89f4727b:m:mnqC4Qyb9o4pxerFIjYZwrA",
                "https://www.ebay.co.uk/itm/CLASSIC-Heritage-Ladies-26-Wheel-7-Speed-Traditional-Bike-Bicycle-White/123456789012?hash=item48a20806b0:m:mLOeYl6dDTt_7zC2OL15_cQ",
                "https://www.ebay.com/p/Bicycle-Bike-Wall-Mount-Hanger-Hook-Storage-Steel-Holder-Garage-Rack-Stand-Black/1973734742?iid=123456789012&_trkparms=aid%3D555018%26algo%3DPL.SIM%26ao%3D2%26asc%3D50543%26meid%3D2c3fb513835342548b618f8d5983938e%26pid%3D100005%26rk%3D3%26rkt%3D12%26sd%3D332588994136%26itm%3D142765676766&_trksid=p2047675.c100005.m1851",
            };

            foreach (var dirtyString in dirtyStrings) {
                var id = new eBayItemIdCleaner(dirtyString);
                String errorMessage = String.Format("string: \"{0}\" id: \"{1}\"", dirtyString, id.Value);
                Assert.True(id.IsValid, "is not valid " + errorMessage);
                Assert.True(id.Value == rigthId, errorMessage);
            }
        }
    }
}
