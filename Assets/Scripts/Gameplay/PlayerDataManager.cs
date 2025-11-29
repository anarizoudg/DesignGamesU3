using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Platformer.Gameplay
{
    public class PlayerDataManager : MonoBehaviour
    {
        public string playerName = "Player1";
        private string url = "http://localhost:8081/UnityGame/player_progress.php";
        // Guardar progreso
        public void SaveProgress(int totalCoins, bool level1Completed, bool level2Completed)
        {
            StartCoroutine(SaveProgressCoroutine(totalCoins, level1Completed, level2Completed));
        }

        private IEnumerator SaveProgressCoroutine(int totalCoins, bool level1Completed, bool level2Completed)
        {
            WWWForm form = new WWWForm();
            form.AddField("action", "save");
            form.AddField("playerName", playerName);
            form.AddField("totalCoins", totalCoins);
            form.AddField("level1Completed", level1Completed ? 1 : 0);
            form.AddField("level2Completed", level2Completed ? 1 : 0);

            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("❌ Save Error: " + www.error);
            }
            else
            {
                Debug.Log("✅ Save Response: " + www.downloadHandler.text);
            }
        }

        // Cargar progreso
        public void LoadProgress(System.Action<int, bool, bool> callback)
        {
            StartCoroutine(LoadProgressCoroutine(callback));
        }

        private IEnumerator LoadProgressCoroutine(System.Action<int, bool, bool> callback)
        {
            WWWForm form = new WWWForm();
            form.AddField("action", "load");
            form.AddField("playerName", playerName);

            UnityWebRequest www = UnityWebRequest.Post(url, form);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("❌ Load Error: " + www.error);
                callback?.Invoke(0, false, false);
            }
            else
            {
                string json = www.downloadHandler.text;
                var data = JsonUtility.FromJson<PlayerProgressResponse>(json);
                if (data.status == "success")
                {
                    callback?.Invoke(
                        data.data.totalCoins,
                        data.data.level1Completed == 1,
                        data.data.level2Completed == 1
                    );
                }
                else
                {
                    callback?.Invoke(0, false, false);
                }
            }
        }

        [System.Serializable]
        private class PlayerProgressResponse
        {
            public string status;
            public PlayerProgressData data;
        }

        [System.Serializable]
        private class PlayerProgressData
        {
            public int totalCoins;
            public int level1Completed;
            public int level2Completed;
        }
    }
}
