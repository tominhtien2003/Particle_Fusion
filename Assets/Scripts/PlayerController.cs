using Fusion;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public static PlayerController LocalPlayerInstance { get; private set; }

    private NetworkCharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<NetworkCharacterController>();
    }
    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            LocalPlayerInstance = this;
        }
    }
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            Vector3 moveDirectionPlayer = data.direction;

            characterController.Move(moveDirectionPlayer);
        }
    }
}
