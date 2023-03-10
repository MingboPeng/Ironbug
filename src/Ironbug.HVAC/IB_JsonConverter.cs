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
        private static IB_JsonConverter_FieldArgument _instance;
        public static IB_JsonConverter_FieldArgument Instance
        {
            get { return _instance ?? (_instance = new IB_JsonConverter_FieldArgument()); }
        }
        
        public IB_JsonConverter_FieldArgument()
        {
        }

        public override IB_FieldArgument ReadJson(JsonReader reader, Type objectType, IB_FieldArgument existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
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
                    //value = JsonConvert.DeserializeObject(valueToken.ToString(), type, Serializer);
                    value = valueToken.ToObject(type, serializer);
                }
            }

            return new IB_FieldArgument(field, value);
        }

        public override void WriteJson(JsonWriter writer, IB_FieldArgument value, JsonSerializer serializer)
        {

            JToken t = JToken.FromObject(value);
            if (value.Value is IB_ModelObject obj)
            {
                t["Value"] = JObject.FromObject(obj, serializer);
            }

            t.WriteTo(writer);

        }

    }


    public static class DeserializationHelper
    {
        public static object Deserialize(JToken result)
        {
            var tp = result.Type;
            if (tp == JTokenType.Array)
            {
                var list = result.ToArray().Select(_ => Deserialize(_)).ToList();
                return list;
            }
            else if (tp == JTokenType.Object)
            {
                var prop = result.OfType<JProperty>().FirstOrDefault(_ => _.Name == "$type");
                if (prop != null)
                {
                    var obj = JsonConvert.DeserializeObject(result.ToString(), IB_JsonSetting.ConvertSetting);
                    if (obj is JToken jObj)
                        obj = Deserialize(jObj);
                    return obj;
                }
                return result;
            }
            else if (result is JValue jv)
            {
                return jv.Value;
            }
            else
            {
                var value = result.ToString();
                return value;
            }
        }

    }

}
