using System;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

namespace Obert.UI.Runtime.Repeaters
{
    /// <summary>
    /// Should be used only for IDataSource binding
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TItemInstance"></typeparam>
    public abstract class ReactiveRepeaterView<TData, TItemInstance> : RepeaterView<TData, TItemInstance> where TItemInstance : Component
    {

        private IDataSource<TData> _dataSource;

        protected override void BindDataSource(IReadOnlyDataSource<TData> readOnlyDataSource)
        {
            if (readOnlyDataSource is not IDataSource<TData> dataSource) throw new Exception();

            UnbindDataSource();
            _dataSource = dataSource;
            BindDataSource();
        }

        private void BindDataSource()
        {
            if (_dataSource != null)
                _dataSource.CollectionChanged += DataSourceOnCollectionChanged;
        }

        protected virtual void OnDestroy()
        {
            UnbindDataSource();
        }

        private void UnbindDataSource()
        {

            if (_dataSource != null)
                _dataSource.CollectionChanged -= DataSourceOnCollectionChanged;

            _dataSource = null;
        }

        private void DataSourceOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var newItem in e.NewItems)
                    {
                        var data = (TData)newItem;
                        var freeInstance = Instances.FirstOrDefault(x => !x.gameObject.activeInHierarchy);
                        if (!freeInstance)
                        {
                            CreateInstance(data, _dataSource.BindItem);
                        }
                    }
                    return;
            }

        }
    }
}