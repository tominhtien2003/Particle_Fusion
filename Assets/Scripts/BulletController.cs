using Fusion;
using UnityEngine;

public class BulletController : NetworkBehaviour
{
    [Networked] TickTimer life { get; set; }
    [SerializeField] float moveSpeedBullet;
    public void Initial()
    {
        life = TickTimer.CreateFromSeconds(Runner, 5f);
    }
    public override void FixedUpdateNetwork()
    {
        if (life.Expired(Runner))
        {
            Runner.Despawn(Object);
        }
        else
        {
            transform.position += moveSpeedBullet * transform.forward * Runner.DeltaTime;
        }
    }
}
