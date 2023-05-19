using System;
using System.Collections.Generic;
using Obert.Common.Runtime.Extensions;
using UnityEngine;

namespace Obert.UI.Runtime.Repeaters
{
    public abstract class RepeaterView : MonoBehaviour
    {
        public abstract void BindDataSource(IReadOnlyDataSource readOnlyDataSource);
    }

    public abstract class RepeaterView<TData, TItemInstance> : RepeaterView where TItemInstance : Component
    {
        [SerializeField] private Transform container;
        [SerializeField] private bool showEmptySlots;
        [SerializeField] private int emptySlotsCount;

        private readonly List<TItemInstance> _instances = new();
        private GameObject[] _emptyInstances = Array.Empty<GameObject>();
        protected List<TItemInstance> Instances => _instances;
        protected Transform Container => container;


        protected virtual void Awake()
        {
            if (!container)
                container = transform;
        }

        protected virtual void Start()
        {
            if (!showEmptySlots) return;

            container.ClearChildGameObjects();
            _emptyInstances = new GameObject[emptySlotsCount];
            for (var i = 0; i < _emptyInstances.Length; i++)
            {
                _emptyInstances[i] = EmptySlotFactory.Invoke();
            }
        }

        protected abstract Func<TItemInstance> ItemFactory { get; }
        protected virtual Func<GameObject> EmptySlotFactory { get; }

        private IDisposable _dataSourceSub;

        public override void BindDataSource(IReadOnlyDataSource readOnlyDataSource) => BindDataSource((IReadOnlyDataSource<TData>)readOnlyDataSource);

        protected virtual void BindDataSource(IReadOnlyDataSource<TData> dataSource) => BindData(dataSource.DataItems, dataSource.BindItem);


        protected virtual void CreateInstance(TData data, Action<TData, TItemInstance> bindAction)
        {
            var instance = ItemFactory();
            bindAction(data, instance);
            _instances.Add(instance);
        }

        protected virtual void DeleteInstance(TItemInstance instance)
        {
            _instances.Remove(instance);
        }

        protected virtual void ClearAllInstances()
        {
            Container.ClearChildGameObjects();
            _instances.Clear();
        }

        protected virtual void BindData(TData[] data, Action<TData, TItemInstance> bindAction)
        {
            ClearAllInstances();

            for (var i = 0; i < data.Length; i++)
            {
                CreateInstance(data[i], bindAction);
            }
        }
    }
}