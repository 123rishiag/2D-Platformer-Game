using System.Collections;
using UnityEngine;

public class LevelOverController : MonoBehaviour
{
    // Section for public references and private variables.
    #region Variables
    public GameObject gameOverScreen; // UI screen to display when the game is over.
    public GameObject levelCompletedImage; // Image to display when a level is completed.
    private PlayerController playerController; // Reference to the player controller.
    #endregion

    // Section for Unity's collision detection functions.
    #region Collision Handling
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Trigger detection to handle game level completion.
        // Checks if the object that triggered the event is the player.
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            // Store the PlayerController component for later use.
            playerController = collision.gameObject.GetComponent<PlayerController>();

            // Perform actions related to completing the level.
            ShowLevelCompletedNotification(); // Show visual notification for level completion.
            LevelManager.Instance.CompleteAndUnlockScene(); // Trigger the completion and unlocking of the next scene.
            playerController.DisablePlayer(); // Disable player controls and other interactions.
            gameOverScreen.SetActive(true); // Show the game over screen.
        }
    }
    #endregion

    // Section for handling UI notifications related to level completion.
    #region UI Notifications
    public void ShowLevelCompletedNotification()
    {
        // Private method to start the coroutine for displaying level completion notification.
        StartCoroutine(DisplayLevelCompletedNotification());
    }

    private IEnumerator DisplayLevelCompletedNotification()
    {
        // Coroutine to display the level completed image briefly.
        levelCompletedImage.SetActive(true); // Activate the level completed image.
        yield return new WaitForSeconds(1.5f); // Wait for 1.5 seconds.
        levelCompletedImage.SetActive(false); // Deactivate the level completed image after showing it briefly.
    }
    #endregion
}
