using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace StationControllerUi
{
    public class Config
    {
        

        public string ScPath { get; set; }
        public int DebugPort { get; set; }
        public string SyntaxDescriptionFile { get; set; }

        public static Config Load(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Config));
            Config result = null;
            using(StreamReader reader = new StreamReader(new FileStream(fileName, FileMode.Open)))
            {
                result = (Config)serializer.Deserialize(reader);
            }

            return result;
        }

        public void Save(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            using(StreamWriter writer = new StreamWriter(new FileStream(fileName, FileMode.Create)))
            {
                serializer.Serialize(writer, this);
                writer.Flush();
            }
        }
    }
}
