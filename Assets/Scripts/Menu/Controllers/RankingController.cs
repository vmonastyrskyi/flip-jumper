using System.Collections;
using Menu.Ranking;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu.Controllers
{
    public class RankingController : MonoBehaviour
    {
        [SerializeField] private GameObject platformWithPlayer;

        [Header("Panels")]
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private GameObject rankingPanel;

        [Header("Buttons")]
        [SerializeField] private Button refreshButton;
        [SerializeField] private Button closeButton;

        [Header("User Statistics")]
        [SerializeField] private TextMeshProUGUI place;
        [SerializeField] private TextMeshProUGUI highScore;

        [Header("Leaderboard")]
        [SerializeField] private Leaderboard leaderboard;

        private void Awake()
        {
            if (refreshButton != null)
                refreshButton.onClick.AddListener(leaderboard.Load);
            if (closeButton != null)
                closeButton.onClick.AddListener(ClosePanel);
        }

        private IEnumerator Start()
        {
            yield return null;

            LeaderboardEventSystem.Instance.OnUserLoaded += user =>
            {
                place.SetText(user.Place.ToString());
                highScore.SetText(user.Score.ToString());
            };
        }

        private void ClosePanel()
        {
            platformWithPlayer.SetActive(true);
            rankingPanel.SetActive(false);
            menuPanel.SetActive(true);
        }
    }
}