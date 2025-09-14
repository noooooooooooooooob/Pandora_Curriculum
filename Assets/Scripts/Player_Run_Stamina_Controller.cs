using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class Player_Run_Stamina_Controller : MonoBehaviour
{
    public Player_Move player_Move;
    public Slider spBar;
    public bool canRun = false;
    bool exhausted = false;
    public float exhaustedTime = 2f;
    public int maxSP;
    float currentSP;
    public float drainPerSecond;
    public float regenPerSecond;
    void Start()
    {
        currentSP = maxSP;
        UpdateStaminaBar();
    }
    void Update()
    {
        if (exhausted) return;
        if (player_Move.isRunning && currentSP > 0f)
        {
            currentSP -= drainPerSecond * Time.deltaTime;
        }
        else if (!player_Move.isRunning && currentSP < maxSP)
        {
            currentSP += regenPerSecond * Time.deltaTime;
        }

        currentSP = Mathf.Clamp(currentSP, 0f, maxSP);

        canRun = currentSP >= 0.001f;

        if (currentSP < 0.001f && !exhausted)
        {
            StartCoroutine(ExhaustedCoroutine());
            return;
        }

        if (!canRun && player_Move.isRunning)
            player_Move.isRunning = false;

        UpdateStaminaBar();
    }
    void UpdateStaminaBar()
    {
        spBar.value = 1 - (float)currentSP / maxSP; // 반대로 하기
    }
    IEnumerator ExhaustedCoroutine()
    {
        exhausted = true;
        yield return new WaitForSeconds(exhaustedTime);
        exhausted = false;
    }
}
