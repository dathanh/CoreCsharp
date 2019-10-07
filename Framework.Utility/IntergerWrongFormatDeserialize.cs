using Newtonsoft.Json;
using System;

namespace Framework.Utility
{
    public class IntergerWrongFormatDeserialize : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            if (objectType == typeof(int) || objectType == typeof(int) || objectType == typeof(decimal) ||
                objectType == typeof(double))
            {
                if (reader.Value == null || reader.Value.ToString() == "null" || reader.Value.ToString() == "undefined")
                {
                    return 0;
                }
            }
            else if (objectType == typeof(int?) || objectType == typeof(int?) || objectType == typeof(decimal?) ||
                     objectType == typeof(double?))
            {
                if (reader.Value == null || reader.Value.ToString() == "null" || reader.Value.ToString() == "undefined")
                {
                    return null;
                }
            }
            if (objectType == typeof(int) || objectType == typeof(int?))
            {
                int.TryParse(reader.Value.ToString(), out int result);
                return result;
            }
            if (objectType == typeof(int) || objectType == typeof(int?))
            {
                int.TryParse(reader.Value.ToString(), out int result);
                return result;
            }
            if (objectType == typeof(decimal) || objectType == typeof(decimal?))
            {
                decimal.TryParse(reader.Value.ToString(), out decimal result);
                return result;
            }
            if (objectType == typeof(double) || objectType == typeof(double?))
            {
                double.TryParse(reader.Value.ToString(), out double result);
                return result;
            }
            return null;
        }

        public override void WriteJson(Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(int) == objectType || typeof(int?) == objectType
                   || typeof(int) == objectType || typeof(int?) == objectType
                   || typeof(decimal) == objectType || typeof(decimal?) == objectType
                   || typeof(double) == objectType || typeof(double?) == objectType;
        }
    }
}
