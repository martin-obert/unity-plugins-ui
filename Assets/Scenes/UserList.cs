using System;
using System.Collections.Generic;
using System.Linq;
using Obert.UI.Runtime.Repeaters;
using UnityEngine;

namespace Scenes
{
    public sealed class DataSource : PlaceholderDataSource<UserProfile, UserProfileView>
    {
        private Action<UserProfile> _onSelect;

        public DataSource(IEnumerable<UserProfile> data, Action<UserProfile> onSelect) : base(data)
        {
            _onSelect = onSelect;
        }

        protected override void BindItem(UserProfile data, UserProfileView view)
        {
            view.Bind(data, _onSelect);
        }
    }

    public class UserList : ReactiveRepeaterView<UserProfile, UserProfileView>
    {
        [SerializeField] private UserProfileView prefab;
        [SerializeField] private UserList other;
        [SerializeField] private GameObject emptySlot;
        [SerializeField] private int dataCount;
        protected override Func<UserProfileView> ItemFactory => () => Instantiate(prefab, Container);
        protected override Func<GameObject> EmptySlotFactory => () => Instantiate(emptySlot, Container);
        private DataSource _dataSource;

        protected override void Awake()
        {
            base.Awake();

            var data = new UserProfile[dataCount];
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = new UserProfile(i);
            }
            _dataSource = new DataSource(data, v =>
            {
                _dataSource.RemoveItem(v);
                other._dataSource.AddItem(v);
            });
            BindDataSource(_dataSource);
        }
    }
}