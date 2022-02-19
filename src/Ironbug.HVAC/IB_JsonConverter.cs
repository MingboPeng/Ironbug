using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug
{
    public class IB_JsonConverter_FieldArgument : JsonConverter<IB_FieldArgument>
    {
        public IB_JsonConverter_FieldArgument()
        {
        }

        public override IB_FieldArgument ReadJson(JsonReader reader, Type objectType, IB_FieldArgument existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            IB_ModelObject.Deserializating = true;
            JToken jToken = JToken.ReadFrom(reader);
            // Field
            var fieldTypeJson = jToken[nameof(existingValue.Field)].ToString();
            var field = JsonConvert.DeserializeObject<IB_Field>(fieldTypeJson);
            // Value
            var valueToken = jToken[nameof(existingValue.Value)];
            object value = valueToken.ToString();
            if (valueToken.Type == JTokenType.Object)
            {
                var prop = valueToken.OfType<JProperty>().FirstOrDefault(_ => _.Name == "$type");
                if (prop != null)
                {
                    var typeName = prop.Value.ToString();
                    var type = Type.GetType(typeName);
                    value = JsonConvert.DeserializeObject(valueToken.ToString(), type);
                }
            }

            IB_ModelObject.Deserializating = false;
            return  new IB_FieldArgument(field, value);
        }

        public override void WriteJson(JsonWriter writer, IB_FieldArgument value, JsonSerializer serializer)
        {
            JToken t = JToken.FromObject(value);
            if (value.Value is IB_ModelObject obj)
            {
                JToken tv = t["Value"];
                tv["$type"] = obj.GetType().FullName;
            }
            t.WriteTo(writer);
        }
    }

  
}