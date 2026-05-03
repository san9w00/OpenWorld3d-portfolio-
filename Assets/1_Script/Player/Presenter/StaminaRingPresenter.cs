using System.Collections;
using UnityEngine;

public class StaminaRingPresenter : MonoBehaviour
{
    [SerializeField] private UIStaminaRingView view;

    // UI 자동 숨김 시간
    [SerializeField] private float hideDelay = 1f;

    private Coroutine hideCoroutine;

    private PlayerStatus playerStatus;

    private void Start()
    {
        playerStatus = PlayerStatus.Instance;

        if (playerStatus == null)
        {
            Debug.LogError("PlayerStatus Instance 없음!");
            return;
        }

        // 이벤트 연결
        playerStatus.OnStaminaChanged += UpdateStamina;

        playerStatus.OnStaminaVisibleChanged += ShowUI;

        playerStatus.OnExhaustedChanged += SetExhausted;
    }

    private void OnDestroy()
    {
        if (playerStatus != null)
        {
            playerStatus.OnStaminaChanged -= UpdateStamina;

            playerStatus.OnStaminaVisibleChanged -= ShowUI;

            playerStatus.OnExhaustedChanged -= SetExhausted;
        }
    }

    // =========================
    // 스태미나 갱신
    // =========================

    private void UpdateStamina(StatData data)
    {
        view.SetFill(data.Normalized);

        RestartHideTimer();
    }

    // =========================
    // UI 표시
    // =========================

    private void ShowUI(bool visible)
    {
        if (visible)
        {
            view.SetVisible(true);

            RestartHideTimer();
        }
    }

    // =========================
    // 탈진 상태
    // =========================

    private void SetExhausted(bool exhausted)
    {
        view.SetExhausted(exhausted);

        // 탈진 상태면 강제로 UI 표시
        if (exhausted)
        {
            view.SetVisible(true);
        }
    }

    // =========================
    // 자동 숨김 타이머
    // =========================

    private void RestartHideTimer()
    {
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        hideCoroutine = StartCoroutine(HideCoroutine());
    }

    private IEnumerator HideCoroutine()
    {
        yield return new WaitForSeconds(hideDelay);

        // 탈진 상태 아닐 때만 숨김
        if (!playerStatus.IsExhausted)
        {
            view.SetVisible(false);
        }
    }
}
