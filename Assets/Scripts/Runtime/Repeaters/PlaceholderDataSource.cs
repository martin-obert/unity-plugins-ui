using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

namespace Obert.UI.Runtime.Repeaters
{
    public abstract class PlaceholderDataSource<TData, TView> : PlaceholderDataSource<TData> where TView : Component
    {
        protected abstract void BindItem(TData data, TView view);

        public override void BindItem<TItemInstance>(TData data, TItemInstance instance)
        {
            if (instance is not TView view) throw new Exception();

            BindItem(data, view);
        }
    }

    public abstract class PlaceholderDataSource<TData> : IDataSource<TData>
    {
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public object[] Items => DataItems.Cast<object>().ToArray();

        public TData[] DataItems => DataList.ToArray();

        protected readonly List<TData> DataList = new();

        protected PlaceholderDataSource()
        {
        }

        protected PlaceholderDataSource(IEnumerable<TData> dataItems)
        {
            DataList = new List<TData>(dataItems);
        }

        public void BindItem<TItemInstance>(object data, TItemInstance instance) where TItemInstance : Component
        {
            if (data is not TData item)
            {
                throw new Exception();
            }

            BindItem(item, instance);
        }

        public abstract void BindItem<TItemInstance>(TData data, TItemInstance instance) where TItemInstance : Component;

        public virtual void AddItem(TData item)
        {
            DataList.Add(item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        public virtual void RemoveItem(TData item)
        {
            if (DataList.Remove(item)) OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
        }

        public void AddBulk(TData[] items)
        {
            DataList.AddRange(items);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items.ToList()));
        }

        public void RemoveBulk(TData[] items)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, items.ToList()));
        }

        public void RemoveWhere(Func<TData, bool> func)
        {
            RemoveBulk(DataList.Where(func).ToArray());
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }
    }
}