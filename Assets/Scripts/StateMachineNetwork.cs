using Fusion;
using UnityEngine;

public class StateMachineNetwork : NetworkBehaviour
{
    private BaseState currentState;
    public void ChangeState(BaseState state)
    {
        if (currentState != null && state.GetTypeState() == currentState.GetTypeState())
        {
            return;
        }
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = state;

        if (currentState != null)
        {
            currentState.Enter();
        }
    }
    public override void FixedUpdateNetwork()
    {
        if (currentState != null)
        {
            currentState.Excute();
        }
    }
}
