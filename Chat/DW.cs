using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Chat
{
    public static class DW
    {
        public static Encoding defaultEncode = Encoding.UTF8;

        public static List<Product> Deserialize(string xml, string xmlRoot)
        {
            XmlRootAttribute root = new XmlRootAttribute(xmlRoot);
            XmlSerializer serializer = null;

            switch (xmlRoot)
            {
                case "Products":
                    serializer = new XmlSerializer(typeof(List<Product>), root);
                    break;
            }

            using (StringReader sr = new StringReader(xml))
            {
                return (List<Product>)serializer.Deserialize(sr);
            }
        }
    }
}
