using System;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text dialogueText;

    [SerializeField] private GameObject choicePanel;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    [SerializeField] private Button continueButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        panel.SetActive(false);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void ShowQuestOffer(string text, Action onYes, Action onNo)
    {
        panel.SetActive(true);
        InputHandler.Instance.SetInventoryState(true);

        dialogueText.text = text;

        choicePanel.SetActive(true);
        continueButton.gameObject.SetActive(false);

        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(() =>
        {
            panel.SetActive(false);
            InputHandler.Instance.SetInventoryState(false);
            onYes?.Invoke();
        });

        noButton.onClick.AddListener(() =>
        {
            panel.SetActive(false);
            InputHandler.Instance.SetInventoryState(false);
            onNo?.Invoke();
        });
    }

    public void ShowSimple(string text, Action onContinue)
    {
        panel.SetActive(true);
        InputHandler.Instance.SetInventoryState(true);

        dialogueText.text = text;

        choicePanel.SetActive(false);
        continueButton.gameObject.SetActive(true);

        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() =>
        {
            panel.SetActive(false);
            InputHandler.Instance.SetInventoryState(false);
            onContinue?.Invoke();
        });
    }
}
