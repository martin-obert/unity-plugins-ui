using Obert.UI.Runtime.Repeaters;
using UnityEngine;

namespace Samples.Repeaters
{
    [CreateAssetMenu(menuName = "Ui Samples/Users Data Source", fileName = "Users Data Source", order = 0)]
    public sealed class ScriptableUsersDataSource : ScriptableDataSource<User>
    {
        [SerializeField] private User[] items;

        public override User[] DataItems => items;
        
        public override void BindItem<TItemInstance>(User data, TItemInstance instance)
        {
            if (instance is not UserListItem item)
            {
                throw ThrowListItemCastException<TItemInstance, UserListItem>();
            }

            item.userName.text = data.Name;
            item.avatar.sprite = data.Avatar;
        }
    }
}