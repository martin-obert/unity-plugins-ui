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

        protected PlaceholderDataSource(IEnumerable<TData> dataItems) : base(dataItems)
        {
        }

        protected PlaceholderDataSource()
        {
        }

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

        public TData[] DataItems => _dataList.ToArray();

        private readonly List<TData> _dataList = new();

        protected PlaceholderDataSource()
        {
        }

        protected PlaceholderDataSource(IEnumerable<TData> dataItems)
        {
            _dataList = new List<TData>(dataItems);
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
            _dataList.Add(item);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        public virtual void RemoveItem(TData item)
        {
            if (_dataList.Remove(item))
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
        }

        public virtual void AddBulk(TData[] items)
        {
            _dataList.AddRange(items);
            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items.ToList()));
        }

        public virtual void RemoveBulk(TData[] items)
        {
            foreach (var item in items)
            {
                _dataList.Remove(item);
            }
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, items.ToList()));
        }

        public virtual void RemoveWhere(Func<TData, bool> func)
        {
            RemoveBulk(_dataList.Where(func).ToArray());
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        public virtual void Clear()
        {
            var itemsCopy = _dataList.ToArray();
            _dataList.Clear();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, itemsCopy.ToList()));

        }
    }
}