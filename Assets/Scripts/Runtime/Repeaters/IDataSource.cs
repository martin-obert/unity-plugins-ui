using UnityEngine;

namespace Obert.UI.Runtime.Repeaters
{
    public interface IDataSource
    {
        object[] Items { get; }
        void BindItem<TItemInstance>(object data, TItemInstance instance) where TItemInstance : Component;
    }

    public interface IDataSource<TData> : IDataSource
    {
        TData[] DataItems { get; }
        void BindItem<TItemInstance>(TData data, TItemInstance instance) where TItemInstance : Component;
    }
}