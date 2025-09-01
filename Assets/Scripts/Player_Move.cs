using UnityEngine;

public class Player_Move : MonoBehaviour
{
    public bool canMove = true;
    public float speed;
    Rigidbody2D rb;
    Vector2 dir;
    Vector2 playerDirection;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        dir = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            dir.x = -1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
        }

        if (Input.GetKey(KeyCode.W))
        {
            dir.y = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dir.y = -1;
        }
        if (!canMove) return;

        dir.Normalize();
        playerDirection = dir;
        rb.linearVelocity = speed * dir;
    }
    float rollPower = 10f;
    public void Roll(float time)
    {
        rb.AddForce(rollPower * playerDirection, ForceMode2D.Impulse);
    }
}
