using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyController : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.PickupKey();
            animator.SetBool("isKeyCollected", true);
        }
    }
    private void DestroyKeyCollider()
    {
        Destroy(boxCollider2D);
    }
    private void DestroyKey()
    {
        Destroy(gameObject);
    }
}
