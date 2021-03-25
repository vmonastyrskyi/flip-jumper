using System.Collections.Generic;
using Menu.Ranking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Controllers
{
    public class RankingController : MonoBehaviour
    {
        [SerializeField] private GameObject platformWithPlayer;

        [SerializeField] private VerticalLayoutGroup rankingItems;
        [SerializeField] private GameObject rankingItemPrefab;
        [SerializeField] private GameObject rankingEmptyItemPrefab;
        [SerializeField] private GameObject rankingItemSeparatorPrefab;

        [SerializeField] private GameObject menuPanel;
        [SerializeField] private GameObject rankingPanel;
        [SerializeField] private GameObject placePanel;
        [SerializeField] private GameObject highScorePanel;
        [SerializeField] private Button closeButton;

        private TextMeshProUGUI _place;
        private TextMeshProUGUI _highScore;

        private void Awake()
        {
            if (closeButton != null)
                closeButton.onClick.AddListener(ClosePanel);

            _place = placePanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            _highScore = highScorePanel.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        }

        private void ClosePanel()
        {
            platformWithPlayer.SetActive(true);
            rankingPanel.SetActive(false);
            menuPanel.SetActive(true);
        }

        public class UserScore
        {
            public int Place { get; set; }
            public string Name { get; set; }
            public int HighScore { get; set; }
        }

        private void OnEnable()
        {
            var userScore1 = new UserScore {Place = 1, Name = "Test 1", HighScore = 123};
            var userScore2 = new UserScore {Place = 2, Name = "Test 2", HighScore = 122};
            var userScore3 = new UserScore {Place = 3, Name = "Test 3", HighScore = 112};
            var userScore4 = new UserScore {Place = 4, Name = "Test 4", HighScore = 98};
            var userScore5 = new UserScore {Place = 5, Name = "Test 5", HighScore = 53};


            var data = new List<UserScore>()
            {
                userScore1, userScore2, userScore3, userScore4, userScore5
            };

            foreach (Transform child in rankingItems.transform)
            {
                Destroy(child.gameObject);
            }

            for (var i = 0; i < data.Count; i++)
            {
                var userScore = data[i];
                var rankingItem = Instantiate(rankingItemPrefab, rankingItems.transform).GetComponent<RankingItem>();
                
                if (i != data.Count - 1)
                {
                    Instantiate(rankingItemSeparatorPrefab, rankingItems.transform);
                }
            }

            Instantiate(rankingEmptyItemPrefab, rankingItems.transform);
        }
    }
}