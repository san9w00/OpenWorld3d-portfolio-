using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private TextMeshProUGUI upgradePointText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI defenseText;

    [SerializeField] private PlayerLevelSystem levelSystem;
    private int currentPoint;

    private int hpUpgradeCount = 0;
    private int damageUpgradeCount = 0;
    private int defenseUpgradeCount = 0;

    private void OnEnable()
    {
        EventBus.Subscribe<UpgradePointChangedEvent>(
            OnUpgradePointChanged);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<UpgradePointChangedEvent>(
            OnUpgradePointChanged);
    }

    private void Start()
    {
        upgradePanel.SetActive(false);

        hpText.text = "+0";
        damageText.text = "+0";
        defenseText.text = "+0";

        upgradePointText.text = "0";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleUpgradePanel();
        }
    }

    void ToggleUpgradePanel()
    {
        bool isOpen = !upgradePanel.activeSelf;
        upgradePanel.SetActive(isOpen);

        if (isOpen)
        {
            UIManager.Instance.OpenUI(upgradePanel);
        }
        else
        {
            UIManager.Instance.CloseUI(upgradePanel);
        }
    }

    private void OnUpgradePointChanged(UpgradePointChangedEvent evt)
    {
        currentPoint = evt.Point;

        upgradePointText.text =
            currentPoint.ToString();
    }

    public void UpgradeHP()
    {
        if (!levelSystem.UseUpgradePoint()) return;

        hpUpgradeCount++;
        PlayerStatus.Instance.IncreaseMaxHP(10);

        hpText.text = "+" + (hpUpgradeCount * 10);
        RefreshPointText();
    }
    public void UpgradeDamage()
    {
        if (!levelSystem.UseUpgradePoint()) return;

        damageUpgradeCount++;
        PlayerStatus.Instance.IncreaseAttack(5);

        damageText.text = "+" + (damageUpgradeCount * 5);

        RefreshPointText();
    }

    public void UpgradeDefense()
    {
        if (!levelSystem.UseUpgradePoint()) return;

        defenseUpgradeCount++;
        PlayerStatus.Instance.IncreaseDefense(3);

        defenseText.text = "+" + (defenseUpgradeCount * 3);

        RefreshPointText();
    }

    private void RefreshPointText()
    {
        upgradePointText.text = currentPoint.ToString();
    }

    public void CloseButton()
    {
        upgradePanel.SetActive(false);

        UIManager.Instance.CloseUI(upgradePanel);
    }
}
