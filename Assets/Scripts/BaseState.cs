public abstract class BaseState
{
    protected PlayerController player;

    public BaseState(PlayerController player)
    {
        this.player = player;
    }

    public abstract void Enter();
    public abstract void Excute();
    public abstract void Exit();
    public abstract string GetTypeState();
}
