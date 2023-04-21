using System;
using Obert.UI.Runtime.Repeaters;
using UnityEngine;

namespace Samples.Repeaters
{
    [Serializable]
    public sealed class UserAsync
    {
        public string avatarUrl;
    }
    
    [Serializable]
    public sealed class User
    {
        [SerializeField] private string name;

        [SerializeField] private Sprite avatar;

        public string Name => name;

        public Sprite Avatar => avatar;
    }

    public sealed class UserList : RepeaterView<User, UserListItem>
    {
        [SerializeField] private UserListItem prefab;
        protected override Func<UserListItem> ItemFactory => () => Instantiate(prefab, Container);
    }
}