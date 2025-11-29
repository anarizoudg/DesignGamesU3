using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Platformer.Gameplay
{
    public class PlayerProgressManager : MonoBehaviour
    {
        [Header("Datos del jugador")]
        public string playerName = "Player1";
        public int totalCoins = 0;

        [Header("PHP URL")]
        public string saveUrl = "http://localhost:8081/UnityGame/player_progress.php";
        public string loadUrl = "http://localhost:8081/UnityGame/player_progress.php?load=1";

        [Header("Referencia UI")]
        public TMPro.TextMeshProUGUI coinsText;

        void Start()
        {
            // Cargar progreso al iniciar
            StartCoroutine(LoadProgress());
        }

        public void AddCoins(int amount)
        {
            totalCoins += amount;
            UpdateUI();
            StartCoroutine(SaveProgress());
        }

        void UpdateUI()
        {
            if (coinsText != null)
                coinsText.text = "Monedas: " + totalCoins;
        }

        IEnumerator SaveProgress()
        {
            WWWForm form = new WWWForm();
            form.AddField("playerName", playerName);
            form.AddField("totalCoins", totalCoins);

            using (UnityWebRequest www = UnityWebRequest.Post(saveUrl, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                    Debug.LogError("❌ Error al guardar progreso: " + www.error);
                else
                    Debug.Log("✅ Progreso guardado");
            }
        }

        IEnumerator LoadProgress()
        {
            using (UnityWebRequest www = UnityWebRequest.Get(loadUrl + "&playerName=" + playerName))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("❌ Error al cargar progreso: " + www.error);
                }
                else
                {
                    // Espera recibir un número de monedas desde PHP
                    int coins;
                    if (int.TryParse(www.downloadHandler.text, out coins))
                    {
                        totalCoins = coins;
                        UpdateUI();
                        Debug.Log("✅ Progreso cargado: " + totalCoins + " monedas");
                    }
                    else
                    {
                        Debug.LogWarning("⚠️ Respuesta inesperada del servidor: " + www.downloadHandler.text);
                    }
                }
            }
        }
    }
}
