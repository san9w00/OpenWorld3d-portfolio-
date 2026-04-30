using System.Collections;
using UnityEngine;

public class TeleportSystem : MonoBehaviour
{
    public static TeleportSystem Instance;

    private InputHandler inputHandler;

    private bool isTeleporting = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        inputHandler = FindAnyObjectByType<InputHandler>();
    }

    public void Teleport(Vector3 targetPosition)
    {
        if (isTeleporting) return;

        StartCoroutine(TeleportRoutine(targetPosition));
    }

    IEnumerator TeleportRoutine(Vector3 targetPosition)
    {
        isTeleporting = true;

        inputHandler.SetInputEnabled(false);

        yield return FadeUI.Instance.FadeOut();

        PlayerManager.Instance.Teleport(targetPosition);

        yield return null;

        yield return FadeUI.Instance.FadeIn();

        inputHandler.SetInputEnabled(true);

        isTeleporting = false;
    }
}
