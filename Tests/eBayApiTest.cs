using eBayPulse.eBayApi;
using Xunit;
using System.IO;
using System;

namespace eBayPulse.Tests
{
    public class eBayApiTest
    {
        [Fact]
        public void GetSessionIDTest()
        {
            var devKey = "fb50a92e-3411-45b9-b3be-f9f68e73a5b3";
            var appKey = "DmitryBr-bravikov-SBX-65d8a3c47-d889524d";
            var certKey = "SBX-5d8a3c475b68-cc2c-458c-a86e-79a3";
            var ruName = "Dmitry_Bravikov-DmitryBr-bravik-zlerne";

            var keys = new Keys(devKey, appKey, certKey);

            Context context = new Context();
            context.Keys = keys;
            context.RuName = ruName;

            var getSessionId = new GetSessionID(context);

            Assert.True(getSessionId.exec(), getSessionId.ErrorMessage);
            Assert.True(getSessionId.SessionId.Length == 40);
        }

        [Fact]
        public void GetItemTest()
        {
            var itemId = "282782889148";
            Context context = new Context();
            context.Gateway = Gateway.Production;
            var tokenFilename = "Token.txt";

            Assert.True(
                File.Exists(tokenFilename),
                $"The {tokenFilename} file not found."
            );
            using (StreamReader tokenFile = new StreamReader(tokenFilename))
            {
                context.Token = tokenFile.ReadLine();
            }

            var getItem = new GetItem(context, itemId);

            Assert.True(getItem.exec(), getItem.ErrorMessage);
            Assert.True(getItem.ItemId == itemId);
            Assert.True(
                Math.Abs((getItem.Timestamp - DateTime.Now).Seconds) < 2
            );
        }
    }
}
