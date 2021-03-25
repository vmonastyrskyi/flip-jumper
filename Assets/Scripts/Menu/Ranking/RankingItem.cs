using Menu.Controllers;
using TMPro;
using UnityEngine;

namespace Menu.Ranking
{
    public class RankingItem : MonoBehaviour
    {
        [SerializeField] private GameObject userRank;
        [SerializeField] private TextMeshProUGUI userName;
        [SerializeField] private GameObject userScore;

        private void Awake()
        {
        }

        private void SetData(RankingController.UserScore data)
        {
            var icon = userRank.transform.GetChild(0);
            var rank = userRank.transform.GetChild(0).GetChild(0);

            var score = userScore.transform.GetChild(1);
            
            userName.SetText(data.Name);
        }
    }
}