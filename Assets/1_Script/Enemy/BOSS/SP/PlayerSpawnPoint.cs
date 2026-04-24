using UnityEngine;

public class BossSceneSpawnPoint : MonoBehaviour
{
    [SerializeField] private bool useTimeLine = true;

    private void Start()
    {
        if (useTimeLine)
        {
            PlayerManager.Instance.SetActive(false);
        }
        else
        {
            SpawnPlayer();
        }
    }

    public void SpawnPlayer()
    {
        PlayerManager.Instance.Teleport(transform.position);
        PlayerManager.Instance.SetActive(true);
    }
}
