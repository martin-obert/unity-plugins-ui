using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes
{
    public sealed class UserProfileView : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text label;
        private string _profileId;
        public void Bind(UserProfile data, Action<UserProfile> onClick)
        {
            _profileId = data.UserId;
            label.text = data.UserId.ToString();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onClick(data));
        }

        public override bool Equals(object other)
        {
            if (other is not UserProfile userProfile) return base.Equals(other);

            return userProfile.UserId == _profileId;
        }
    }
}