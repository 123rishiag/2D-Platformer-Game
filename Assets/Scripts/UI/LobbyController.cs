using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    // References to UI elements within the lobby.
    public Button playButton;
    public Button quitButton;
    public Button muteButton;
    public GameObject lobbyGameObject;

    #region Initialization
    private void Awake()
    {
        // Add listeners to buttons for corresponding methods.
        playButton.onClick.AddListener(PlayGame); // Listener for the Play button.
        quitButton.onClick.AddListener(QuitGame); // Listener for the Quit button.
        muteButton.onClick.AddListener(MuteGame); // Listener for the Mute button.
    }
    #endregion

    #region Button Functionality Methods
    // Handles the mute button functionality.
    private void MuteGame()
    {
        // Get the TextMeshPro component from the mute button's children to change the display text.
        TextMeshProUGUI muteButtonText = muteButton.GetComponentInChildren<TextMeshProUGUI>();

        // Toggle the mute state and update the button's text accordingly.
        if (muteButtonText.text == "Mute: On")
        {
            muteButtonText.text = "Mute: Off"; // If currently muted, set to unmute.
        }
        else
        {
            muteButtonText.text = "Mute: On"; // If currently unmuted, set to mute.
        }

        // Call the SoundManager to handle the game's mute state and play a button click effect.
        SoundManager.Instance.MuteGame();
        SoundManager.Instance.PlayEffect(SoundType.ButtonClick);
    }

    // Handles the play button functionality.
    private void PlayGame()
    {
        // Play a button click sound effect.
        SoundManager.Instance.PlayEffect(SoundType.ButtonClick);

        // Activate the lobby game object which can be a menu or a game scene.
        lobbyGameObject.SetActive(true);
    }

    // Handles the quit button functionality.
    private void QuitGame()
    {
        // Call a utility method to quit the game.
        SceneManagerUtility.QuitGame();
    }
    #endregion
}
