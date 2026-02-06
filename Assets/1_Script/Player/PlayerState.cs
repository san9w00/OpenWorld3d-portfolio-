using UnityEngine;

public abstract class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player; 

    public PlayerState(PlayerStateMachine sm)
    {
        sm = stateMachine;
        //player = sm.player;
    }

    public virtual void Enter() { }
    public virtual void Move(Vector3 dir) { }
    public virtual void Jump() { }
}
