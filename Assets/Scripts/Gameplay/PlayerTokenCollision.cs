using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;

namespace Platformer.Gameplay
{
    /// <summary>
    /// Se ejecuta cuando el jugador colisiona con un token.
    /// Guarda el progreso usando PlayerDataManager (PHP + MySQL).
    /// </summary>
    public class PlayerTokenCollision : Simulation.Event<PlayerTokenCollision>
    {
        public PlayerController player;
        public TokenInstance token;

        private PlayerDataManager dataManager;

        public override void Execute()
        {
            Debug.Log("🔥 Token tocado: " + token.name);

            // Si ya se recogió, no duplicar monedas
            if (token.collected) return;

            token.collected = true;

            // Sonido de recolección
            if (token.tokenCollectAudio != null)
                AudioSource.PlayClipAtPoint(token.tokenCollectAudio, token.transform.position);

            // Buscar PlayerDataManager automáticamente si no está asignado
            if (dataManager == null)
                dataManager = GameObject.FindFirstObjectByType<PlayerDataManager>();


            if (dataManager == null)
            {
                Debug.LogError("❌ NO se encontró PlayerDataManager en la escena. Colócalo en GameController.");
                return;
            }

            // Cargar progreso → sumar moneda → guardar de nuevo
            dataManager.LoadProgress((coins, lvl1, lvl2) =>
            {
                int newCoins = coins + 1;
                Debug.Log("🪙 Moneda recolectada. Total = " + newCoins);

                dataManager.SaveProgress(newCoins, lvl1, lvl2);
            });
        }
    }
}
