using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelOverController : MonoBehaviour
{
    public GameObject gameOverScreen;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>() != null)
        {
            LevelManager.Instance.CompleteAndUnlockScene();
            gameOverScreen.SetActive(true);
        }
    }
}
