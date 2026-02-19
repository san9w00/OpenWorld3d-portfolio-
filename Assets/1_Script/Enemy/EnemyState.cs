using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    protected EnemyAI _enemyAI;
    protected EnemyStatus _enemyStatus;

    protected virtual void Awake()
    {
        _enemyAI = GetComponent<EnemyAI>();
        _enemyStatus = GetComponent<EnemyStatus>();
    }

    public void Handle()
    {
        Action();
        Decision();
    }

    protected abstract void Action();
    protected abstract void Decision();
}
