using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelOverController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerController>() != null)
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            if ((currentSceneIndex + 1) < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene((currentSceneIndex + 1));
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
