using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Ironbug.Core;

namespace Ironbug.HVAC.BaseClass
{

    [DataContract]
    public class IB_PropArgumentSet : Dictionary<string, object>, System.IEquatable<IB_PropArgumentSet>
    {
        public void Set<T>(T value, [CallerMemberName] string caller = null)
        {
            if (string.IsNullOrEmpty(caller))
                throw new ArgumentException("Missing caller name! Use SetByKey instead!");
            SetByKey(caller, value);
        }

        public void SetByKey<T>(string propertyName, T value)
        {
            this.TryAdd(propertyName, value);
        }


        public object Get(string propertyName)
        {
            //if (!this.Any() || !this.TryGetValue(propertyName, out var prop))
            //    throw new ArgumentException($"Failed to find the property {propertyName}");
            this.TryGetValue(propertyName, out var prop);
            return prop;
        }
        public T Get<T>([CallerMemberName] string caller = null)
        {
            if (string.IsNullOrEmpty(caller))
                throw new ArgumentException("User GetByKey instead!");
            return GetByKey<T>(caller);
        }

        public T Get<T>(T defaultValue, [CallerMemberName] string caller = null)
        {
            if (string.IsNullOrEmpty(caller))
                throw new ArgumentException("User GetByKey instead!");
            return GetByKey(caller, defaultValue);
        }

        public T GetByKey<T>(string propertyName, T defaultValue)
        {
            var props = this;

            if (!props.ContainsKey(propertyName))
            {                // add default value first
                this.SetByKey(propertyName, defaultValue);
                return defaultValue;
            }
            else
                return GetByKey<T>(propertyName);
        }
        public T GetByKey<T>(string propertyName)
        {
            var prop = Get(propertyName);
            if (prop == null)
                return default(T);

            // convert to T
            if (prop is T pt)
                return pt;

            var realValue = prop.To<T>();

            // override the current property with the correct type
            this.SetByKey(propertyName, realValue);
            return realValue;

        }

        public List<T> GetList<T>([CallerMemberName] string caller = null)
        {
            if (string.IsNullOrEmpty(caller))
                throw new ArgumentException("Missing caller name! Use GetListByKey instead!");
            return GetListByKey<T>(caller, null);
        }

        public List<T> GetList<T>(List<T> defaultList, [CallerMemberName] string caller = null)
        {
            if (string.IsNullOrEmpty(caller))
                throw new ArgumentException("Missing caller name! Use GetListByKey instead!");
            return GetListByKey<T>(caller, defaultList);
        }
        public List<T> GetList<T>(bool initDefault, [CallerMemberName] string caller = null)
        {
            if (string.IsNullOrEmpty(caller))
                throw new ArgumentException("Missing caller name! Use GetListByKey instead!");
            return GetListByKeyInit<T>(caller, initDefault ? () => new List<T>() : null);
        }

        public List<T> GetListByKey<T>(string propertyName, List<T> defaultList)
        {
            return GetListByKeyInit(propertyName, ()=> defaultList);
        }

        public List<T> GetListByKeyInit<T>(string propertyName, Func<List<T>> initDefault)
        {
            var props = this;

            if (!props.TryGetValue(propertyName, out var prop))
            {   // add default value first
                var def = initDefault?.Invoke();
                this.SetByKey(propertyName, def);
                return def;
            }
            else if (prop is IEnumerable ls)
            {
                if (prop is List<T> lst)
                    return lst;
                else
                {
                    try
                    {
                        var casted = ls.Cast<T>().ToList();
                        this.SetByKey(propertyName, casted);
                        return casted;
                    }
                    catch (Exception)
                    {
                        var casted = ls.Cast<object>().Select(_ => _.To<T>()).ToList();
                        this.SetByKey(propertyName, casted);
                        return casted;
                    }

                }
            }
            else
            {
                throw new ArgumentException($"{propertyName} is not a list type property");
            }
        }

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            try
            {
                var checkedValues = this
                    .Where(_ => _.Value is Newtonsoft.Json.Linq.JToken)
                    .Select(_ => new { _.Key, Value = DeserializationHelper.Deserialize(_.Value as Newtonsoft.Json.Linq.JToken)  })
                    .ToList();
              
                // update
                foreach (var item in checkedValues)
                {
                    this.SetByKey(item.Key, item.Value);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Failed to deserialize", ex);
            }
            


        }


        public IB_PropArgumentSet Duplicate()
        {
            var dup = new IB_PropArgumentSet();
            foreach (var item in this)
            {
                dup.Add(item.Key, Duplicate(item.Value));
            }
            return dup;
        }

        static object Duplicate(object obj)
        {
            if (obj is IEnumerable enu)
            {
                return enu.Cast<object>().Select(_ => Duplicate(_));
            }
            else if (obj is IB_ModelObject mo)
                return mo.Duplicate();
            else
            {
                return obj;
            }
        }

        public override bool Equals(object obj) => this.Equals(obj as IB_PropArgumentSet);
        public bool Equals(IB_PropArgumentSet other)
        {
            if (other == null)
                return false;

            if (this.Count != other.Count)
                return false;

            var same = true;
            foreach (var item in this)
            {
                same &= other.TryGetValue(item.Key, out var o);
                if (!same) return false;
                same = AreSame(item.Value, o);
                if (!same) return false;
            }
            return same;
        }

        static bool AreSame(object o1, object o2)
        {
            var same = true;
            if (o1 is IEnumerable enu)
            {
                var o1m = enu.Cast<object>();
                var o2m = (o2 as IEnumerable)?.Cast<object>();
                if (!o1m.SequenceEqual(o2m))
                {
                    var zip = o1m.Zip(o2m, (l, r) => new { l, r });
                    same &= zip.All(_ => AreSame(_.l, _.r));
                }
            
            }
            else
            {
                same = o1.Equals(o2);
                if (same) return true;

                // try to match the type
                o2 = Convert.ChangeType(o2, o1.GetType());
                same = o1.Equals(o2);
            }
            

            return same;
        }

    }

}
