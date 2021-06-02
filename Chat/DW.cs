using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Chat
{
    public static class DW
    {
        public static Encoding defaultEncode = Encoding.UTF8;

        //    public static List<Product> Deserialize(string xml, string xmlRoot)
        //    {
        //        XmlRootAttribute root = new XmlRootAttribute(xmlRoot);
        //        XmlSerializer serializer = null;

        //        switch (xmlRoot)
        //        {
        //            case "Products":
        //                serializer = new XmlSerializer(typeof(List<Product>), root);
        //                break;
        //        }

        //        using (StringReader sr = new StringReader(xml))
        //        {
        //            return (List<Product>)serializer.Deserialize(sr);
        //        }
        //    }
        public static object DeserializeFromFile<T>(string path)
        {
            XDocument xDoc = XDocument.Load(path);
            XmlRootAttribute root = new XmlRootAttribute(xDoc.Root.Name.ToString());

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>), root);
            XmlReader xr = xDoc.CreateReader();
            return xmlSerializer.Deserialize(xr);
        }

        public static object DeserializeFromText<T>(string xml)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(xml);

            XmlRootAttribute root = new XmlRootAttribute(xDoc.DocumentElement.Name.ToString());
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>), root);

            using (StringReader sr = new StringReader(xDoc.InnerXml.ToString())) 
            {
                return xmlSerializer.Deserialize(sr);
            }
        }
    }
}
