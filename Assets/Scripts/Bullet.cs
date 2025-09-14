using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
