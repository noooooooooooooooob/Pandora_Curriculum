using System.Collections;
using UnityEngine;

public class Player_Roll : MonoBehaviour
{
    public Player_Move player_Move;
    public Player_Attack player_Attack;
    public Player_Stamina_Controller staminaController;
    public Player_Animation player_Animation;
    public HP_Controller hpController;
    public float invincibilityDuration = 0.5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Roll();
        }
    }
    public void Roll()
    {
        if (!hpController.isInvincible)
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }
    IEnumerator InvincibilityCoroutine()
    {
        if (!staminaController.DecreaseStamina())
        {
            hpController.isInvincible = false;
            yield break;
        }

        hpController.isInvincible = true;
        player_Attack.canAttack = false;
        player_Move.canMove = false;
        player_Move.Roll(invincibilityDuration);
        player_Animation.Roll(invincibilityDuration);
        yield return new WaitForSeconds(invincibilityDuration);
        hpController.isInvincible = false;
        player_Attack.canAttack = true;
        player_Move.canMove = true;
    }
}
