using UnityEngine;

public enum State
{
    Idle,
    Attack,
    Pursue,
    Avoid
}

public class EnemyStateMachine : MonoBehaviour
{
    public EnemyState currentState { get; set; }

    private EnemyState _IdleState, _AttackState, _PursueState, _AvoidState;

    private void AttachStateToObject()
    {
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
                currentState = _AttackState;
                break;
            case State.Pursue:
                currentState = _PursueState;
                break;
            case State.Avoid:
                currentState = _AvoidState;
                break;
        }
    }

    public void RunState()
    {
        currentState.Handle();
    }
}
