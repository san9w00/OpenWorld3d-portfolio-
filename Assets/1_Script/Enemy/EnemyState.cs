using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    protected virtual void Awake()
    {

    }

    public void Handle()
    {

    }

    protected abstract void Action();
    protected abstract void Decision();
}
