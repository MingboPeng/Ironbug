using OpenStudio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ironbug.HVAC.BaseClass
{
    public abstract class IB_DataFieldSet<T, K> : ICollection<IB_IDDDataField>
        where T : IB_DataFieldSet<T, K>
        //where K : ModelObject
    {
        /// <summary>
        /// Static instance. Needs to use lambda expression
        /// to construct an instance (since constructor is private).
        /// https://www.codeproject.com/Articles/572263/A-Reusable-Base-Class-for-the-Singleton-Pattern-in
        /// https://stackoverflow.com/questions/16745629/how-to-abstract-a-singleton-class
        /// </summary>
        private static readonly Lazy<T> instance = new Lazy<T>(() => Activator.CreateInstance(typeof(T), true) as T);


        /// <summary>
        /// Value contains a sin
        /// </summary>
        public static T Value { get { return instance.Value; } }

        private ICollection<IB_IDDDataField> _items = new List<IB_IDDDataField>();


        //IDD object for later unit converting
        private IddObject RefIddObject { get; }

        //parent type for getting all "set" methods 
        protected Type ParentType { get; } = typeof(K);



        public readonly IB_MasterDataField TheMasterDataField;

        protected IB_DataFieldSet()
        {

            this.RefIddObject = GetIddObject(this.ParentType);

            this._items = GetIddDataFields(this.RefIddObject).ToList();
            MapOSSettings(this.ParentType, this);

            this.TheMasterDataField = GetTheMasterDataField();

        }

        private static IddObject GetIddObject(Type parentType)
        {
            var iddType = parentType.GetMethod("iddObjectType", BindingFlags.Public | BindingFlags.Static).Invoke(null, null) as IddObjectType;
            return new IdfObject(iddType).iddObject();
        }

        private static IEnumerable<IB_IDDDataField> GetIddDataFields(IddObject iddObject)
        {
            var fromList = iddObject.nonextensibleFields();
            //var toList = new List<IB_IDDDataField>();

            return
                fromList
                .Where(_ =>
                {
                    var dataType = _.properties().type.valueDescription();
                    var result = dataType != "object-list";
                    result &= dataType != "node";
                    result &= dataType != "handle";
                    return result;
                })
                .Select(_ => new IB_IDDDataField(_));


        }

        public IEnumerable<IB_DataField> GetCustomizedDataFields()
        {
            return this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                            .Select(_ => (IB_DataField)_.GetValue(this, null));
        }

        public IB_DataField GetDataFieldByName(string name)
        {
            var field = this.GetType().GetField(name);
            return (IB_DataField)field.GetValue(this);
        }

        private static void MapOSSettings(Type OSType, IB_DataFieldSet<T, K> dataFieldSet)
        {
            //var masterSettings =
            OSType
            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(_ =>
                {
                    if (!_.Name.StartsWith("set")) return false;
                    if (_.GetParameters().Count() != 1) return false;

                    var paramType = _.GetParameters().First().ParameterType;
                    var isValidType = paramType == typeof(string) || paramType == typeof(double) || paramType == typeof(bool);
                    if (!isValidType) return false;

                    return true;

                }
                ).ToList().ForEach(_ =>
                {
                    var name = _.Name.Substring(3);
                    var type = _.GetParameters().First().ParameterType;

                    if (dataFieldSet.Contains(name))
                    {
                        dataFieldSet.First(item => item.FullName == name).UpdateDataType(type);
                    };
                });


            //return masterSettings;
        }

        private IB_MasterDataField GetTheMasterDataField()
        {

            var masterSettings = this.OrderBy(_ => _.FullName);
            var description = "This gives you an option that if you are looking for a setting that is not listed above," +
                "please feel free to pick any setting from following items, " +
                "but please double check the EnergyPlus Input References to ensure you know what you are doing.\r\n\r\n";
            description += "TDDO: show an example to explain how to use this!\r\n\r\n";
            description += string.Join("\r\n", masterSettings.Select(_ => _.FullName));

            //TODO: there must be a better way to do this.
            var masterDataFieldMap = new Dictionary<string, IB_IDDDataField>();
            foreach (var item in masterSettings)
            {
                masterDataFieldMap.Add(item.FullName.ToUpper(), item);
            }

            var df = new IB_MasterDataField(description, masterDataFieldMap);
            return df;

        }




        public int Count => _items.Count;

        public bool IsReadOnly => _items.IsReadOnly;


        public void Add(IB_IDDDataField item)
        {
            _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(IB_IDDDataField item)
        {
            //TDDO: how to compare ???
            return _items.Contains(item);
        }

        public bool Contains(string fullName)
        {
            //TDDO: how to compare ???
            return _items.Any(_ => _.FullName == fullName);
        }

        public void CopyTo(IB_IDDDataField[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public bool Remove(IB_IDDDataField item)
        {
            //TDDO: how to compare ???
            return _items.Remove(item);
        }

        public IEnumerator<IB_IDDDataField> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }

    public sealed class IB_DataFieldSet
        : IB_DataFieldSet<IB_DataFieldSet, ModelObject>
    {
        private IB_DataFieldSet() { }
    }
}
