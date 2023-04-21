using System;
using UnityEngine;

namespace Obert.UI.Runtime.Repeaters
{
    public abstract class RepeaterView : MonoBehaviour
    {
        public abstract void BindDataSource(IDataSource dataSource);
    }
    
    public abstract class RepeaterView<TData, TItemInstance> : RepeaterView where TItemInstance : Component
    {
        [SerializeField] private Transform container;
        private TItemInstance[] _instances = Array.Empty<TItemInstance>();
        protected Transform Container => container;
        
        private void Awake()
        {
            if (!container)
                container = transform;
        }

        protected abstract Func<TItemInstance> ItemFactory { get; }

        public override void BindDataSource(IDataSource dataSource) => BindDataSource((IDataSource<TData>)dataSource);

        public void BindDataSource(IDataSource<TData> dataSource) => BindData(dataSource.DataItems, dataSource.BindItem);

        public void BindData(TData[] data, Action<TData, TItemInstance> bindAction)
        {
            if (data.Length > _instances.Length)
            {
                var temp = _instances;
                _instances = new TItemInstance[data.Length];
                Array.Copy(temp, _instances, temp.Length);
            }

            for (int i = 0; i < data.Length; i++)
            {
                var instance = _instances[i];
                if (instance == null)
                {
                    _instances[i] = instance = ItemFactory();
                }

                bindAction(data[i], instance);
                _instances[i].gameObject.SetActive(true);
            }

            var overflowInstances = _instances.Length - data.Length;
            if (overflowInstances > 0)
            {
                for (int i = data.Length - 1; i < _instances.Length; i++)
                {
                    _instances[i].gameObject.SetActive(false);
                }
            }
        }
    }
}