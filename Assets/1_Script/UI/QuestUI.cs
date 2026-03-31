using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    public static QuestUI Instance;

    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private TMP_Text rewardText;
    [SerializeField] private Button claimButton;

    private QuesterNPC currentNPC;

    private void Awake()
    {
        Instance = this;
    }

    public void Open(QuesterNPC npc)
    {
        currentNPC = npc;

        panel.SetActive(true);
        Refresh();
    }

    public void Refresh()
    {
        if (!QuestManager.Instance.HasQuest)
            return;

        titleText.text = "Kill Enemies";

        progressText.text =
            $"{QuestManager.Instance.CurrentKillCount} / {QuestManager.Instance.RequiredKillCount} 처치";

        rewardText.text =
            $"보상: {QuestManager.Instance.RewardGold} 골드";

        claimButton.gameObject.SetActive(QuestManager.Instance.IsQuestComplete());
    }

    public void ClaimReward(PlayerStatus playerStatus)
    {
        QuestManager.Instance.ClaimReward(playerStatus);
    }

    public void Close()
    {
        panel.SetActive(false);
    }
}
