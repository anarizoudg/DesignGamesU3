using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer.Gameplay
{
    public class PlayerShooting : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public Transform firePoint;
        public float bulletSpeed = 10f;

        private InputAction shootAction;

        void Awake()
        {
            shootAction = InputSystem.actions.FindAction("Player/Attack");

            if (shootAction == null)
                Debug.LogError("❌ No se encontró la acción Player/Attack");

            else
                shootAction.Enable();
        }

        void Update()
        {
            if (shootAction != null && shootAction.WasPressedThisFrame())
            {
                Debug.Log("🔫 Disparo detectado");
                Shoot();
            }
        }

        void Shoot()
        {
            if (bulletPrefab == null)
            {
                Debug.LogError("❌ BulletPrefab no asignado en el inspector");
                return;
            }

            if (firePoint == null)
            {
                Debug.LogError("❌ FirePoint no asignado en el inspector");
                return;
            }

            Debug.Log("✨ Instanciando bala en " + firePoint.position);

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            if (rb == null)
            {
                Debug.LogError("❌ La bala NO tiene Rigidbody2D");
                return;
            }

            rb.linearVelocity = firePoint.right * bulletSpeed;
            Debug.Log("➡ Velocidad de bala: " + rb.linearVelocity);
        }
    }
}
