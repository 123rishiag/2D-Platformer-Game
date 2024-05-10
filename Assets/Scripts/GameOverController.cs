using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    public Button restartButton;
    public Button quitButton;

    private void Awake()
    {
        restartButton.onClick.AddListener(ReloadScene);
        quitButton.onClick.AddListener(QuitScene);
    }
    public void ReloadMenu()
    {
        gameObject.SetActive(true);
    }
    private void ReloadScene()
    {
        SoundManager.Instance.PlayEffect(SoundType.ButtonClick);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        SoundManager.Instance.PlayEffect(SoundType.LevelStart);
    }
    private void QuitScene()
    {
        SoundManager.Instance.PlayEffect(SoundType.ButtonQuit);
        gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }
}
