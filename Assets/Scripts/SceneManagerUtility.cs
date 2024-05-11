using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneManagerUtility
{
    // Load a scene by its index
    public static void LoadScene(int sceneIndex)
    {
        SoundManager.Instance.PlayEffect(SoundType.ButtonClick);
        SceneManager.LoadScene(sceneIndex);
        SoundManager.Instance.PlayEffect(SoundType.LevelStart);
    }

    // Reload the current scene
    public static void ReloadCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        LoadScene(currentSceneIndex);
    }

    // Load the main menu scene, assumed to be at index 0
    public static void LoadMainMenu()
    {
        SoundManager.Instance.PlayEffect(SoundType.ButtonQuit);
        SceneManager.LoadScene(0);
        SoundManager.Instance.PlayMusic(SoundType.BackgroundMusic);
    }

    // Quit the game
    public static void QuitGame()
    {
        SoundManager.Instance.PlayEffect(SoundType.ButtonQuit);
        if (UnityEditor.EditorApplication.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        Application.Quit();
    }
}
