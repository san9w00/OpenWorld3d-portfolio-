using UnityEngine;
using UnityEngine.UI;

public class UIBarView : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    // 게이지 비율 설정
    public void SetFill(float normalized)
    {
        fillImage.fillAmount = Mathf.Clamp01(normalized);
    }
}
