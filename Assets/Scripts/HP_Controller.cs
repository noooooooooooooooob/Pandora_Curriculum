using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HP_Controller : MonoBehaviour
{
    public UnityEvent onDie;
    public Slider hpBar;
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
    }
}