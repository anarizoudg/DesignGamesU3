using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Evitar destruir la bala si toca al jugador
        if (collision.CompareTag("Player"))
            return;

        // Se destruye solo si toca enemigos o el suelo
        if (collision.CompareTag("Enemy") || collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
