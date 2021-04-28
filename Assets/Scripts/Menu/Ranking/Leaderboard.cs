using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using PlayGames;
using PlayGames.Dao;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

namespace Menu.Ranking
{
    public class Leaderboard : MonoBehaviour
    {
        [Header("Ranking List")]
        [SerializeField] private VerticalLayoutGroup rankingItems;
        [SerializeField] private GameObject rankingItemPrefab;
        [SerializeField] private GameObject viewport;

        [Header("Status")]
        [SerializeField] private GameObject noDataLabel;
        [SerializeField] private GameObject loader;

        private CanvasGroup _canvasGroup;

        private void OnEnable()
        {
            Load();
        }

        private void Awake()
        {
            _canvasGroup = viewport.GetComponent<CanvasGroup>();
        }

        public void Load()
        {
            noDataLabel.SetActive(false);
            loader.SetActive(true);
            _canvasGroup.alpha = 0.5f;

            PlayGamesPlatform.Instance.LoadScores(
                Gps.LeaderboardHighScore,
                LeaderboardStart.TopScores,
                25,
                LeaderboardCollection.Public,
                LeaderboardTimeSpan.AllTime,
                scoreData =>
                {
                    StartCoroutine(GetLeaderboardPlayers(scoreData, users =>
                    {
                        if (users.Count > 0)
                        {
                            Reset();

                            noDataLabel.SetActive(false);
                            loader.SetActive(false);
                            _canvasGroup.alpha = 1;

                            foreach (var user in users)
                            {
                                var item = Instantiate(rankingItemPrefab, rankingItems.transform)
                                    .GetComponent<RankingItem>();
                                item.SetData(user);
                            }
                        }
                        else
                        {
                            noDataLabel.SetActive(true);
                            loader.SetActive(false);
                            _canvasGroup.alpha = 1;
                        }
                    }));
                });
        }

        private static IEnumerator GetLeaderboardPlayers(LeaderboardScoreData scoreData,
            Action<List<UserProfile>> callback)
        {
            if (!scoreData.Valid ||
                (scoreData.Status != ResponseStatus.Success &&
                 scoreData.Status != ResponseStatus.SuccessWithStale))
            {
                Debug.Log("DEBUG+: Valid: " + scoreData.Valid + ", Status: " + scoreData.Status);
                yield break;
            }

            Debug.Log("DEBUG+: Scores loaded successful!");

            var scores = new List<IScore>();
            scores.AddRange(scoreData.Scores);

            for (var i = 0; i < 3; i++)
            {
                PlayGamesPlatform.Instance.LoadMoreScores(scoreData.NextPageToken, 25, nextScoreData =>
                {
                    if (nextScoreData.Valid &&
                        (nextScoreData.Status == ResponseStatus.Success ||
                         nextScoreData.Status == ResponseStatus.SuccessWithStale))
                        scores.AddRange(nextScoreData.Scores);
                });
            }

            var usersProfiles = new List<UserProfile>();
            var usersIDs = new List<string>(scores.Select(score => score.userID));

            yield return LoadUsersByIds(usersIDs.ToArray(), users => { usersProfiles = users; });

            for (var i = 0; i < usersProfiles.Count; i++)
            {
                var user = usersProfiles[i];

                if (user.Id == scoreData.Scores[i].userID)
                {
                    user.Score = scoreData.Scores[i].value;
                    user.Place = scoreData.Scores[i].rank;

                    if (user.Id == PlayGamesPlatform.Instance.localUser.id)
                        LeaderboardEventSystem.Instance.UserLoaded(user);
                }
            }

            callback.Invoke(usersProfiles);
        }

        private static IEnumerator LoadUsersByIds(string[] usersIds, Action<List<UserProfile>> callback)
        {
            var isUsersLoaded = false;
            var usersProfiles = new List<UserProfile>();

            PlayGamesPlatform.Instance.LoadUsers(usersIds, users =>
            {
                if (users.Length > 0)
                {
                    foreach (var user in users)
                    {
                        usersProfiles.Add(
                            new UserProfile(user.id, user.userName, user.isFriend, user.state, user.image));
                        Debug.Log("DEBUG+: User: " + user.userName + " loaded!");
                    }
                }
                else
                {
                    Debug.Log("DEBUG+: Users Length = 0");
                }

                isUsersLoaded = true;
            });

            yield return new WaitUntil(() => isUsersLoaded);

            callback.Invoke(usersProfiles);
        }

        public void Reset()
        {
            foreach (Transform child in rankingItems.transform)
                Destroy(child.gameObject);
        }
    }
}