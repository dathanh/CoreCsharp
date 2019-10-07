using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Framework.Utility
{
    public class ValidEnumConverter : StringEnumConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (!Enum.IsDefined(objectType, reader.Value))
            {
                throw new ArgumentException("Invalid enum value");
            }

            return base.ReadJson(reader, objectType, existingValue, serializer);
        }
    }
}
