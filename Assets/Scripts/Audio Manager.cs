using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public enum SoundType { BGM, SFX, }

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioClip[] preloadClips;

    private readonly Dictionary<string, AudioClip> clipsDictionary = new();
    private readonly List<TemporarySoundPlayer> activePlayers = new();   // 모든 재생 추적
    private readonly List<TemporarySoundPlayer> loopPlayers = new();   // 루프만 별도

    [Header("UI Sliders")]
    public Slider masterSlider, bgmSlider, sfxSlider;

    private float currentMasterVolume, currentBGMVolume, currentEffectVolume;

    // ★ 안전한 싱글턴
    public static AudioManager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        clipsDictionary.Clear();
        foreach (var clip in preloadClips)
        {
            if (!clip) continue;
            if (!clipsDictionary.ContainsKey(clip.name))
                clipsDictionary.Add(clip.name, clip);
        }
    }

    void Start()
    {
        if (masterSlider)
        {
            masterSlider.value = Mathf.Pow(10f, currentMasterVolume / 20f);
            masterSlider.onValueChanged.AddListener(sliderMasterValueChanged);
        }
        if (bgmSlider)
        {
            bgmSlider.value = Mathf.Pow(10f, currentBGMVolume / 20f);
            bgmSlider.onValueChanged.AddListener(sliderBGMValueChanged);
        }
        if (sfxSlider)
        {
            sfxSlider.value = Mathf.Pow(10f, currentEffectVolume / 20f);
            sfxSlider.onValueChanged.AddListener(sliderSFXValueChanged);
        }
    }

    AudioClip GetClip(string clipName)
    {
        if (string.IsNullOrEmpty(clipName)) { Debug.LogError("clipName is null"); return null; }
        if (!clipsDictionary.TryGetValue(clipName, out var clip) || clip == null)
        {
            Debug.LogError($"{clipName} 클립을 찾을 수 없습니다.");
            return null;
        }
        return clip;
    }

    // ▶ 재생: 반환값으로 "핸들(ID)"을 돌려줌 → 특정 인스턴스만 제어 가능
    public int PlaySound(string clipName, float delay = 0f, bool isLoop = false, SoundType type = SoundType.SFX)
    {
        var clip = GetClip(clipName);
        if (!clip) return -1;

        var obj = new GameObject($"TemporarySoundPlayer_{clipName}");
        var sp = obj.AddComponent<TemporarySoundPlayer>();

        sp.InitSound(clip, type);
        var group = audioMixer.FindMatchingGroups(type.ToString());
        sp.Play(group.Length > 0 ? group[0] : null, delay, isLoop);

        activePlayers.Add(sp);
        if (isLoop) loopPlayers.Add(sp);

        // 원샷은 사라질 때 목록 정리
        if (!isLoop) StartCoroutine(Co_AutoRemoveWhenDestroyed(sp));

        return sp.Id;
    }

    IEnumerator Co_AutoRemoveWhenDestroyed(TemporarySoundPlayer sp)
    {
        // 오브젝트가 파괴될 때까지 대기 후 목록 정리
        yield return new WaitUntil(() => sp == null);
        activePlayers.Remove(sp); // null이면 Remove는 그냥 무시됨
    }

    // 이름으로 "모든 인스턴스" 정지
    public void StopByClip(string clipName)
    {
        for (int i = activePlayers.Count - 1; i >= 0; i--)
        {
            var p = activePlayers[i];
            if (p != null && p.ClipName == clipName)
            {
                activePlayers.RemoveAt(i);
                loopPlayers.Remove(p);
                p.StopAndDispose();
            }
        }
    }

    // 특정 "ID 한 개"만 정지
    public void StopById(int id)
    {
        for (int i = activePlayers.Count - 1; i >= 0; i--)
        {
            var p = activePlayers[i];
            if (p != null && p.Id == id)
            {
                activePlayers.RemoveAt(i);
                loopPlayers.Remove(p);
                p.StopAndDispose();
                return;
            }
        }
    }

    // 타입(BGM/SFX)별 일시정지/재개
    public void PauseByType(SoundType type, bool pause)
    {
        foreach (var p in activePlayers)
            if (p != null && p.Type == type) p.Pause(pause);
    }

    // 이름으로 일시정지/재개 (동일 클립 여러 개면 전부)
    public void PauseByClip(string clipName, bool pause)
    {
        foreach (var p in activePlayers)
            if (p != null && p.ClipName == clipName) p.Pause(pause);
    }

    // 기존 루프 정지 API 안전화
    public void StopLoopSound(string clipName)
    {
        for (int i = loopPlayers.Count - 1; i >= 0; i--)
        {
            var p = loopPlayers[i];
            if (p != null && p.ClipName == clipName)
            {
                loopPlayers.RemoveAt(i);
                activePlayers.Remove(p);
                p.StopAndDispose();
                return;
            }
        }
        Debug.LogWarning($"{clipName} 루프를 찾을 수 없습니다.");
    }

    public void InitVolumes(float bgm, float effect)
    {
        SetVolume(SoundType.BGM, bgm);
        SetVolume(SoundType.SFX, effect);
    }

    public void SetVolume(SoundType type, float value)
    {
        audioMixer.SetFloat(type.ToString(), value);
    }

    public void sliderMasterValueChanged(float value)
    {
        float v = Mathf.Clamp(value, 0.0001f, 1f);
        float dB = Mathf.Log10(v) * 20f;
        audioMixer.SetFloat("Master", dB);
        currentMasterVolume = dB;
    }
    public void sliderBGMValueChanged(float value)
    {
        float v = Mathf.Clamp(value, 0.0001f, 1f);
        float dB = Mathf.Log10(v) * 20f;
        SetVolume(SoundType.BGM, dB);
        currentBGMVolume = dB;
    }
    public void sliderSFXValueChanged(float value)
    {
        float v = Mathf.Clamp(value, 0.0001f, 1f);
        float dB = Mathf.Log10(v) * 20f;
        SetVolume(SoundType.SFX, dB);
        currentEffectVolume = dB;
    }

    // 일시정지 메뉴 연동은 AudioListener.pause로 전역 일시정지 가능
    public GameObject pauseMenu;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool willOpen = !pauseMenu.activeSelf;
            pauseMenu.SetActive(willOpen);

            if (willOpen) Time_Controller.instance.PauseGame();
            else Time_Controller.instance.ResumeGame();

        }
    }
}
