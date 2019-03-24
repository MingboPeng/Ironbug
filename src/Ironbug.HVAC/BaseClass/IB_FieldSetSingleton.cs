using OpenStudio;
using System;

namespace Ironbug.HVAC.BaseClass
{
    /// <summary>
    /// Handle the IB_FieldSet's children's singleton 
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

        internal override Type RefOpsType => typeof(K);
        internal override Type RefEpType => typeof(K);

        protected IB_FieldSet():base()
        {

        }
    }
    
}
