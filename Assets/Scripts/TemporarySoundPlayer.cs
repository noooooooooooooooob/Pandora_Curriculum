// TemporarySoundPlayer.cs
using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class TemporarySoundPlayer : MonoBehaviour
{
    public AudioSource Source { get; private set; }
    public string ClipName => Source && Source.clip ? Source.clip.name : string.Empty;
    public SoundType Type { get; private set; }
    public int Id { get; private set; }
    static int _seq;

    void Awake() => Source = GetComponent<AudioSource>();

    public void InitSound(AudioClip clip, SoundType type)
    {
        Type = type;
        Source.clip = clip;
        Id = ++_seq;
    }

    public void Play(AudioMixerGroup group, float delay, bool isLoop)
    {
        Source.outputAudioMixerGroup = group;
        Source.loop = isLoop;

        if (delay > 0f) Source.PlayDelayed(delay);
        else Source.Play();

        if (!isLoop) StartCoroutine(Co_AutoDispose());
    }

    IEnumerator Co_AutoDispose()
    {
        // 재생이 실제 끝날 때까지 대기 (Pause/Stop/피치 변화 반영)
        yield return new WaitWhile(() => Source != null && Source.isPlaying);
        Destroy(gameObject);
    }

    public void Pause(bool p)
    {
        if (!Source) return;
        if (p) Source.Pause(); else Source.UnPause();
    }

    public void StopAndDispose()
    {
        if (Source) Source.Stop();
        Destroy(gameObject);
    }
}
