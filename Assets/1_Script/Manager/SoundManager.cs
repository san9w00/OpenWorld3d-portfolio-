using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public enum SFXType
{
    FootStep, Jump, Hit, Rolling,
    DoorOpen, DoorClose, QuestUnLcok, QuestComplete, CampfireUnlock,
    AddItem, OriginSkill, IcerSkill, FireSkill, PeacerSkill, UIClick, UISmallClick, 
    DialogueType, InventorySFX, MapSFX, LevelUP,
    BossWalk, BossAttack, BossBite, BossFireBreath, BossNoise,
    AchievementUnlock,

}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("BGM")]
    private AudioSource bgmSource;
    private bool blockAutoBGM = false;

    [Header("Volume")]
    [SerializeField] private float masterVolume = 1f;
    [SerializeField] private float sfxVolume = 1f;

    [Header("SFX")]
    [SerializeField] private int poolSize = 10;
    private List<AudioSource> sfxSources = new List<AudioSource>();

    [SerializeField] private List<SFXData> sfxList;
    private Dictionary<SFXType, AudioClip> sfxDict;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 저장된 볼륨 불러오기
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        // 전체 오브젝트 사운드까지 제어
        AudioListener.volume = masterVolume;

        // BGM
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.volume = masterVolume;

        // SFX 풀 생성
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            sfxSources.Add(source);
        }

        // 딕셔너리 반환
        sfxDict = new Dictionary<SFXType, AudioClip>();
        foreach (var sfx in sfxList)
        {
            sfxDict[sfx.type] = sfx.clip;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (blockAutoBGM)
            return;

        PlayBGM(scene.name);
    }

    public void SetAutoBGMBlocked(bool blocked)
    {
        blockAutoBGM = blocked;
    }

    public void PlayBGMByName(string bgmName)
    {
        PlayBGM(bgmName);
    }

    private void PlayBGM(string sceneName)
    {
        AudioClip clip = Resources.Load<AudioClip>($"BGM/{sceneName}");

        if (clip == null)
        {
            Debug.LogWarning($"BGM not found : {sceneName}");
            return;
        }

        bgmSource.clip = clip;

        bgmSource.volume = masterVolume;

        bgmSource.Play();
    }

    // SFX 재생 (enum 기반)
    public void PlaySFX(SFXType type)
    {
        if (!sfxDict.ContainsKey(type))
        {
            Debug.LogWarning($"SFX 찾지 못함: {type}");
            return;
        }

        AudioSource source = GetAvailableSFXSource();

        // Master * SFX 적용
        source.PlayOneShot(sfxDict[type], masterVolume * sfxVolume);
    }

    // SFX 재생 (AudioClip 기반)
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
            return;

        AudioSource source = GetAvailableSFXSource();

        // Master * SFX 적용
        source.PlayOneShot(clip, masterVolume * sfxVolume);
    }

    public void SetSFXVolume(float volume) // SFX 볼륨 조절
    {
        sfxVolume = volume;

        // 저장
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void SetMasterVolume(float volume) // Master 볼륨 조정
    {
        masterVolume = volume;


        AudioListener.volume = masterVolume;

        // BGM도 같이 변경
        bgmSource.volume = masterVolume;

        // 저장
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    private AudioSource GetAvailableSFXSource()
    {
        foreach (var source in sfxSources)
        {
            if (!source.isPlaying)
                return source;
        }

        return sfxSources[0]; // 부족하면 덮어쓰기
    }


    // 슬라이더 초기화용
    public float MasterVolume => masterVolume;
    public float SFXVolume => sfxVolume;
}

[System.Serializable]
public class SFXData
{
    public SFXType type;
    public AudioClip clip;
}
