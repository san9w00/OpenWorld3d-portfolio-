using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button continueButton;

    private Action onFinish;

    private void Awake()
    {
        panel.SetActive(false);
        continueButton.onClick.AddListener(FinishDialogue);
    }

    public void Show(string text, Action finishCallback)
    {
        panel.SetActive(true);

        dialogueText.text = text;
        onFinish = finishCallback;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void FinishDialogue()
    {
        panel.SetActive(false);

        onFinish?.Invoke();
        onFinish = null;
    }
}
