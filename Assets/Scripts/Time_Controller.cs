using DG.Tweening;
using UnityEngine;

public class Time_Controller : MonoBehaviour
{
    public static Time_Controller instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }
    public void EndGame()
    {
        DOTween.KillAll(true);
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }
    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }
}
