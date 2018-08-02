using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace eBayPulse.Tools
{
    public class XmlReader
    {
        public XmlReader(string xml)
        {
            try
            {
                xdoc = XDocument.Load(new StringReader(xml));
            }
            catch (XmlException)
            {
                xdoc = null;
            }
        }

        public string GetValue(string tag)
        {
            if (xdoc == null)
            {
                return null;
            }

            return GetNode(xdoc.Elements().ToList(), tag);
        }

        private string GetNode(List<XElement> xElements, string tag)
        {
            foreach (var xElement in xElements)
            {
                if (xElement.Name.LocalName == tag)
                {
                    return xElement.Value;
                }
                else if (xElement.HasElements)
                {
                    var res = GetNode(xElement.Elements().ToList(), tag);
                    if (res != null)
                    {
                        return res;
                    }
                }
            }
            return null;
        }

        private XDocument xdoc;
    }
}
