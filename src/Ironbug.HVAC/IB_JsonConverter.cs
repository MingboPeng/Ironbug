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
                    value = JsonConvert.DeserializeObject(valueToken.ToString(), type, IB_JsonConverter_FieldArgumentSet.Instance);
                }
            }

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


    /// <summary>
    /// ModelObject will have a new tracking ID added upon its initialization. Have to use this converter to override the all field arguments
    /// </summary>
    public class IB_JsonConverter_FieldArgumentSet : JsonConverter<IB_FieldArgumentSet>
    {
        private static IB_JsonConverter_FieldArgumentSet _instance;
        public static IB_JsonConverter_FieldArgumentSet Instance
        {
            get { return _instance ?? (_instance = new IB_JsonConverter_FieldArgumentSet()); }
        }

        public IB_JsonConverter_FieldArgumentSet()
        {
        }

        public override IB_FieldArgumentSet ReadJson(JsonReader reader, Type objectType, IB_FieldArgumentSet existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var array = JArray.Load(reader);
            var obj = array.ToObject<IB_FieldArgumentSet>(Serializer);
            
            return obj;

        }

        public override void WriteJson(JsonWriter writer, IB_FieldArgumentSet value, JsonSerializer serializer)
        {
            JArray t = new JArray(value.Select(_ => JToken.FromObject(_, serializer)));
            t.WriteTo(writer);
        }

        private static JsonSerializer _serializer;
        public static JsonSerializer Serializer
        {
            get
            {
                if (_serializer == null)
                {
                    _serializer = JsonSerializer.Create(IB_JsonSetting.FieldArgumentSet_ConvertSetting);
                }
                return _serializer;

            }
        }

    }

}
