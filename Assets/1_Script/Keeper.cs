using UnityEngine;

public class Keeper : MonoBehaviour
{
    public static Keeper Instance;

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

    public void DestroyALL()
    {
        Destroy(gameObject);
    }
}
