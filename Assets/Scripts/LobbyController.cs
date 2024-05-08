using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    public Button playButton;
    public Button quitButton;
    public GameObject lobbyGameObject;

    private void Awake()
    {
        playButton.onClick.AddListener(PlayGame);
        quitButton.onClick.AddListener(QuitGame);
    }
    private void PlayGame()
    {
        SoundManager.Instance.PlayEffect(SoundType.ButtonClick);
        lobbyGameObject.SetActive(true);
    }
    private void QuitGame()
    {
        SoundManager.Instance.PlayEffect(SoundType.ButtonQuit);
        if (UnityEditor.EditorApplication.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        Application.Quit();
    }
}
