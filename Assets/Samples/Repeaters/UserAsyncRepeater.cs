using System;
using Obert.UI.Runtime.Repeaters;
using UnityEngine;

namespace Samples.Repeaters
{
    public sealed class UserAsyncRepeater : RepeaterView<UserAsync, UserAsyncListItem>
    {
        [SerializeField] private UserAsyncListItem prefab;
        protected override Func<UserAsyncListItem> ItemFactory => () => Instantiate(prefab, Container);
    }
}