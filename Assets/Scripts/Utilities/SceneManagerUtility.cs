using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneManagerUtility
{
    #region Scene Loading Functions
    // Handles loading a specific scene by its index.
    public static void LoadScene(int sceneIndex)
    {
        // Play a sound effect for button click.
        SoundManager.Instance.PlayEffect(SoundType.ButtonClick);
        // Load the scene with the provided index.
        SceneManager.LoadScene(sceneIndex);
        // Play a sound effect indicating the start of a level.
        SoundManager.Instance.PlayEffect(SoundType.LevelStart);
    }

    // Reloads the current scene.
    public static void ReloadCurrentScene()
    {
        // Retrieve the index of the currently active scene.
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        // Use LoadScene to reload the current scene using its index.
        LoadScene(currentSceneIndex);
    }

    // Loads the main menu scene, which is at index 0.
    public static void LoadMainMenu()
    {
        // Play a sound effect for quitting to the menu.
        SoundManager.Instance.PlayEffect(SoundType.ButtonQuit);
        // Load the main menu scene using index 0.
        SceneManager.LoadScene(0);
        // Start background music, assuming main menu has its specific music.
        SoundManager.Instance.PlayMusic(SoundType.BackgroundMusic);
    }
    #endregion

    #region Application Management Functions
    // Quits the game application.
    public static void QuitGame()
    {
        // Play a sound effect for the quit action.
        SoundManager.Instance.PlayEffect(SoundType.ButtonQuit);
        // Check if the application is running in the Unity Editor.
#if UNITY_EDITOR
        {
            // Stop playing the scene in the Unity Editor.
            UnityEditor.EditorApplication.isPlaying = false;
        }
#else
        Application.Quit();
#endif
    }
    #endregion
}
