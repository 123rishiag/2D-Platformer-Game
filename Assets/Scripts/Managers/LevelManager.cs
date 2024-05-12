using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    #region Singleton Implementation
    private static LevelManager instance; // Static instance of LevelManager which allows it to be accessed by any other script.
    public static LevelManager Instance { get { return instance; } } // Publicly accessible property to get the instance.

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // Set the instance if it hasn't been set yet.
            DontDestroyOnLoad(gameObject); // Ensures that the instance persists across different scenes.
        }
        else
        {
            Destroy(gameObject); // Ensures there is only one instance of the object in the game.
        }
    }
    #endregion

    #region Initialization
    private void Start()
    {
        InitializeLevels(); // Initialize level statuses on game start.
    }

    private void InitializeLevels()
    {
        // Set initial level status as Unlocked for first level and Locked for others.
        for (int currentLevel = 1; currentLevel <= SceneManager.sceneCountInBuildSettings - 1; currentLevel++)
        {
            if (currentLevel == 1)
            {
                SetLevelStatus(currentLevel, LevelStatus.Unlocked); // First level is always unlocked.
            }
            else
            {
                SetLevelStatus(currentLevel, LevelStatus.Locked); // Remaining levels are initially locked.
            }
        }
    }
    #endregion

    #region Level Management
    public void CompleteAndUnlockScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // Get the index of the current active scene.
        SetLevelStatus(currentSceneIndex, LevelStatus.Completed); // Set the current scene's status to Completed.

        // Check if there's a next level and unlock it.
        if ((currentSceneIndex + 1) < SceneManager.sceneCountInBuildSettings)
        {
            SetLevelStatus(currentSceneIndex + 1, LevelStatus.Unlocked); // Unlock the next level.
        }

        StartCoroutine(CompleteAndUnlockSceneWait()); // Start Complete And Unlock Scene Coroutine.
    }

    private IEnumerator CompleteAndUnlockSceneWait()
    {
        yield return new WaitForSeconds(0.5f); // Delay before playing the success sound.
        SoundManager.Instance.PlayEffect(SoundType.LevelSuccess); // Play level completion success sound.
    }

    public LevelStatus GetLevelStatus(int levelNumber)
    {
        // Retrieves the status of a specific level from player preferences.
        LevelStatus levelStatus = (LevelStatus)PlayerPrefs.GetInt("" + levelNumber, (int)LevelStatus.Locked);
        return levelStatus; // Return the status as LevelStatus enum.
    }

    public void SetLevelStatus(int levelNumber, LevelStatus levelStatus)
    {
        // Sets the status of a specific level in player preferences.
        PlayerPrefs.SetInt("" + levelNumber, (int)levelStatus); // Store the level status in PlayerPrefs.
    }
    #endregion
}

#region Enums
// Enum defining possible states for levels within the game.
public enum LevelStatus
{
    Locked,    // Indicates the level is locked and cannot be accessed.
    Unlocked,  // Indicates the level is unlocked and can be played.
    Completed  // Indicates the level has been completed.
}
#endregion