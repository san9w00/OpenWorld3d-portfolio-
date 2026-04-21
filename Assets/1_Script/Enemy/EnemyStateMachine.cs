using UnityEngine;

public enum State
{
    Idle,
    Attack,
    Pursue,
}

[RequireComponent(typeof(EnemyIdleState))]
[RequireComponent(typeof(EnemyPursueState))]
[RequireComponent(typeof(EnemyAttackState))]
public class EnemyStateMachine : MonoBehaviour
{
    public EnemyState currentState { get; set; }

    private EnemyState _IdleState, _AttackState, _PursueState;
    private EnemyState _BossAttackState;

    private void AttachStateToObject()
    {
        _IdleState = gameObject.GetComponent<EnemyIdleState>();
        _PursueState = gameObject.GetComponent<EnemyPursueState>(); 
        _AttackState = gameObject.GetComponent<EnemyAttackState>();
        _BossAttackState = GetComponent<BossAttackState>();
    }

    private void Start()
    {
        AttachStateToObject();
        currentState = _IdleState;
    }

    public void Transition(State state)
    {
        switch (state)
        {
            case State.Idle:
                currentState = _IdleState;
                break;
            case State.Attack:
                currentState = _BossAttackState != null ? _BossAttackState : _AttackState;
                break;
            case State.Pursue:
                currentState = _PursueState;
                break;
        }
    }

    public void RunState()
    {
        currentState.Handle();
    }
}
