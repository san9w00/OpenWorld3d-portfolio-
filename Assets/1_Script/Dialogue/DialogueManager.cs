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

        InputHandler.Instance.SetInventoryState(true);

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

        InputHandler.Instance.SetInventoryState(false);
    }
}
