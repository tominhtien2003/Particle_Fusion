using Fusion;
using Fusion.Addons.Physics;
using UnityEngine;

[RequireComponent(typeof(StateMachineNetwork))]
public class PlayerController : NetworkBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] NetworkObject bulletPrefabNetwork;

    [Networked] private TickTimer attackCooldown { get; set; }

    private StateMachineNetwork stateMachine;
    private Vector3 direction;
    private NetworkCharacterController characterController;
    public GameInput gameInput { get; private set; }

    private bool isIdle = false;
    private void Awake()
    {
        stateMachine = GetComponent<StateMachineNetwork>();
        characterController = GetComponent<NetworkCharacterController>();
        gameInput = FindFirstObjectByType<GameInput>();
    }
    private void Start()
    {
    }
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            if (data.direction.sqrMagnitude > 0f)
            {
                direction = data.direction;
            }

            if (data.direction != Vector3.zero)
            {
                stateMachine.ChangeState(new PlayerStateMove(this));
                isIdle = false;
            }
            else
            {
                if (!isIdle)
                {
                    stateMachine.ChangeState(new PlayerStateIdle(this));
                    isIdle = true;
                }
            }
            if (HasInputAuthority && data.buttons.IsSet(NetworkInputData.MOUSEBUTTON0))
            {
                stateMachine.ChangeState(new PlayerStateAttack(this));
            }
        }
    }
    #region Handle state idle
    public void HandlePlayerStateIdle()
    {
        characterController.Move(Vector3.zero);
    }
    #endregion
    #region Handle state Move
    public void HandlePlayerStateMove()
    {
        characterController.Move(direction * moveSpeed * Runner.DeltaTime);
    }
    #endregion
    #region Handle state Attack
    public void HandlePlayerStateAttack()
    {
        if (attackCooldown.ExpiredOrNotRunning(Runner))
        {
            attackCooldown = TickTimer.CreateFromSeconds(Runner, .5f);
            Runner.Spawn(bulletPrefabNetwork, transform.position + direction, Quaternion.LookRotation(direction), Object.InputAuthority,
            (Runner, O) =>
            {
                O.GetComponent<BulletController>().Initial();
            });
        }
    }
    #endregion
}
