using Ironbug.HVAC.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ironbug.HVAC
{

    //public delegate void LinkToChild<T>(T child) where T : IB_ModelObject;

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <typeparam name="T">Child type</typeparam>
    ///// <param name="child"></param>
    //public delegate void RelationWithChild<T>(T child) where T : IIB_ModelObject;
    ///// <summary>
    ///// 
    ///// </summary>
    ///// <typeparam name="T">Child1 type</typeparam>
    ///// <typeparam name="K">Child2 type</typeparam>
    ///// <param name="child1"></param>
    ///// <param name="child2"></param>
    //public delegate void RelationWithChild<T, K>(T child1, K child2) where T : IB_ModelObject where K : IB_ModelObject;

    public class IB_Child
    {
        private IB_ModelObject IB_Obj;
        //private Action<IB_ModelObject> linkAction;

        public IB_Child(IB_ModelObject ibObj)
        {
            this.IB_Obj = ibObj;
            //this.linkAction = link;
        }

        //invoke action
        //public void LinkToNewChild(IB_ModelObject NewChild)
        //{
        //    this.linkAction(NewChild);
            
        //}

        internal bool IsType(Type type)
        {
            return type.IsInstanceOfType(this.IB_Obj);
        }

        internal void Set(IB_ModelObject NewChild)
        {
            this.IB_Obj = NewChild;
        }

        public IB_Child Duplicate()
        {
            return new IB_Child(this.IB_Obj.Duplicate());
        }
        

        public T To<T>() where T: IB_ModelObject
        {
            return (IB_Obj as T);
        }
        public IB_ModelObject Get () 
        {
            return IB_Obj;
        }

        
        }

    //    public class IB_Child<T>: IIB_Child where T : IB_ModelObject
    //{
    //    public T IB_Obj;
    //    public LinkToChild<T> link;

    //    //public IB_Child()
    //    //{
    //    //}

    //    public IB_Child(T obj, LinkToChild<T> linkChild)
    //    {
    //        this.IB_Obj = obj;
    //        this.link = linkChild;
    //    }

    //    public IB_Child<T> Set(T obj)
    //    {
    //        return new IB_Child<T>(obj, link);
    //    }

    //    //public IB_Child<K> Get<K>(K obj) where K : IB_ModelObject
    //    //{
    //    //    return this;
    //    //}

    //    public T Get()
    //    {
    //        return IB_Obj;
    //    }

    //    public U Get<U>() where U : IB_ModelObject
    //    {
    //        return IB_Obj as U;
    //    }

    //    public T Duplicate()
    //    {
    //        return IB_Obj.Duplicate() as T;
    //    }

    //    //public Action<U> GetAction<U>() where U : IB_ModelObject
    //    //{
    //    //    if (typeof(U) == typeof(T))
    //    //    {
    //    //        return (obj) => link(Convert.ChangeType(obj,U));
    //    //    }
    //    //    else
    //    //    {
    //    //        return (obj) => obj.;
    //    //    }

    //    //    //return link;
    //    //}

    //    //public Action<T> GetAction()
    //    //{
    //    //    if (typeof(U) == typeof(T))
    //    //    {
    //    //        return (obj) => link(Convert.ChangeType(obj, U));
    //    //    }
    //    //    else
    //    //    {
    //    //        return (obj) => obj.;
    //    //    }

    //    //    //return link;
    //    //}
        

    //    public LinkToChild<T> GetAction()
    //    {
    //        return this.link;
    //    }

    //    LinkToChild<IB_ModelObject> IIB_Child.GetAction()
    //    {
    //        return this.link;
    //    }

    //    public IB_Child<T1> Inst<T1>() where T1 : IB_ModelObject
    //    {
    //        return 
    //    }

    //    public IB_Child<IB_ModelObject> Inst()
    //    {
    //        return this;
    //    }




    //    //public static implicit operator IB_Child<IB_ModelObject>(IB_Child<T> left) 
    //    //{
    //    //    return new IB_Child<K>(left.IB_Obj,left.link);
    //    //}

    //}

    //public class IB_Child
    //{
    //    public IB_ModelObject IB_Obj { get; set; }
    //    public Action<IB_ModelObject> linkTo;

    //    //public IB_Child()
    //    //{
    //    //}

    //    public IB_Child(IB_ModelObject obj, Action<IB_ModelObject> linkChild)
    //    {
    //        this.IB_Obj = obj;
    //        this.linkTo = linkChild;
    //    }

    //    public IB_Child Set(IB_ModelObject obj)
    //    {
    //        return new IB_Child(obj, linkTo);
    //    }

    //    //public IB_Child<K> Get<K>(K obj) where K : IB_ModelObject
    //    //{
    //    ////    return this;
    //    ////}

    //    //public IB_Child<K> Set<K>(K obj, Action<K> linkChild) where K : IB_ModelObject
    //    //{
    //    //    return new IB_Child<K>(obj, linkChild);
    //    //}

    //    //public static implicit operator IB_Child<IB_ModelObject>(IB_Child<T> left)
    //    //{
    //    //    return new IB_Child<K>(left.IB_Obj, left.link);
    //    //}


    //}

   
}
