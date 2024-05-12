using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    #region Variables
    // Buttons to manage game control actions.
    public Button restartButton; // Button to restart the current scene.
    public Button quitButton;    // Button to quit to the main menu.
    #endregion

    #region Unity Lifecycle
    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        // Add listeners to buttons for their respective actions.
        restartButton.onClick.AddListener(ReloadScene); // Listener for restarting the game.
        quitButton.onClick.AddListener(QuitScene);       // Listener for quitting to the main menu.
    }
    #endregion

    #region Public Methods
    // Public method to activate the game over menu.
    public void ReloadMenu()
    {
        // Activates the game over menu GameObject.
        gameObject.SetActive(true);
    }
    #endregion

    #region Private Methods
    // Private method to reload the current scene.
    private void ReloadScene()
    {
        // Calls utility method to reload the current scene.
        SceneManagerUtility.ReloadCurrentScene();
    }

    // Private method to load the main menu scene.
    private void QuitScene()
    {
        // Calls utility method to load the main menu.
        SceneManagerUtility.LoadMainMenu();
    }
    #endregion
}
