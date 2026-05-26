using UnityEngine;

public class BossTimelineController : MonoBehaviour
{
    [SerializeField] private string bossBGMName;

    public void StartBossBGM()
    {
        SoundManager.Instance.SetAutoBGMBlocked(false);
        SoundManager.Instance.PlayBGMByName(bossBGMName);
    }
}
