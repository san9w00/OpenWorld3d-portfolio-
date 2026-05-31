using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    private bool isDialogueOpen;

    private void Awake()
    {
        Instance = this;
    }

    public void Show(DialogueRequest request)
    {
        if (isDialogueOpen)
            return;

        isDialogueOpen = true;

        UIManager.Instance.OpenUI(DialogueUI.Instance.gameObject);

        WrapRequest(request);
    }

    private void WrapRequest(DialogueRequest request)
    {
        DialogueRequest wrappedRequest = new DialogueRequest
        {
            text = request.text,
            choices = request.choices,

            onContinue = () =>
            {
                request.onContinue?.Invoke();

                Close();
            }
        };

        if (wrappedRequest.choices != null)
        {
            foreach (DialogueChoice choice in wrappedRequest.choices)
            {
                System.Action originalAction = choice.action;

                choice.action = () =>
                {
                    originalAction?.Invoke();

                    Close();
                };
            }
        }

        DialogueUI.Instance.Show(wrappedRequest);
    }

    public void Close()
    {
        isDialogueOpen = false;

        DialogueUI.Instance.Hide();

        UIManager.Instance.CloseUI(DialogueUI.Instance.gameObject);
        SoundManager.Instance.PlaySFX(SFXType.UISmallClick);
    }
}
