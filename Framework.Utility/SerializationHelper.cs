using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Framework.Utility
{
    public class SerializationHelper
    {
        public static string SerializeToXml<T>(T obj)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            var stringBuilder = new StringBuilder();
            using (TextWriter writer = new StringWriter(stringBuilder))
            {
                xmlSerializer.Serialize(writer, obj);
            }

            return stringBuilder.ToString();
        }

        public static T Deserialize<T>(string xmlInput)
        {
            if (string.IsNullOrWhiteSpace(xmlInput))
            {
                return default(T);
            }

            var ser = new XmlSerializer(typeof(T));

            using (var sr = new StringReader(xmlInput))
            {
                return (T)ser.Deserialize(sr);
            }
        }
    }
}