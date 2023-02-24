using System;
using Obert.UI.Runtime.Repeaters;
using UnityEngine;

namespace Samples.Repeaters
{
    [CreateAssetMenu(menuName = "Ui Samples/Users Async Data Source", fileName = "Users Async Data Source", order = 0)]
    public sealed class ScriptableUserAsyncDataSource : ScriptableDataSource<UserAsync>
    {
        [SerializeField]
        private UserAsync[] items;

        public override UserAsync[] DataItems => items;
        public override void BindItem<TItemInstance>(UserAsync data, TItemInstance instance)
        {
            if (instance is not UserAsyncListItem asyncUserListItem)
            {
                throw ThrowListItemCastException<UserAsync, UserAsyncListItem>();
            }
            
            asyncUserListItem.Bind(data);
        }
    }
}