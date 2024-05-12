using UnityEngine;

public class ChomperController : MonoBehaviour
{
    #region Variables
    // Public variables to adjust the chomper's patrol distance and movement speed.
    public float patrolDistance = 4f; // Distance the chomper patrols back and forth.
    public float moveSpeed = 2f; // Speed at which the chomper moves.

    // Private variables to store the chomper's original position and movement direction.
    private Vector2 originalPosition; // Original starting position of the chomper.
    private bool isMovingRight = true; // Boolean flag to determine if the chomper is moving right.
    private float directionFactor; // Factor used to multiply the movement direction.

    // Animator component for controlling animations.
    private Animator animator;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        // Initialize components.
        animator = GetComponent<Animator>(); // Get the Animator component attached to the chomper.
    }

    private void Start()
    {
        // Capture the initial position of the chomper to use for patrol logic.
        originalPosition = transform.position; // Store the original position at start.
    }

    private void Update()
    {
        // Handle movement each frame.
        ChomperMovement(); // Call the ChomperMovement method to move the chomper.
    }
    #endregion

    #region Collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check collision with objects of type PlayerController.
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            // Decrease health of the player on collision.
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.DecreaseHealth(); // Call the DecreaseHealth method on the player controller.
        }
    }
    #endregion

    #region Movement
    private void ChomperMovement()
    {
        // Calculate the left and right points of the patrol based on the chomper's starting position.
        float leftPoint = originalPosition.x - patrolDistance / 2f; // Left boundary of patrol.
        float rightPoint = originalPosition.x + patrolDistance / 2f; // Right boundary of patrol.

        // Determine the direction of movement using directionFactor.
        directionFactor = isMovingRight ? 1 : -1; // Set direction factor based on current movement direction.

        // Play movement sound effect.
        SoundManager.Instance.PlayEffect(SoundType.ChomperMove);

        // Movement logic for patrol behavior.
        if (isMovingRight)
        {
            // Move right if the condition is true.
            transform.Translate(Vector2.right * directionFactor * moveSpeed * Time.deltaTime);
            if (transform.position.x >= rightPoint)
            {
                // If the chomper reaches or exceeds the right boundary, change direction.
                isMovingRight = false;
                transform.Rotate(0, 180, 0); // Rotate the chomper to face left.
            }
        }
        else
        {
            // Move left otherwise.
            transform.Translate(Vector2.left * directionFactor * moveSpeed * Time.deltaTime);
            if (transform.position.x <= leftPoint)
            {
                // If the chomper reaches or exceeds the left boundary, change direction.
                isMovingRight = true;
                transform.Rotate(0, 180, 0); // Rotate the chomper to face right.
            }
        }

        // Update the animator with the current move speed.
        animator.SetFloat("moveSpeed", moveSpeed);
    }
    #endregion
}
