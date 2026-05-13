using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    [Header("Panel")]
    [SerializeField] private GameObject panel;

    [Header("Portrait")]
    [SerializeField] private GameObject portraitRoot;
    [SerializeField] private Image portraitImage; // 초상화이미지

    [Header("Dialogue")]
    [SerializeField] private TMP_Text dialogueText;

    [Header("Choice")]
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private Transform choiceParent;
    [SerializeField] private Button choiceButtonPrefab;

    [Header("Continue")]
    [SerializeField] private Button continueButton;

    private readonly List<Button> spawnedButtons = new();

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

    public void Show(DialogueRequest request)
    {
        panel.SetActive(true);

        dialogueText.text = request.text;

        ClearChoices();

        bool hasChoices =
            request.choices != null &&
            request.choices.Count > 0;

        choicePanel.SetActive(hasChoices);
        continueButton.gameObject.SetActive(!hasChoices);

        if (hasChoices)
        {
            CreateChoiceButtons(request.choices);
        }
        else
        {
            SetupContinueButton(request.onContinue);
        }
    }

    private void CreateChoiceButtons(List<DialogueChoice> choices)
    {
        foreach (DialogueChoice choice in choices)
        {
            Button button =
                Instantiate(choiceButtonPrefab, choiceParent);

            TMP_Text buttonText =
                button.GetComponentInChildren<TMP_Text>();

            buttonText.text = choice.buttonText;

            button.onClick.AddListener(() =>
            {
                choice.action?.Invoke();
            });

            spawnedButtons.Add(button);
        }
    }

    private void SetupContinueButton(System.Action onContinue)
    {
        continueButton.onClick.RemoveAllListeners();

        continueButton.onClick.AddListener(() =>
        {
            onContinue?.Invoke();
        });
    }

    public void SetPortrait(Sprite sprite)
    {
        if (sprite == null)
        {
            portraitRoot.SetActive(false);
            return;
        }

        portraitRoot.SetActive(true);

        portraitImage.sprite = sprite;
    }

    public void Hide()
    {
        panel.SetActive(false);
        portraitRoot.SetActive(false);

        ClearChoices();
    }

    private void ClearChoices()
    {
        foreach (Button button in spawnedButtons)
        {
            Destroy(button.gameObject);
        }

        spawnedButtons.Clear();
    }
}
