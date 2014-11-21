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
            : this("[]")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomFieldsCollection"/> class.
        /// </summary>
        /// <param name="json">
        /// The serialized JSON representation of the <see cref="CustomFieldsCollection"/>.
        /// </param>
        internal CustomFieldsCollection(string json)
        {
            this.Initialize(json);
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

        /// <summary>
        /// Serializes this collection to a JSON string
        /// </summary>
        /// <returns>
        /// The JSON <see cref="string"/> representation of this collection.
        /// </returns>
        internal string SerializeAsJson()
        {
            return JsonConvert.SerializeObject(this);
        }

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

        private void Initialize(string json)
        {
            if (string.IsNullOrEmpty(json)) json = "[]";

            var fieldData = JsonConvert.DeserializeObject<IEnumerable<ICustomField>>(json).ToArray();

            foreach (var field in fieldData)
            {
                this.SetValue(field);
            }
        }
    }
}