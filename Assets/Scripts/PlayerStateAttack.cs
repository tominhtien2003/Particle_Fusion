public class PlayerStateAttack : BaseState
{
    public PlayerStateAttack(PlayerController player) : base(player)
    {
    }

    public override void Enter()
    {

    }

    public override void Excute()
    {
        player.HandlePlayerStateAttack();
    }

    public override void Exit()
    {

    }

    public override string GetTypeState()
    {
        return "PlayerAttack";
    }
}
