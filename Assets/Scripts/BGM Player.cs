using System;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    [SerializeField] String soundName;
    void Start()
    {
        AudioManager.Instance.PlaySound(soundName, 0, true, SoundType.BGM);
    }
}
