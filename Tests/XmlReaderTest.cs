using eBayPulse.Tools;
using Xunit;

namespace eBayPulse.Tests
{
    public class XmlReaderTest
    {
        [Fact]
        public void TestRoot()
        {
            var xmlReader = new XmlReader("<root>123</root>");
            var value = xmlReader.GetValue("root");
            Assert.True(value == "123", string.Format("value: '{0}'", value));
        }
        [Fact]
        public void TestNestedTag()
        {
            var xmlReader = new XmlReader("<root><item>123</item></root>");
            var value = xmlReader.GetValue("item");
            Assert.True(value == "123", string.Format("value: '{0}'", value));
        }
        [Fact]
        public void TestEmptyDocument()
        {
            var xmlReader = new XmlReader("");
            var value = xmlReader.GetValue("root");
            Assert.True(value == null, string.Format("value: '{0}'", value));
        }
        [Fact]
        public void TestEmptyTag()
        {
            var xmlReader = new XmlReader("<root><item>123</item></root>");
            var value = xmlReader.GetValue("");
            Assert.True(value == null, string.Format("value: '{0}'", value));
        }
    }
}
