using System;
using System.Linq;
using UnityEngine;

namespace Obert.UI.Runtime.Repeaters
{
    public abstract class ScriptableDataSource<TData> : ScriptableDataSourceBase, IDataSource<TData>
    {
        public abstract TData[] DataItems { get; }

        public override object[] Items => DataItems.Select(x=>x as object).ToArray();

        public override void BindItem<TItemInstance>(object data, TItemInstance instance) => BindItem((TData)data, instance);

        public abstract void BindItem<TItemInstance>(TData data, TItemInstance instance)
            where TItemInstance : Component;

        protected Exception ThrowListItemCastException<TFrom, TTo>() =>
            throw new InvalidCastException($"Unable cast from: {typeof(TFrom)} to: {typeof(TTo)}");
    }
}