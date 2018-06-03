using OpenStudio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ironbug.HVAC.BaseClass
{
    /// <summary>
    /// Handle the IB_DataFieldSet's children's singleton 
    /// </summary>
    /// <typeparam name="T">T is derived class</typeparam>
    /// <typeparam name="K">K is ParentType from OpenStudio</typeparam>
    public abstract class IB_FieldSet<T, K> : IB_FieldSet
        where T : IB_FieldSet<T, K>
        where K : ModelObject
    {
        /// <summary>
        /// Static instance. Needs to use lambda expression
        /// to construct an instance (since constructor is private).
        /// https://www.codeproject.com/Articles/572263/A-Reusable-Base-Class-for-the-Singleton-Pattern-in
        /// https://stackoverflow.com/questions/16745629/how-to-abstract-a-singleton-class
        /// </summary>
        private static readonly Lazy<T> instance = new Lazy<T>(() => Activator.CreateInstance(typeof(T), true) as T);


        /// <summary>
        /// Value contains a single instance.
        /// </summary>
        public static T Value { get { return instance.Value; } }

        internal override Type ParentType => typeof(K);

        protected IB_FieldSet():base()
        {

        }
    }

    public abstract class IB_FieldSet: ICollection<IB_Field>
    {

        private ICollection<IB_Field> _items = new List<IB_Field>();


        //IDD object for later unit converting, etc
        private IddObject RefIddObject { get; }

        //parent type for getting all "set" methods 
        internal abstract Type ParentType { get; }

        
        public IB_MasterField TheMasterDataField { get; }


        protected IB_FieldSet()
        {
            //Assign reference IddObject from OpenStudio
            this.RefIddObject = GetIddObject(this.ParentType);

            //Get IddDataFields and map OpenStudio field properties.
            //var fields = GetIddDataFields(this.RefIddObject).MapOpsSettings(this.ParentType);
            //_items = fields.MapOpsSettings(this.ParentType)
            //  MapCustomizedDataFieldsSettings()
            //    .ToList();

            this._items =
                GetIddDataFields(this.RefIddObject)
                .MapOpsSettings(this.ParentType)
                .MapCustomizedDataFieldsSettings(this)
                .ToList();

            
            this.TheMasterDataField = GetTheMasterDataField(this);
            this._items.Add(TheMasterDataField);

        }

        private static IddObject GetIddObject(Type parentType)
        {
            var iddType = parentType.GetMethod("iddObjectType", BindingFlags.Public | BindingFlags.Static).Invoke(null, null) as IddObjectType;
            return new IdfObject(iddType).iddObject();
        }

        private static IEnumerable<IB_IddField> GetIddDataFields(IddObject iddObject)
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
                .Select(_ => new IB_IddField(_));//Construct IddDataField as default first. 
            
        }
        /// <summary>
        /// Call this method to get all fields that inside of this fieldset.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IB_Field> GetCustomizedDataFields()
        {
            return this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                            .Select(_ => (IB_Field)_.GetValue(this, null));
        }

        //public IB_DataField GetDataFieldByName(string name)
        //{
        //    var field = this.GetType().GetField(name);
        //    return (IB_DataField)field.GetValue(this);
        //}



        private static IB_MasterField GetTheMasterDataField(IB_FieldSet dataFieldSet)
        {

            var masterSettings = dataFieldSet.OrderBy(_ => _.FullName);
            var description = "This gives you an option that if you are looking for a setting that is not listed above," +
                "please feel free to pick any setting from following items, " +
                "but please double check the EnergyPlus Input References to ensure you know what you are doing.\r\n\r\n";
            description += "TDDO: show an example to explain how to use this!\r\n\r\n";
            description += string.Join("\r\n", masterSettings.Select(_ => _.FullName));

            ////TODO: there must be a better way to do this.
            //var masterDataFieldMap = new Dictionary<string, IB_DataField>();
            //foreach (var item in masterSettings)
            //{
            //    masterDataFieldMap.Add(item.FullName.ToUpper(), item);
            //}

            var df = new IB_MasterField(description);
            return df;

        }


       

        public int Count => _items.Count;

        public bool IsReadOnly => _items.IsReadOnly;


        public void Add(IB_Field item)
        {
            _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(IB_Field item)
        {
            return _items.Any(_ => _.FULLNAME == item.FULLNAME);
        }

        
        public bool Contains(string fullName)
        {
            return _items.Any(_ => _.FULLNAME == fullName.CleanFULLNAME());
        }

        

        public void CopyTo(IB_Field[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public bool Remove(IB_Field item)
        {
            //TDDO: how to compare ???
            return _items.Remove(item);
        }

        public IEnumerator<IB_Field> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }

    public static class FieldSetExtension
    {
        /// <summary>
        /// Note: this would return null if cannot find the dataField by name.
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns>IB_DataField or null</returns>
        public static IB_Field GetByName(this IEnumerable<IB_Field>dataFields, string fullName)
        {
            return dataFields.FirstOrDefault(item => item.FULLNAME == fullName.CleanFULLNAME());
        }

        /// <summary>
        /// Note: this would return null if cannot find the dataField by name.
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns>IB_DataField or null</returns>
        public static IB_Field GetByName(this IB_FieldSet dataFields, string fullName)
        {
            return dataFields.FirstOrDefault(item => item.FULLNAME == fullName.CleanFULLNAME());
        }

        public static string CleanFULLNAME(this string fullName)
        {
            return new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9]")
                .Replace(fullName, string.Empty)
                .ToUpper();
        }


        public static IEnumerable<IB_IddField> MapOpsSettings(this IEnumerable<IB_IddField> dataFieldSet, Type OSType)
        {
            

            var opsSettingMethods = OSType
            .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(_ =>
                {
                    //get all setting methods
                    if (!_.Name.StartsWith("set")) return false;
                    if (_.GetParameters().Count() != 1) return false;

                    var paramType = _.GetParameters().First().ParameterType;
                    var isValidType =
                    paramType == typeof(string) ||
                    paramType == typeof(double) ||
                    paramType == typeof(bool) ||
                    paramType == typeof(int);

                    if (!isValidType) return false;

                    return true;

                }
                );

            var dfSet = dataFieldSet.ToList();
            foreach (var _ in opsSettingMethods)
            {
                //assign OpenStudio acceptable parameter type to matched data field.
                var name = _.Name.Substring(3);
                var type = _.GetParameters().First().ParameterType;
                
                var found = dfSet.FirstOrDefault(item => item.FULLNAME == name.ToUpper());
               
                if (found is null) continue;
                found.UpdateFromOpenStudioMethod(name, type);
                //TODO: add setter method delegate as well.
            }
            

            return dfSet;

        }

        /// <summary>
        /// Map properties of the ProDataField or BasicDataField that defined in derived class, to DataFieldSet's IB_DataField collection.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IB_Field> MapCustomizedDataFieldsSettings(this IEnumerable<IB_IddField> dataFieldSet, IB_FieldSet derivedDataFieldSet)
        {
            var dfSet = dataFieldSet;
            derivedDataFieldSet.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Select(_ => (IB_Field)_.GetValue(derivedDataFieldSet, null))
                .ToList().ForEach(_ =>
                {
                    //all customized DataFields will be tested before release, so there is no need to test it here for inclusion.
                    var dataField = dfSet.GetByName(_.FULLNAME);


                    dataField.ShortName = _.ShortName;
                    dataField.DetailedDescription = _.DetailedDescription;

                });

            return dfSet;

        }

    }
}
