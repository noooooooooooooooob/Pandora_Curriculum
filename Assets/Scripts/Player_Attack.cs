using System;
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
    public float minChargeSecond = 0.2f;
    public float minAttackRange;
    public float maxAttackRange;
    public float maxAttackDisplayWidth; // 늘리기용
    float angle;
    [SerializeField] String attackSoundName;
    [SerializeField] String bulletSoundName;
    int id;

    public int maxDamage = 50;

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
            player_Move.SetAttackSlow(true);
            attackDisplay.SetActive(true);
            id = AudioManager.Instance.PlaySound(attackSoundName);
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
            AudioManager.Instance.StopById(id);
            if (isCharging < minChargeSecond)
            {
                isCharging = 0;
                player_Move.SetAttackSlow(false);
                attackDisplay.SetActive(false);
                camera_Follow.ResetCameraSize();
                player_Animation.UpdateBow(0);
                return;
            }
            AudioManager.Instance.PlaySound(bulletSoundName);
            float chargeRatio = isCharging / maxCharge;

            float range = Mathf.Max(minAttackRange, chargeRatio * maxAttackRange);

            Attack(range);

            isCharging = 0;
            player_Move.SetAttackSlow(false);
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

        attack.GetComponent<Bullet>().damage =
            Mathf.RoundToInt(Mathf.Lerp(10, maxDamage, range / maxAttackRange));

        Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
        Vector2 targetPos = (Vector2)transform.position + dir * range;

        attack.transform.DOMove(targetPos, 1.0f)
            .SetEase(Ease.Linear)
            .SetLink(attack, LinkBehaviour.KillOnDisable | LinkBehaviour.KillOnDestroy)
            .OnComplete(() => { if (attack != null) Destroy(attack); });
    }
}
