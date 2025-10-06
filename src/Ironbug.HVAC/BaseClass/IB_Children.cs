﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Ironbug.HVAC.BaseClass
{

    [DataContract]
    public class IB_Children : List<IB_ModelObject>, System.IEquatable<IB_Children>
    {

        public IB_Children()
        {
        }

        public IB_Children(int capacity):base(capacity)
        {
            this.AddRange(Enumerable.Repeat<IB_ModelObject>(null, capacity));
        }

        internal void SetChild<T>(T ChildObj) where T : IB_ModelObject
        {
            var item = this.FirstOrDefault(_ => _ is T);
            if (item == null)
            {
                this.Add(ChildObj);
            }
            else
            {
                var index = this.IndexOf(item);
                this[index] = ChildObj;
            }
        }
        internal void SetChild<T>(int ChildIndex, T ChildObj) where T : IB_ModelObject
        {
            var child = this[ChildIndex];
            if (child is null || child is T)
            {
                this[ChildIndex] = ChildObj;
            }
            else
            {
                throw new ArgumentException($"Set {ChildObj.GetType()} to {child.GetType()} failed!");
            }
        }

        public T Get<T>() where T : IB_ModelObject => this.GetChild<T>();

        public T GetChild<T>() where T : IB_ModelObject
        {
            return this.FirstOrDefault(_ => _ is T) as T;
        }
        public T Get<T>(int childIndex) where T : IB_ModelObject => this.GetChild<T>(childIndex);
        public T GetChild<T>(int childIndex) where T : IB_ModelObject
        {
            if (!(this.Count > childIndex)) return (T)null;

            var child = this[childIndex];
            if (child is null) return (T)null;

            if (child is T)
            {
                return child as T;
            }
            else
            {
                throw new ArgumentException($"Child at {childIndex} doesn't match type: {typeof(T)}!");
            }
        }

        public override bool Equals(object obj) => this.Equals(obj as IB_Children);

        public bool Equals(IB_Children other)
        {
            if (other is null)
                return false;
            return this.SequenceEqual(other);
        }
        public static bool operator ==(IB_Children x, IB_Children y)
        {
            if (x is null)
                return y is null ? true : false;
            return x.Equals(y);
        }

        public static bool operator !=(IB_Children x, IB_Children y) => !(x == y);


    }

}
