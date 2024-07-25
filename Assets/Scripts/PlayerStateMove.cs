using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMove : BaseState
{
    public PlayerStateMove(PlayerController player) : base(player)
    {
    }

    public override void Enter()
    {
        
    }

    public override void Excute()
    {
        player.HandlePlayerStateMove();
    }

    public override void Exit()
    {
        
    }

    public override string GetTypeState()
    {
        return "PlayerMove";
    }
}
