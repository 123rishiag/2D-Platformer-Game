using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelOverController : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject levelCompletedImage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>() != null)
        {
            ShowLevelCompletedNotification();
            LevelManager.Instance.CompleteAndUnlockScene();
            gameOverScreen.SetActive(true);
        }
    }

    public void ShowLevelCompletedNotification()
    {
        StartCoroutine(DisplayLevelCompletedNotification());
    }

    IEnumerator DisplayLevelCompletedNotification()
    {
        levelCompletedImage.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        levelCompletedImage.SetActive(false);
    }
}
