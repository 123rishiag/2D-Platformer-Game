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
        SceneManagerUtility.ReloadCurrentScene();
    }
    private void QuitScene()
    {
        SceneManagerUtility.LoadMainMenu();
    }
}
