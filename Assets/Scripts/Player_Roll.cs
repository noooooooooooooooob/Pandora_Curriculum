using System.Collections;
using UnityEngine;

public class Player_Roll : MonoBehaviour
{
    [Header("Refs")]
    public Player_Move player_Move;
    public Player_Attack player_Attack;
    public Player_Stamina_Controller staminaController;
    public Player_Animation player_Animation;
    public HP_Controller hpController;

    [Header("Roll")]
    public KeyCode rollKey = KeyCode.Space;
    public float rollDistance = 2.6f;      // 총 이동 거리(유닛)
    public float rollDuration = 0.28f;     // 롤 시간(초)
    public float rollCooldown = 0.5f;
    public LayerMask obstacleMask;         // 벽/장해물
    public float skin = 0.02f;             // 벽 여유거리(겹침 방지)

    [Header("I-Frames")]
    public float iFrameStart = 0.04f;
    public float iFrameEnd = 0.22f;

    float cdTimer;
    bool isRolling;
    Vector2 rollDir;
    float rollSpeed;
    float rollElapsed;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        cdTimer -= Time.deltaTime;
        if (Input.GetKeyDown(rollKey))
            TryRoll();
    }

    void FixedUpdate()
    {
        if (!isRolling) return;
        StepRoll(Time.fixedDeltaTime);
    }

    void TryRoll()
    {
        if (isRolling) return;
        if (cdTimer > 0f) return;

        if (staminaController != null && !staminaController.DecreaseStamina())
            return;

        rollDir = (player_Move != null && player_Move.LastMoveDir.sqrMagnitude > 0.0001f)
                    ? player_Move.LastMoveDir.normalized
                    : Vector2.down;

        rollSpeed = rollDistance / Mathf.Max(0.0001f, rollDuration);
        rollElapsed = 0f;

        StartCoroutine(RollRoutine());
    }

    IEnumerator RollRoutine()
    {
        isRolling = true;
        cdTimer = rollCooldown + rollDuration;

        if (player_Attack) player_Attack.canAttack = false;
        if (player_Move) player_Move.canMove = false;

        float t = 0f;
        while (t < rollDuration)
        {
            t += Time.deltaTime;

            bool iframe = (t >= iFrameStart && t <= iFrameEnd);
            if (hpController) hpController.isInvincible = iframe;

            spriteRenderer.color = iframe ? new Color(1f, 1f, 1f, 0.6f) : Color.white;
            yield return null;
        }

        if (hpController) hpController.isInvincible = false;
        if (player_Attack) player_Attack.canAttack = true;
        if (player_Move) player_Move.canMove = true;

        isRolling = false;
    }

    void StepRoll(float dt)
    {
        rollElapsed += dt;
        if (rollElapsed > rollDuration) dt -= (rollElapsed - rollDuration);

        float step = rollSpeed * dt;
        var rb = GetComponent<Rigidbody2D>();
        Vector2 pos = rb.position;

        // 앞에 벽 있는지 체크
        RaycastHit2D hit = Physics2D.Raycast(pos, rollDir, step + skin, obstacleMask);
        Vector2 target = hit
            ? pos + rollDir * Mathf.Max(0f, hit.distance - skin)
            : pos + rollDir * step;

        rb.MovePosition(target);
    }
}
