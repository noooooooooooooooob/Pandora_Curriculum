using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Player_Hard_Hp_Controller : MonoBehaviour
{
    public Camera_Follow camera_Follow;
    public bool isInvincible = false;
    public SpriteRenderer spriteRenderer;
    public float invincibleDuration = 1f;
    public UnityEvent onDie;
    public Slider hpBar;
    public int maxHP;
    int currentHP;
    void Start()
    {
        currentHP = maxHP;
    }
    public void Damage(int damageAmount)
    {
        if (isInvincible) return;
        currentHP -= damageAmount;
        if (currentHP <= 0)
        {
            currentHP = 0;
            hpBar.value = 1 - (float)currentHP / maxHP; // 반대로 하기
            onDie.Invoke();
            return;
        }
        camera_Follow.Shake(0.2f, 0.3f);
        StartCoroutine(InvincibleCoroutine());
        
        hpBar.value = 1 - (float)currentHP / maxHP; // 반대로 하기
    }
    IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;
        float elapsed = 0f;
        while (elapsed < invincibleDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.PingPong(elapsed * 5f, 1f);
            if (spriteRenderer != null)
            {
                Color c = spriteRenderer.color;
                c.a = alpha;
                spriteRenderer.color = c;
            }
            yield return null;
        }
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = 1f;
            spriteRenderer.color = c;
        }
        isInvincible = false;
    }
}
