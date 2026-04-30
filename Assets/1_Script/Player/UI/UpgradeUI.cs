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

    private int hpUpgradeCount = 0;
    private int damageUpgradeCount = 0;
    private int defenseUpgradeCount = 0;

    private PlayerLevelSystem levelSystem;
    private PlayerStatus status;

    private void OnLevelStatChanged(StatType type, float current, float max)
    {
        if (type == StatType.Level)
        {
            RefreshPointText();
        }
    }

    private void Start()
    {
        levelSystem = PlayerLevelSystem.Instance;
        status = PlayerStatus.Instance;

        if (levelSystem != null)
        {
            levelSystem.OnLevelStatChanged += OnLevelStatChanged;
        }

        upgradePanel.SetActive(false);

        hpText.text = "+0";
        damageText.text = "+0";
        defenseText.text = "+0";

        RefreshPointText();
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
        InputHandler.Instance?.SetInventoryState(isOpen);
    }

    public void UpgradeHP()
    {
        if (!levelSystem.UseUpgradePoint()) return;

        hpUpgradeCount++;
        status.IncreaseMaxHP(10);

        hpText.text = "+" + (hpUpgradeCount * 10);
        RefreshPointText();
    }
    public void UpgradeDamage()
    {
        if (!levelSystem.UseUpgradePoint()) return;

        damageUpgradeCount++;
        status.IncreaseAttack(5);

        damageText.text = "+" + (damageUpgradeCount * 5);

        RefreshPointText();
    }

    public void UpgradeDefense()
    {
        if (!levelSystem.UseUpgradePoint()) return;

        defenseUpgradeCount++;
        status.IncreaseDefense(3);

        defenseText.text = "+" + (defenseUpgradeCount * 3);

        RefreshPointText();
    }

    private void RefreshPointText()
    {
        upgradePointText.text = levelSystem.UpgradePoint.ToString();
    }

    public void CloseButton()
    {
        upgradePanel.SetActive(false);
        InputHandler.Instance?.SetInventoryState(false);
    }

    private void OnDestroy()
    {
        if (levelSystem != null)
        {
            levelSystem.OnLevelStatChanged -= OnLevelStatChanged;
        }
    }
}
