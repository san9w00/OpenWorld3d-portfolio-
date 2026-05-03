using UnityEngine;
using UnityEngine.UI;

public class UIStaminaRingView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image fillImage;

    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Color")]
    [SerializeField] private Color normalColor = Color.green;

    [SerializeField] private Color exhaustedColor = Color.red;

    // =========================
    // 게이지 업데이트
    // =========================

    public void SetFill(float normalized)
    {
        fillImage.fillAmount = Mathf.Clamp01(normalized);
    }

    // =========================
    // UI 표시 / 숨김
    // =========================

    public void SetVisible(bool visible)
    {
        canvasGroup.alpha = visible ? 1 : 0;

        canvasGroup.blocksRaycasts = visible;
    }

    // =========================
    // 탈진 색상 변경
    // =========================

    public void SetExhausted(bool exhausted)
    {
        fillImage.color = exhausted ? exhaustedColor : normalColor;
    }
}
