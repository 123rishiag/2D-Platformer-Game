using UnityEngine;

public class DeathController : MonoBehaviour
{
    #region Variables
    // Reference to the PlayerController to call player death-related functions.
    private PlayerController playerController;
    #endregion

    #region Unity Collision Handling
    // This method is triggered when another collider enters the trigger collider attached to this object.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object has a PlayerController component.
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            // Get the PlayerController component from the colliding object.
            playerController = collision.gameObject.GetComponent<PlayerController>();

            // Call the KillPlayer method on the PlayerController to handle player death.
            playerController.KillPlayer();
        }
    }
    #endregion
}
