using UnityEngine;

public class GameInput : MonoBehaviour
{
    public Vector3 GetMoveDirectionPlayer()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");
        return new Vector3(horizontalInput, 0f, VerticalInput);
    }
}
