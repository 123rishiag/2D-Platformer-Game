using UnityEngine;

public class KeyController : MonoBehaviour
{
    #region Variables
    // References to the Animator and BoxCollider2D components for animation and collision handling.
    private Animator animator;
    private BoxCollider2D boxCollider2D;
    #endregion

    #region Initialization
    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        // Get and store the BoxCollider2D component for later use in disabling the key's physical interactions.
        boxCollider2D = GetComponent<BoxCollider2D>();
        // Get and store the Animator component to control key-related animations.
        animator = GetComponent<Animator>();
    }
    #endregion

    #region Collision Handling
    // OnCollisionEnter2D is called when this collider/rigidbody has begun touching another rigidbody/collider.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object we collided with has a PlayerController component (i.e., is the player).
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            // Retrieve the PlayerController component from the collided object.
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            // Call the PickupKey method from the player's script to handle key collection logic.
            playerController.PickupKey();
            // Trigger the key collection animation.
            animator.SetBool("isKeyCollected", true);
        }
    }
    #endregion

    #region Key Destruction Methods
    // Method to disable the key's collider.
    private void DestroyKeyCollider()
    {
        // Safely destroys the BoxCollider2D component attached to this key object.
        Destroy(boxCollider2D);
    }

    // Method to destroy the key game object entirely.
    private void DestroyKey()
    {
        // Destroys this key's game object from the scene, effectively removing the key.
        Destroy(gameObject);
    }
    #endregion
}
