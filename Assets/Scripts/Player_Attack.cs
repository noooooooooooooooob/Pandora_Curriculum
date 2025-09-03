using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    public bool canAttack = true;
    Camera cam;
    public Camera_Follow camera_Follow;
    public Player_Move player_Move;
    public GameObject attackDisplay;
    public GameObject attackPrefab;
    float isCharging = 0;
    public float maxCharge;
    public float maxAttackRange;
    float angle;

    void Start()
    {
        cam = Camera.main;
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
            DisplayAttack(maxAttackRange * isCharging);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isCharging = 0;
            player_Move.speed *= 2;
            attackDisplay.SetActive(false);
            camera_Follow.ResetCameraSize();
            // Attack();
        }
    }
    void DisplayAttack(float range)
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        float x = mousePos.x - transform.position.x;
        float y = mousePos.y - transform.position.y;
        angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        attackDisplay.transform.rotation = Quaternion.Euler(0, 0, angle);
        attackDisplay.GetComponent<SpriteRenderer>().size = new Vector2(range, 1);
    }
    void Attack()
    {
        
    }
}
