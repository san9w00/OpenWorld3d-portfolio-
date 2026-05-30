using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Data.SqlTypes;

public enum SFXType
{
    FootStep, Jump, Hit, Rolling,
    DoorOpen, DoorClose, QuestUnLcok, QuestComplete, CampfireUnlock,
    AddItem, OriginSkill, IcerSkill, FireSkill, PeacerSkill, 

}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("BGM")]
    private AudioSource bgmSource;
    private bool blockAutoBGM = false;

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

        // BGM
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;

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
        source.PlayOneShot(sfxDict[type]);
    }

    // SFX 재생 (AudioClip 기반)
    public void PlaySFX(AudioClip clip)
    {
        if (clip == null)
            return;

        AudioSource source = GetAvailableSFXSource();
        source.PlayOneShot(clip);
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
}

[System.Serializable]
public class SFXData
{
    public SFXType type;
    public AudioClip clip;
}
