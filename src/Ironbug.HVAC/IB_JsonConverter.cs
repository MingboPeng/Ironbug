using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug
{
    public class IB_JsonConverter : JsonConverter<IB_FieldArgumentSet>
    {
        public IB_JsonConverter()
        {
        }

        public override IB_FieldArgumentSet ReadJson(JsonReader reader, Type objectType, IB_FieldArgumentSet existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var array = JArray.Load(reader);
            return array.ToObject<IB_FieldArgumentSet>();

        }

        public override void WriteJson(JsonWriter writer, IB_FieldArgumentSet value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value, serializer);
            t.WriteTo(writer);
        }
    }
}