using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleport : MonoBehaviour, IInteractable
{
    [SerializeField] private string deongeonSceneName;

    [Header("Boss Setting")]
    [SerializeField] private bool isBossScene;

    public bool CanInteract()
    {
        return true;
    }

    public string GetInteractText()
    {
        return "Enter";
    }

    public void Interact()
    {
        // 보스씬이면 자동 브금 잠금
        if (isBossScene)
        {
            SoundManager.Instance.SetAutoBGMBlocked(true);
            SoundManager.Instance.StopBGM();
        }

        SceneManager.LoadScene(deongeonSceneName);
    }
}
