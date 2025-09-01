using System.Collections;
using UnityEngine;

public class Player_Roll : MonoBehaviour
{
    public Player_Move player_Move;
    public Player_Attack player_Attack;
    public Player_Stamina_Controller staminaController;
    public Player_Animation player_Animation;
    public float invincibilityDuration = 0.5f;
    bool isInvincible = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Roll();
        }
    }
    public void Roll()
    {
        if (!isInvincible)
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }
    IEnumerator InvincibilityCoroutine()
    {
        if (!staminaController.DecreaseStamina())
        {
            isInvincible = false;
            yield break;
        }

        isInvincible = true;
        player_Attack.canAttack = false;
        player_Move.canMove = false;
        player_Move.Roll(invincibilityDuration);
        player_Animation.Roll(invincibilityDuration);
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
        player_Attack.canAttack = true;
        player_Move.canMove = true;
    }
}
