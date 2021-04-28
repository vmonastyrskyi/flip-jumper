using System;
using System.Text;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using PlayGames.Dao;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace PlayGames
{
    public class PlayGamesServices : MonoBehaviour
    {
        private const string CloudSaveName = "cloud_data";

        public static ILocalUser LocalUser => PlayGamesPlatform.Instance.localUser;

        public static bool IsAuthenticated => PlayGamesPlatform.Instance.localUser.authenticated;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            var config = new PlayGamesClientConfiguration.Builder()
                .RequestServerAuthCode(false)
                .EnableSavedGames()
                .Build();

            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
        }

        public static void Authenticate(Action<bool> callback)
        {
            PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, status =>
                {
                    if (status == SignInStatus.Success)
                        callback.Invoke(true);
                    else
                        callback.Invoke(false);
                }
            );
        }

        public static void LoadCloudData(Action<CloudData> callback)
        {
            if (IsAuthenticated)
                PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConflictResolution(
                    CloudSaveName,
                    DataSource.ReadCacheOrNetwork,
                    ConflictResolutionStrategy.UseLongestPlaytime,
                    (status, metadata) => OnSavedGameOpened(status, metadata, false, null, callback)
                );
            else
                callback?.Invoke(null);
        }

        public static void SaveCloudData(CloudData data)
        {
            if (IsAuthenticated)
                PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConflictResolution(
                    CloudSaveName,
                    DataSource.ReadCacheOrNetwork,
                    ConflictResolutionStrategy.UseLongestPlaytime,
                    (status, metadata) => OnSavedGameOpened(status, metadata, true, data)
                );
        }

        private static void OnSavedGameOpened(
            SavedGameRequestStatus status, ISavedGameMetadata metadata,
            bool saving, CloudData data, Action<CloudData> callback = null)
        {
            if (status == SavedGameRequestStatus.Success)
            {
                if (saving)
                    PlayGamesPlatform.Instance.SavedGame.CommitUpdate(
                        metadata,
                        new SavedGameMetadataUpdate.Builder().Build(),
                        Encoding.ASCII.GetBytes(JsonUtility.ToJson(data)),
                        OnSavedGameWritten
                    );
                else
                    PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(
                        metadata,
                        (readStatus, readData) => OnSavedGameRead(readStatus, readData, callback)
                    );
            }
        }

        private static void OnSavedGameRead(SavedGameRequestStatus status, byte[] data, Action<CloudData> callback)
        {
            if (status == SavedGameRequestStatus.Success)
            {
                var jsonData = Encoding.ASCII.GetString(data);

                if (jsonData == string.Empty)
                    callback.Invoke(null);
                else
                    callback.Invoke(JsonUtility.FromJson<CloudData>(jsonData));
            }
        }

        private static void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata metadata)
        {
        }

        public static void ReportScore(string board, int score)
        {
            PlayGamesPlatform.Instance.ReportScore(score, board, null);
        }

        public static void LoadLocalUser()
        {
            PlayGamesPlatform.Instance.LoadUsers(
                new[] {PlayGamesPlatform.Instance.localUser.id},
                user => { }
            );
        }
    }
}