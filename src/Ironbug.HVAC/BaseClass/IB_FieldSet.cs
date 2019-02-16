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
        public string OwnerEpNote = string.Empty;

        private ICollection<IB_Field> _items { get; set; } = new List<IB_Field>();


        //IDD object for later unit converting, etc
        //private IddObject RefIddObject { get; }

        //parent type for getting all "set" methods 
        internal abstract Type RefOpsType { get; }

        internal abstract Type RefEpType { get; }

        protected IB_FieldSet()
        {
            //First, get all self properties such as IB_TopField, IB_BasicField, IB_ProField, etc
            //and put it to _items
            this._items = this.GetSelfPreperties().ToList();

            var osSetters = IB_OpsTypeOperator.GetOSSetters(this.RefOpsType).Select(_ => new IB_Field(_)); // convert to IB_Field
            this._items = _items.MergeFromOSMethods(osSetters).ToList();

            //Assign reference IddObject from OpenStudio
            //this.RefIddObject = IB_OpsTypeOperator.GetIddObject(this.RefOpsType);
            var iddObj = IB_OpsTypeOperator.GetIddObject(this.RefOpsType);

            var iddFields = iddObj.GetIddFields();
            this._items.UpdateFromIddFields(iddFields);

            AddDescriptionFromEPDoc(this.RefEpType);
        }

        private void AddDescriptionFromEPDoc(Type RefObjType)
        {
            var tp = RefObjType;

            var name = string.Format("Ironbug.EPDoc.{0}", tp.Name);
            Type type = typeof(EPDoc.ExampleClass).Assembly.GetType(name, false, true);
            
            if (type!=null)
            {
                //add notes to field's description
                var note = type.GetProperty("Note").GetValue(null, null) as string;
                if (!string.IsNullOrEmpty(note))
                {
                    note = note.Length > 3000?note.Substring(0,3000)+"....(Due to the length of content, documentation has been shown partially)":note;
                    note += Environment.NewLine;
                    note += Environment.NewLine;
                    note += "Above content copyright © 1996-2019 EnergyPlus, all contributors. All rights reserved. EnergyPlus is a trademark of the US Department of Energy.";
                    this.OwnerEpNote = note;
                }
                
                this._items.UpdateFromEpDoc(type);
            }
            

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
            
            foreach (var item in dfSet)
            {
                var found = iddFields.FirstOrDefault(_ => CleanFULLNAME(_.name()) == item.FULLNAME);

                if (found is null) continue;
                item.UpdateFromIddField(found);
            }

            return dfSet;

        }

        public static void UpdateFromEpDoc(this IEnumerable<IB_Field> iB_fields, Type EpDocType)
        {
            
            foreach (var item in iB_fields)
            {
                var fieldNameInEpDoc = string.Format("FIELD_{0}", item.FULLNAME);
                var found = EpDocType.GetProperties().FirstOrDefault(_ => _.Name.ToUpper() == fieldNameInEpDoc);
                if (found is null) continue;

                string fieldNote = found.GetValue(null, null) as string;
                
                item.AddDescriptionFromEpNote(fieldNote);
            }
            

        }

        /// <summary>
        /// Call this method to get all fields that inside of this fieldset.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IB_Field> GetSelfPreperties(this IB_FieldSet derivedDataFieldSet)
        {

            return derivedDataFieldSet.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Select(_ => (IB_Field)_.GetValue(derivedDataFieldSet, null));
            

        }

        /// <summary>
        /// Map properties of the ProDataField or BasicDataField that defined in derived class, to DataFieldSet's IB_DataField collection.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IB_Field> MergeFromOSMethods(this IEnumerable<IB_Field> fieldItemCollection, IEnumerable<IB_Field> openStudioSetters)
        {
            var mergedFields = fieldItemCollection.ToList();
            openStudioSetters.ToList().ForEach(_ =>
            {
                
                var fieldMatched = mergedFields.GetByName(_.FULLNAME);
                if (fieldMatched is null)
                {
                    //setterMethod has not been included in fieldItemCollection
                    mergedFields.Add(_);
                }
                else
                {
                    //TODO: this is not the best practice
                    fieldMatched.DataType = _.DataType;
                    fieldMatched.DetailedDescription = _.DataType == typeof(bool)? $"{fieldMatched.DetailedDescription}\r\nPlease use TRUE or FALSE to set this value regardless whatever suggested below!" : fieldMatched.DetailedDescription;
                    fieldMatched.SetterMethod = _.SetterMethod;
                }

            });
            
            return mergedFields;

        }

    }
}
