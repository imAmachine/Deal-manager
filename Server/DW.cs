using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Server
{
    public static class DW
    {
        public static List<Product> Deserialize(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                XmlRootAttribute xmlRoot = new XmlRootAttribute("Products");
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Product>), xmlRoot);
                return (List<Product>)xmlSerializer.Deserialize(fs);
            }
        }
    }
}
