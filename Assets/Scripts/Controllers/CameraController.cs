using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Variables
    // Reference to the player's transform used to follow the player.
    public Transform player;

    // Relative position to maintain from the player, allowing the camera to follow at a set distance and angle.
    public Vector3 relativePosition;

    // Speed at which the camera moves to follow the player.
    public float moveSpeed = 5.0f;
    #endregion

    #region Unity Methods
    void LateUpdate()
    {
        FollowPlayer();
    }
    #endregion

    #region Camera Behavior Methods
    // Handles the camera's following behavior, keeping it a fixed distance from the player.
    void FollowPlayer()
    {
        // Calculate the target position based on the player's position and the predetermined relative position.
        Vector3 targetPosition = player.position + relativePosition;

        // Interpolate between the current camera position and the target position to create a smooth follow effect.
        // Multiplication by Time.deltaTime ensures smooth movement that is frame rate independent.
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
    #endregion
}
