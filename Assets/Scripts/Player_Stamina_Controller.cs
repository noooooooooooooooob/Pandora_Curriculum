using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class Player_Stamina_Controller : MonoBehaviour
{
    public Image staminaImage;
    public List<Sprite> staminaSprites;
    int maxStamina = 5;
    int currentStamina;
    [SerializeField]
    float recoveryTime = 2f;

    void Start()
    {
        currentStamina = maxStamina;
    }
    public bool DecreaseStamina()
    {
        if (currentStamina <= 0)
            return false;
        currentStamina--;
        UpdateStaminaUI();
        StopAllCoroutines();
        StartCoroutine(StaminaRecoveryCoroutine());
        return true;
    }
    public void IncreaseStamina()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina++;
            UpdateStaminaUI();
        }
    }
    IEnumerator StaminaRecoveryCoroutine()
    {
        while (currentStamina < maxStamina)
        {
            yield return new WaitForSeconds(recoveryTime);
            IncreaseStamina();
        }
    }
    void UpdateStaminaUI()
    {
        staminaImage.sprite = staminaSprites[currentStamina];
    }
}
