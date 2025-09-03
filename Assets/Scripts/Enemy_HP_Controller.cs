using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Enemy_HP_Controller : MonoBehaviour
{
    public UnityEvent onDie;
    public Slider hpBar;
    public TextMeshProUGUI hpText;
    public int maxHP;
    int currentHP;
    public void Damage(int damageAmount)
    {
        currentHP -= damageAmount;
        if (currentHP < 0)
        {
            currentHP = 0;
            onDie.Invoke();
        }
        hpBar.value = 1 - (float)currentHP / maxHP; // 반대로 하기
        hpText.text = $"{((float)currentHP / maxHP * 100f).ToString("F2")} %";
    }
    void Start()
    {
        currentHP = maxHP;
        hpBar.value = 0;
        hpText.text = "100 %";
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Damage(10);
            DOTween.CompleteAll();
            Destroy(collision.gameObject);
        }
    }
}
