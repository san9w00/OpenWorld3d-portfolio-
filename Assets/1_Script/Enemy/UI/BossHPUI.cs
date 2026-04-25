using UnityEngine;
using UnityEngine.UI;

public class BossHPUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private GameObject hpBarRoot; // 실제 보이는 UI

    private EnemyStatus bossStatus;

    public void Init(EnemyStatus status)
    {
        bossStatus = status;

        bossStatus.OnHPChanged += UpdateHP;
        bossStatus.OnDeath += Hide;

        hpBarRoot.SetActive(false);

        // 초기값 반영
        UpdateHP(status.Data.maxHP, status.Data.maxHP);
    }

    private void UpdateHP(float current, float max)
    {
        fillImage.fillAmount = current / max;
    }

    public void Show()
    {
        Debug.Log("Show 호출됨");
        hpBarRoot.SetActive(true);
    }

    private void Hide()
    {
        hpBarRoot.SetActive(false);
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
