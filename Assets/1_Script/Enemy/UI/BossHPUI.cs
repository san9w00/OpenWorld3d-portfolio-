using UnityEngine;
using UnityEngine.UI;

public class BossHPUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    private EnemyStatus bossStatus;

    public void Init(EnemyStatus status)
    {
        bossStatus = status;

        bossStatus.OnHPChanged += UpdateHP;
        bossStatus.OnDeath += Hide;

        gameObject.SetActive(false);

        // 초기값 반영
        UpdateHP(status.Data.maxHP, status.Data.maxHP);
    }

    private void UpdateHP(float current, float max)
    {
        fillImage.fillAmount = current / max;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (bossStatus != null)
        {
            bossStatus.OnHPChanged -= UpdateHP;
            bossStatus.OnDeath -= Hide;
        }
    }
}
