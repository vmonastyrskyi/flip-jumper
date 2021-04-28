using PlayGames;
using PlayGames.Dao;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using Util;

namespace Menu.Ranking
{
    public class RankingItem : MonoBehaviour
    {
        [SerializeField] private GameObject place;
        [SerializeField] private TextMeshProUGUI profileName;
        [SerializeField] private TextMeshProUGUI score;
        [SerializeField] private Sprite goldRankIcon;
        [SerializeField] private Sprite silverRankIcon;
        [SerializeField] private Sprite bronzeRankIcon;
        [SerializeField] private Sprite singleRankIcon;

        private SVGImage _placeIcon;
        private TextMeshProUGUI _placeLabel;

        private void Awake()
        {
            _placeIcon = place.transform.GetChild(0).GetComponent<SVGImage>();
            _placeLabel = place.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        }

        public void SetData(UserProfile userProfile)
        {
            switch (userProfile.Place)
            {
                case 1:
                    _placeIcon.sprite = goldRankIcon;
                    break;
                case 2:
                    _placeIcon.sprite = silverRankIcon;
                    break;
                case 3:
                    _placeIcon.sprite = bronzeRankIcon;
                    break;
                default:
                    _placeIcon.sprite = singleRankIcon;
                    break;
            }

            _placeLabel.SetText(userProfile.Place.ToString());
            profileName.SetText(userProfile.UserName);
            score.SetText(userProfile.Score.ToString());
        }
    }
}