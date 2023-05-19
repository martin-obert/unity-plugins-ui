using System;

namespace Scenes
{
    public sealed class UserProfile
    {
        public UserProfile(int i)
        {
            UserId = $"Item_{i}";
        }

        public string UserId { get; }
    }
}