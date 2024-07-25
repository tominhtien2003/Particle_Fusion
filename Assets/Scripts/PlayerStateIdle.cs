using UnityEngine;

public class PlayerStateIdle : BaseState
{
    public PlayerStateIdle(PlayerController player) : base(player)
    {
    }

    public override void Enter()
    {
        
    }

    public override void Excute()
    {
        player.HandlePlayerStateIdle();
    }

    public override void Exit()
    {
        
    }

    public override string GetTypeState()
    {
        return "PlayerIdle";
    }
}
