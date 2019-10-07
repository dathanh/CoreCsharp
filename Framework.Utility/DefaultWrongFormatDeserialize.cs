using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace Framework.Utility
{
    public class DefaultWrongFormatDeserialize : DateTimeConverterBase
    {
        public override object ReadJson(Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(reader.Value.ToString()))
            {
                return DateTime.MinValue;
            }
            var listDateFormat = new[] { "d/M/yyyy", "d-M-yyyy", "dd/MM/yyyy", "dd-MM-yyyy", "d/M/yyyy HH:mm", "d-M-yyyy HH:mm", "dd/MM/yyyy HH:mm", "dd-MM-yyyy HH:mm" };
            //var listDateFormat = new[] { "M/d/yyyy", "M-d-yyyy", "MM/dd/yyyy", "MM-dd-yyyy", "M/d/yyyy HH:mm", "M-d-yyyy HH:mm", "MM/dd/yyyy HH:mm", "MM-dd-yyyy HH:mm" };
            var success = DateTime.TryParseExact(
                reader.Value.ToString(),
                listDateFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var date);

            if (success)
            {
                return date;
            }

            var valueStr = reader.Value.ToString();
            valueStr = DateTime.Now.Date.ToString("dd/MM/yyy") + " " + valueStr;
            success = DateTime.TryParseExact(
                valueStr,
                new[] { "dd/MM/yyyy HH:mm" },
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out date);

            if (success)
            {
                return date;
            }

            if (DateTime.TryParse(reader.Value.ToString(), out date))
            {
                return date;
            }

            return DateTime.MinValue;
        }

        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            WriteJson(writer, value, serializer);
        }
    }
}
