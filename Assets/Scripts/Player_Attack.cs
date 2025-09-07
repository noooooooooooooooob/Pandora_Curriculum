using DG.Tweening;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    public bool canAttack = true;
    Camera cam;
    public Camera_Follow camera_Follow;
    public Player_Move player_Move;
    public Player_Animation player_Animation;
    public GameObject attackDisplay;
    public GameObject attackPrefab;
    float isCharging = 0;
    public float maxCharge;
    public float minAttackRange;
    public float maxAttackRange;
    public float maxAttackDisplayWidth; // 늘리기용
    float angle;

    void Start()
    {
        cam = Camera.main;
        attackDisplay.SetActive(false);
    }
    void Update()
    {
        if (!canAttack) return;
        if (Input.GetMouseButtonDown(0))
        {
            player_Move.speed /= 2;
            attackDisplay.SetActive(true);
        }
        else if (Input.GetMouseButton(0))
        {
            isCharging += Time.deltaTime;
            isCharging = Mathf.Clamp(isCharging, 0, maxCharge);
            camera_Follow.PlusCameraSize(isCharging);
            DisplayAttack(isCharging / maxCharge * maxAttackDisplayWidth);
            player_Animation.UpdateBow(isCharging / maxCharge);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            float chargeRatio = isCharging / maxCharge;

            float range = Mathf.Max(minAttackRange, chargeRatio * maxAttackRange);

            Attack(range);

            isCharging = 0;
            player_Move.speed *= 2;
            attackDisplay.SetActive(false);
            camera_Follow.ResetCameraSize();
            player_Animation.UpdateBow(0);
        }
    }
    void DisplayAttack(float range)
    {
        angle = Player_Method.GetMouseToRotation(transform).z;
        attackDisplay.transform.rotation = Quaternion.Euler(0, 0, angle);
        attackDisplay.GetComponent<SpriteRenderer>().size = new Vector2(range, 1);
    }
    void Attack(float range)
    {
        GameObject attack = Instantiate(attackPrefab, transform.position, Quaternion.Euler(0, 0, angle));
        attack.transform.DOMove(
            new Vector2(
                transform.position.x + Mathf.Cos(angle * Mathf.Deg2Rad) * range,
                transform.position.y + Mathf.Sin(angle * Mathf.Deg2Rad) * range),
            1.0f
        ).SetLink(gameObject, LinkBehaviour.KillOnDestroy).OnComplete(() => Destroy(attack));
    }
}
