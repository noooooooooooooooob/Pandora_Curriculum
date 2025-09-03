using UnityEngine;

public class Player_Move : MonoBehaviour
{
    public bool canMove = true;
    bool isRunning = false;
    public float speed;
    Rigidbody2D rb;
    Vector2 dir;
    Vector2 playerDirection;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }
    }
    void FixedUpdate()
    {
        dir = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            dir.x = -1;
            playerDirection = Vector2.zero;
            playerDirection = dir;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
            playerDirection = Vector2.zero;
            playerDirection = dir;
        }
        if (Input.GetKey(KeyCode.W))
        {
            dir.y = 1;
            playerDirection = Vector2.zero;
            playerDirection = dir;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dir.y = -1;
            playerDirection = Vector2.zero;
            playerDirection = dir;
        }
        if (!canMove) return;

        dir.Normalize();
        if(isRunning) dir *= 2;
        rb.linearVelocity = speed * dir;
    }
    float rollPower = 10f;
    public void Roll(float time)
    {
        rb.AddForce(rollPower * playerDirection, ForceMode2D.Impulse);
    }
}
