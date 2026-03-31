using UnityEngine;
using UnityEngine.Playables;

public class TimeLineTrigger : MonoBehaviour
{
    public enum TriggerMode
    {
        PlayOnce,
        PlayEveryTime
    }

    [Header("TimeLine")]
    [SerializeField] private PlayableDirector playableDirector;

    [Header("Settings")]
    [SerializeField] private TriggerMode triggerMode = TriggerMode.PlayOnce;
    [SerializeField] private string playerTag = "Player";

    private bool hasPlayed = false;

    private void Reset()
    {
        BoxCollider box = GetComponent<BoxCollider>();
        box.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag))
            return;

        switch (triggerMode)
        {
            case TriggerMode.PlayOnce:
                if (hasPlayed)
                    return;

                PlayTimeLine();
                hasPlayed = true;
                break;

            case TriggerMode.PlayEveryTime:
                PlayTimeLine();
                break;
        }
    }

    private void PlayTimeLine()
    {
        if (playableDirector == null)
        {
            return;
        }

        playableDirector.time = 0;
        playableDirector.Evaluate();
        playableDirector.Play();
    }

    // (필요시 다른 스크립트에서 활성화 ㄱㄴ)
    public void ResetTrigger()
    {
        hasPlayed = false;
    }
}
