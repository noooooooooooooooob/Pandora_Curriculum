using UnityEngine;

public class Player_Move : MonoBehaviour
{
    public Player_Run_Stamina_Controller stamina_Controller;
    public bool canMove = true;

    [Header("Speeds")]
    public float baseSpeed = 4f;
    public float runMultiplier = 2f;
    public float walkMultiplier = 0.5f;

    Rigidbody2D rb;
    Vector2 input;  // -1~1
    public Vector2 LastMoveDir { get; private set; } = Vector2.down;

    public bool isRunning;
    bool isWalking;

    // 공격 슬로우(기본 1.0f). 공격 중 0.5f처럼 세팅.
    float attackMultiplier = 1f;

    public Running_Effect running_Effect;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))  isRunning = true;
        if (Input.GetKeyUp(KeyCode.LeftShift))    isRunning = false;
        if (Input.GetKeyDown(KeyCode.LeftControl)) isWalking = true;
        if (Input.GetKeyUp(KeyCode.LeftControl))   isWalking = false;

        int x = (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ?  1 : 0);
        int y = (Input.GetKey(KeyCode.S) ? -1 : 0) + (Input.GetKey(KeyCode.W) ?  1 : 0);
        input = new Vector2(x, y).normalized;

        if (input.sqrMagnitude > 0.0001f)
            LastMoveDir = input;

        if (isRunning && input.sqrMagnitude > 0.0001f && running_Effect != null)
        {
            int idx = DirTo8(input);
            running_Effect.PlayEffect(idx);
        }
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        float moveMul = 1f;
        if (isRunning && stamina_Controller.canRun)      moveMul = runMultiplier;
        else if (isWalking) moveMul = walkMultiplier;

        float finalMul = moveMul * attackMultiplier;
        rb.velocity = input * baseSpeed * finalMul;
    }

    public void SetExternalVelocity(Vector2 v) => rb.velocity = v;

    public void SetAttackSlow(bool isSlowed)
    {
        attackMultiplier = isSlowed ? 0.5f : 1f;
    }

    int DirTo8(Vector2 dir)
    {
        float deg = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (deg < 0) deg += 360f;
        int sector = Mathf.RoundToInt(deg / 45f) % 8;
        int[] map = { 6, 7, 4, 3, 2, 1, 0, 5 };
        return map[sector];
    }
}
