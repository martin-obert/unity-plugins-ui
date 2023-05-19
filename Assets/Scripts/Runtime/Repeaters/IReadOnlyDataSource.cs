using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using UnityEngine;

namespace Obert.UI.Runtime.Repeaters
{
    public interface IReadOnlyDataSource
    {
        object[] Items { get; }
        void BindItem<TItemInstance>(object data, TItemInstance instance) where TItemInstance : Component;

    }

    public interface IReadOnlyDataSource<TData> : IReadOnlyDataSource
    {
        TData[] DataItems { get; }
        void BindItem<TItemInstance>(TData data, TItemInstance instance) where TItemInstance : Component;
    }

    public interface IDataSource<TData> : IReadOnlyDataSource<TData>, INotifyCollectionChanged
    {
        void AddItem(TData item);
        void RemoveItem(TData item);
        void AddBulk(TData[] items);

        void RemoveBulk(TData[] items);
        void RemoveWhere(Func<TData, bool> func);
    }
}