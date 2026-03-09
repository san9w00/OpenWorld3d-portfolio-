using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonTeleporter : MonoBehaviour, IInteractable
{
    [SerializeField] private string deongeonSceneName;

    public bool CanInteract()
    {
        return true;
    }

    public string GetInteractText()
    {
        return "Enter the Dungeon";
    }

    public void Interact()
    {
        SceneManager.LoadScene(deongeonSceneName);
    }
}
