using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState currentState {  get; private set; }
    public Player player { get; private set; }

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        
    }

    private void ChangeState(PlayerState newState)
    {
        currentState = newState;
        currentState.Enter();
    }
}
