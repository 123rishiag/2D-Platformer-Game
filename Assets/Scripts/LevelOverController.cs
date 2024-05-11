using System.Collections;
using UnityEngine;

public class LevelOverController : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject levelCompletedImage;
    private PlayerController playerController;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>() != null)
        {
            playerController = collision.gameObject.GetComponent<PlayerController>();
            ShowLevelCompletedNotification();
            LevelManager.Instance.CompleteAndUnlockScene();
            playerController.DisablePlayer();
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
