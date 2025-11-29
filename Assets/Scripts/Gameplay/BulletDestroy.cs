using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Destruye la bala cuando toca cualquier cosa
        Destroy(gameObject);
    }
}
