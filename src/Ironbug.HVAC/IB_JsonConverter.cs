using Ironbug.HVAC;
using Ironbug.HVAC.BaseClass;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ironbug
{
    public class IB_JsonConverter_FieldArgSet : JsonConverter<IB_FieldArgumentSet>
    {
        public IB_JsonConverter_FieldArgSet()
        {
        }

        public override IB_FieldArgumentSet ReadJson(JsonReader reader, Type objectType, IB_FieldArgumentSet existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var array = JArray.Load(reader);
            return array.ToObject<IB_FieldArgumentSet>();

        }

        public override void WriteJson(JsonWriter writer, IB_FieldArgumentSet value, JsonSerializer serializer)
        {
            JArray t = new JArray(value.Select(_ => JToken.FromObject(_, serializer)));
            t.WriteTo(writer);
        }
    }

    public class IB_JsonConverter_Children : JsonConverter<IB_Children>
    {
        public IB_JsonConverter_Children()
        {
        }

        public override IB_Children ReadJson(JsonReader reader, Type objectType, IB_Children existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var array = JArray.Load(reader);
            var children = new IB_Children();
            foreach (var item in array)
            {
                var typeName = item["_IBType"].ToString();
                var type = Type.GetType(typeName);
                var child = item.ToObject(type, serializer) as IB_ModelObject;
                children.Add(child);
            }
            return children;

        }

        public override void WriteJson(JsonWriter writer, IB_Children value, JsonSerializer serializer)
        {
            JArray array = new JArray();
            foreach (var item in value)
            {
                var t = JToken.FromObject(item, serializer);
                t["_IBType"] = item.GetType().FullName;
                array.Add(t);
            }

            //JArray t = new JArray(value.Select(_ => JToken.FromObject(_, serializer)));
            array.WriteTo(writer);


            //JToken t = JToken.FromObject(value, serializer);
            //t["ChildType"] = value.GetType().FullName;
            //t.WriteTo(writer);
        }
    }

    public class IB_JsonConverter_HVACObjects : JsonConverter<List<IB_HVACObject>>
    {
        public IB_JsonConverter_HVACObjects()
        {
        }

        public override List<IB_HVACObject> ReadJson(JsonReader reader, Type objectType, List<IB_HVACObject> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var array = JArray.Load(reader);
            var children = new List<IB_HVACObject>();
            foreach (var item in array)
            {
                var typeName = item["_IBType"].ToString();
                var type = Type.GetType(typeName);
                var child = item.ToObject(type, serializer) as IB_HVACObject;
                children.Add(child);
            }
            return children;

        }

        public override void WriteJson(JsonWriter writer, List<IB_HVACObject> value, JsonSerializer serializer)
        {
            JArray array = new JArray();
            foreach (var item in value)
            {
                var t = JToken.FromObject(item, serializer);
                t["_IBType"] = item.GetType().FullName;
                array.Add(t);
            }

            array.WriteTo(writer);
        }
    }
}