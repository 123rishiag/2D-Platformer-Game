using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelLoader : MonoBehaviour
{
    #region Variables
    private Button button; // Button component on this GameObject.
    public string buttonType; // Type of button ("Level" or "Back").
    public GameObject levelLockedImage; // UI element shown when a level is locked.
    #endregion

    #region Initialization
    private void Awake()
    {
        // Initializes the button component and adds an event listener to it.
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick); // Attach the OnClick method to the button's click event.
    }
    #endregion

    #region Button Click Handlers
    private void OnClick()
    {
        // Handles click events based on the type of button.
        if (buttonType == "Level")
        {
            // If the button type is "Level", handle level loading.
            int levelIndex = transform.GetSiblingIndex(); // Determine the level index based on the button's position in the UI hierarchy.
            LevelStatus levelStatus = LevelManager.Instance.GetLevelStatus(levelIndex + 1); // Retrieve the status of the level from LevelManager.

            // Handle actions based on the level status.
            switch (levelStatus)
            {
                case LevelStatus.Locked:
                    // If the level is locked, play a sound and show a notification.
                    SoundManager.Instance.PlayEffect(SoundType.ButtonLock);
                    ShowLevelLockedNotification();
                    break;
                case LevelStatus.Unlocked:
                case LevelStatus.Completed:
                    // If the level is unlocked or completed, load the level scene.
                    SceneManagerUtility.LoadScene(levelIndex + 1);
                    break;
            }
        }
        else if (buttonType == "Back")
        {
            // If the button type is "Back", load the main menu scene.
            SceneManagerUtility.LoadMainMenu();
        }
    }
    #endregion

    #region Level Locked Notification
    public void ShowLevelLockedNotification()
    {
        // Private method to start the notification display coroutine.
        StartCoroutine(DisplayLevelLockedNotification());
    }

    private IEnumerator DisplayLevelLockedNotification()
    {
        // Coroutine to display the level locked notification.
        levelLockedImage.SetActive(true); // Activate the locked level image.
        yield return new WaitForSeconds(1.5f); // Wait for 1.5 seconds.
        levelLockedImage.SetActive(false); // Deactivate the locked level image.
    }
    #endregion
}
