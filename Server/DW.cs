using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Server
{
    public static class DW
    {
        public static object Deserialize<T>(string path)
        {
            XDocument xDoc = XDocument.Load(path);
            string rootElement = xDoc.Root.Name.ToString();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<T>), rootElement);
            XmlReader xr = xDoc.CreateReader();
            return xmlSerializer.Deserialize(xr);
        }

        public static string GetXml(string file)
        {
            using (StreamReader sr = new StreamReader($@"{AppDomain.CurrentDomain.BaseDirectory}/xmls/{file}"))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
