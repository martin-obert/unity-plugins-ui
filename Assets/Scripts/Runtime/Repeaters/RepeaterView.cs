using System;
using System.Collections.Generic;
using System.Linq;
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

        private GameObject[] _emptyInstances = Array.Empty<GameObject>();
        protected List<TItemInstance> Instances { get; } = new();

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

        public override void BindDataSource(IReadOnlyDataSource readOnlyDataSource) =>
            BindDataSource((IReadOnlyDataSource<TData>)readOnlyDataSource);

        protected virtual void BindDataSource(IReadOnlyDataSource<TData> dataSource) =>
            BindData(dataSource.DataItems, dataSource.BindItem);


        protected virtual void CreateInstance(TData data, Action<TData, TItemInstance> bindAction)
        {
            var freeInstance = Instances.FirstOrDefault(x => !x.gameObject.activeSelf);
            if (!freeInstance)
            {
                freeInstance = ItemFactory();
                if (showEmptySlots)
                {
                    freeInstance.transform.SetSiblingIndex(Instances.Count(x => x.gameObject.activeInHierarchy));
                }

                Instances.Add(freeInstance);
            }
            else if (freeInstance is IDisposable disposable)
            {
                disposable.Dispose();
            }

            bindAction(data, freeInstance);
            freeInstance.gameObject.SetActive(true);
            _emptyInstances.FirstOrDefault(x => x.activeInHierarchy)?.SetActive(false);
        }

        protected virtual void DeleteInstance(TItemInstance instance)
        {
            if (instance is IDisposable disposable)
            {
                disposable.Dispose();
            }

            instance.gameObject.SetActive(false);
            _emptyInstances.LastOrDefault(x => !x.activeInHierarchy)?.SetActive(true);
        }

        protected virtual void ClearAllInstances()
        {
            foreach (var itemInstance in Instances)
            {
                DeleteInstance(itemInstance);
            }
        }

        protected virtual void BindData(IEnumerable<TData> data, Action<TData, TItemInstance> bindAction)
        {
            var items = data as TData[] ?? data.ToArray();

            ClearAllInstances();

            foreach (var t in items)
            {
                CreateInstance(t, bindAction);
            }
        }
    }
}