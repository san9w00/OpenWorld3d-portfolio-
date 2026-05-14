using System;
using System.Collections;
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
    [SerializeField] private Transform portraitRoot;
    private GameObject currentPortrait;

    [Header("Dialogue")]
    [SerializeField] private TMP_Text dialogueText;

    [Header("Choice")]
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private Transform choiceParent;
    [SerializeField] private Button choiceButtonPrefab;

    [Header("Continue")]
    [SerializeField] private Button continueButton;

    [Header("Fade")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 0.3f;

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

        StopAllCoroutines();
        StartCoroutine(FadeIn());

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

    public void SetPortrait(GameObject portraitPrefab)
    {
        ClearPortrait();

        if (portraitPrefab == null)
        {
            portraitRoot.gameObject.SetActive(false);
            return;
        }

        portraitRoot.gameObject.SetActive(true);

        currentPortrait =
            Instantiate(
                portraitPrefab,
                portraitRoot);
    }

    private void ClearPortrait()
    {
        if (currentPortrait != null)
        {
            Destroy(currentPortrait);
        }
    }

    public void Hide()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());

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

    private IEnumerator FadeIn()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        canvasGroup.alpha = 0f;

        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            canvasGroup.alpha =
                Mathf.Lerp(
                    0f,
                    1f,
                    time / fadeDuration);

            yield return null;
        }

        canvasGroup.alpha = 1f;

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private IEnumerator FadeOut()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            canvasGroup.alpha =
                Mathf.Lerp(
                    1f,
                    0f,
                    time / fadeDuration);

            yield return null;
        }

        canvasGroup.alpha = 0f;

        panel.SetActive(false);

        ClearPortrait();

        portraitRoot.gameObject.SetActive(false);
    }
}
