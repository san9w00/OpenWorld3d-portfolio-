using System.Collections;
using UnityEngine;

public class TeleportSystem : MonoBehaviour
{
    public static TeleportSystem Instance;

    private Transform playerTransform;
    private InputHandler inputHandler;

    private bool isTeleporting = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        inputHandler = FindAnyObjectByType<InputHandler>();
    }

    public void Teleport(Vector3 targetPosition)
    {
        if (isTeleporting) return;

        StartCoroutine(TeleportRoutine(targetPosition));
    }

    IEnumerator TeleportRoutine(Vector3 targetPosition)
    {
        inputHandler.SetInputEnabled(false);

        yield return FadeUI.Instance.FadeOut();

        var controller = playerTransform.GetComponent<CharacterController>();
        if (controller != null)
            controller.enabled = false;

        playerTransform.position = targetPosition;

        yield return null;

        if (controller != null)
            controller.enabled = true;

        yield return FadeUI.Instance.FadeIn();

        inputHandler.SetInputEnabled(true);

        isTeleporting = false;
    }
}
