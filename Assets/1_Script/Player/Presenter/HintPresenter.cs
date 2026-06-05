using System;
using UnityEngine;

// 알림 전달자 (비상, 업적 등등)

public class HintPresenter : MonoBehaviour
{
    public event Action<HintData> OnHintReceived;

    private void OnEnable()
    {
        EventBus.Subscribe<HintEvent>(HandleHint);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<HintEvent>(HandleHint);
    }

    private void HandleHint(HintEvent evt)
    {
        OnHintReceived?.Invoke(new HintData(evt.Message, evt.Type));
    }
}
