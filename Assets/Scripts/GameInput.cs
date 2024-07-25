using UnityEngine;

public class GameInput : MonoBehaviour
{
    public PlayerInputAction inputAction { get; private set; }
    private void Start()
    {
        inputAction = new PlayerInputAction();
        inputAction.Player.Enable();
    }
    public Vector2 GetMoveDirectionPlayer()
    {
        return inputAction.Player.Move.ReadValue<Vector2>();
    }
}
