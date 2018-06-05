using OpenStudio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ironbug.HVAC.BaseClass
{
    
    public abstract class IB_FieldSet: ICollection<IB_Field>
    {

        private ICollection<IB_Field> _items { get; set; } = new List<IB_Field>();


        //IDD object for later unit converting, etc
        //private IddObject RefIddObject { get; }

        //parent type for getting all "set" methods 
        internal abstract Type RefOpsType { get; }
        
        public IB_MasterField TheMasterDataField { get; }


        protected IB_FieldSet()
        {
            //Assign reference IddObject from OpenStudio
            //this.RefIddObject = IB_OpsTypeOperator.GetIddObject(this.RefOpsType);
            var iddFields = IB_OpsTypeOperator.GetIddObject(this.RefOpsType).GetIddFields();

            this._items = IB_OpsTypeOperator.GetOSSetters(this.RefOpsType)
                .Select(_ => new IB_Field(_)) // convert to IB_Field
                .UpdateFromIddFields(iddFields)
                .UpdateFromSelfPreperties(this)
                .ToList();
            
            this.TheMasterDataField = GetTheMasterDataField(this);
            this._items.Add(TheMasterDataField);
            
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


        public static IEnumerable<IB_Field> UpdateFromIddFields(this IEnumerable<IB_Field> iB_fields, IEnumerable<IddField> iddFields)
        {

            var dfSet = iB_fields.ToList();

            dfSet.Add(new IB_Field("Name", "Name"));
            //dfSet.Add(new IB_Field("Comment", "Comment"));

            foreach (var item in dfSet)
            {
                var found = iddFields.FirstOrDefault(_ => CleanFULLNAME(_.name()) == item.FULLNAME);

                if (found is null) continue;
                item.UpdateFromIddField(found);
            }

            return dfSet;

        }

        

        /// <summary>
        /// Map properties of the ProDataField or BasicDataField that defined in derived class, to DataFieldSet's IB_DataField collection.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IB_Field> UpdateFromSelfPreperties(this IEnumerable<IB_Field> ib_dataFields, IB_FieldSet derivedDataFieldSet)
        {
            
            derivedDataFieldSet.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Select(_ => (IB_Field)_.GetValue(derivedDataFieldSet, null))
                .ToList().ForEach(_ =>
                {
                    //all customized DataFields will be tested before release, 
                    //so there is no need to test it here for inclusion.
                    var dataField = ib_dataFields.GetByName(_.FULLNAME);
                    
                    dataField.NickName = _.NickName;
                    dataField.DetailedDescription = _.DetailedDescription;

                });

            return ib_dataFields;

        }

    }
}
