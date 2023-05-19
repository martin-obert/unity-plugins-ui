using UnityEngine;

namespace Obert.UI.Runtime.Repeaters
{
    public abstract class ScriptableDataSourceBase : ScriptableObject, IReadOnlyDataSource
    {

        public abstract object[] Items { get; }

        public abstract void BindItem<TItemInstance>(object data, TItemInstance instance)
            where TItemInstance : Component;
    }
}