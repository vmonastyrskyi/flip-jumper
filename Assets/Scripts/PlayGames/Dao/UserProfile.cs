using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace PlayGames.Dao
{
    public class UserProfile
    {
        public string Id { get; }
        
        public string UserName { get; }
        
        public bool IsFriend { get; }
        
        public UserState State { get; }
        
        public Texture2D Image { get; }
        
        public int Place { get; set; }
        
        public long Score { get; set; }
        
        public UserProfile(
            string id,
            string userName,
            bool isFriend,
            UserState state,
            Texture2D image)
        {
            Id = id;
            UserName = userName;
            IsFriend = isFriend;
            State = state;
            Image = image;
        }
    }
}