using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelLoader : MonoBehaviour
{
    private Button button;
    public string buttonType;
    public GameObject levelLockedImage;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }
    private void OnClick()
    {
        if (buttonType == "Level")
        {
            int levelIndex = transform.GetSiblingIndex();
            LevelStatus levelStatus = LevelManager.Instance.GetLevelStatus(levelIndex+1);
            switch (levelStatus)
            {
                case LevelStatus.Locked:
                    SoundManager.Instance.PlayEffect(SoundType.ButtonLock);
                    ShowLevelLockedNotification();
                    break;
                case LevelStatus.Unlocked:
                case LevelStatus.Completed:
                    SoundManager.Instance.PlayEffect(SoundType.ButtonClick);
                    SceneManager.LoadScene(levelIndex + 1);
                    break;
            }
        }
        else if (buttonType == "Back")
        {
            SoundManager.Instance.PlayEffect(SoundType.ButtonQuit);
            SceneManager.LoadScene(0);
        }
    }

    public void ShowLevelLockedNotification()
    {
        StartCoroutine(DisplayLevelLockedNotification());
    }

    IEnumerator DisplayLevelLockedNotification()
    {
        levelLockedImage.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        levelLockedImage.SetActive(false);
    }
}
