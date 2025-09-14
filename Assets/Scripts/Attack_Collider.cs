using UnityEngine;

public class Attack_Collider : MonoBehaviour
{
    public int damage = 10;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HP_Controller player = collision.GetComponent<HP_Controller>();
            if (player != null)
            {
                player.Damage(damage);
            }
        }
    }
}
