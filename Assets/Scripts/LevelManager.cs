using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        { 
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        InitializeLevels();
    }

    private void InitializeLevels()
    {
        for (int currentLevel = 1; currentLevel <= SceneManager.sceneCountInBuildSettings - 1; currentLevel++)
        {
            if (currentLevel == 1)
            {
                SetLevelStatus(currentLevel, LevelStatus.Unlocked);
            }
            else
            {
                SetLevelStatus(currentLevel, LevelStatus.Locked);
            }
        }
    }
    public void CompleteAndUnlockScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SetLevelStatus(currentSceneIndex, LevelStatus.Completed);
        if ((currentSceneIndex + 1) < SceneManager.sceneCountInBuildSettings)
        {
            SetLevelStatus(currentSceneIndex + 1, LevelStatus.Unlocked);
        }
        StartCoroutine(CompleteAndUnlockSceneWait());
    }
    IEnumerator CompleteAndUnlockSceneWait()
    {
        yield return new WaitForSeconds(0.5f);
        SoundManager.Instance.PlayEffect(SoundType.LevelSuccess);
    }
    public LevelStatus GetLevelStatus(int levelNumber)
    {
        LevelStatus levelStatus = (LevelStatus) PlayerPrefs.GetInt("" + levelNumber, (int)LevelStatus.Locked);
        return levelStatus;
    }

    public void SetLevelStatus(int levelNumber, LevelStatus levelStatus)
    {
        PlayerPrefs.SetInt("" + levelNumber, (int)levelStatus);
    }

}
