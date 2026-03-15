using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifeTime = 4f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
