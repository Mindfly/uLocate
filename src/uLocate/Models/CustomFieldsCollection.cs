using System;

namespace uLocate.Models
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;

    using Newtonsoft.Json;

    /// <summary>
    /// The field value collection.
    /// </summary>
    public class CustomFieldsCollection : ConcurrentDictionary<string, ICustomField>, INotifyCollectionChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomFieldsCollection"/> class.
        /// </summary>
        public CustomFieldsCollection()
            : this(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomFieldsCollection"/> class.
        /// </summary>
        internal CustomFieldsCollection(int LocationTypeId)
        {
            //if (LocationTypeId != 0)
            //{
            //    this.Initialize(LocationTypeId);
            //}
        }

        /// <summary>
        /// The collection changed event
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// The set value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public void SetValue(ICustomField value)
        {
            AddOrUpdate(value.Alias, value, (x, y) => value);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value));
        }

        /// <summary>
        /// The remove value.
        /// </summary>
        /// <param name="alias">
        /// The alias.
        /// </param>
        public void RemoveValue(string alias)
        {
            ICustomField obj;
            TryRemove(alias, out obj);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, obj));
        }

        /// <summary>
        /// The clear.
        /// </summary>
        public new void Clear()
        {
            base.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// The get value.
        /// </summary>
        /// <param name="alias">
        /// The alias.
        /// </param>
        /// <returns>
        /// The <see cref="ICustomField"/>.
        /// </returns>
        public ICustomField GetValue(string alias)
        {
            return ContainsKey(alias) ? this[alias] : null;
        }

        ///// <summary>
        ///// Serializes this collection to a JSON string
        ///// </summary>
        ///// <returns>
        ///// The JSON <see cref="string"/> representation of this collection.
        ///// </returns>
        //internal string SerializeAsJson()
        //{
        //    return JsonConvert.SerializeObject(this);
        //}

        /// <summary>
        /// The on collection changed.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, args);
            }
        }

        private void InitializeCustomTypeFields(int LocationTypeId)
        {
            //var CustomLocationType = 
            //foreach (var field in fieldData)
            //{
            //    this.SetValue(field);
            //}
        }

         private void InitializeDefaultTypeFields()
         {
            // var CustomLocationType = new LocationType(Constants.DefaultLocationTypeId);

            //foreach (var field in fieldData)
            //{
            //    this.SetValue(field);
            //}
        }
    }
}