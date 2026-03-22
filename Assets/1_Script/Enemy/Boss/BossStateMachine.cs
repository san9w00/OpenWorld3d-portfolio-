using UnityEngine;

public class BossStateMachine
{
    private IBossState currentState;

    public void ChangeState(IBossState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }
}
