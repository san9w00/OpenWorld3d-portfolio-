using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public GameObject Player { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void RegisterPlayer(GameObject player)
    {
        Player = player;
    }

    public void SetActive(bool active)
    {
        Player.SetActive(active);
    }

    public void Teleport(Vector3 position)
    {
        Player.transform.position = position;
    }
}
