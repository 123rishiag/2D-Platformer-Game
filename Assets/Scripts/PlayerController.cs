using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private bool isFacingLeft = false;
    private bool isCrouching = false;
    private bool isGrounded = true;
    private int currentHealth = 0;
    private float horizontal = 0.0f;
    private float xMovement = 0.0f;
    private float vertical = 0.0f;
    private float yMovement = 0.0f;
    private float crouchingHeightRatio = 0.6f;
    private float crouchingWidthSizeRatio = 0.7f;
    private float crouchingWidthOffsetRatio = .025f;
    private Vector2 colliderOffsetOriginal = Vector2.zero;
    private Vector2 colliderSizeOriginal = Vector2.zero;

    public LayerMask groundLayer;
    public float groundCheckDistance = 0.001f;

    public Animator animator;
    private CapsuleCollider2D capsuleCollider2D;
    private Rigidbody2D rb;

    public int maxHealth = 3;
    public float moveSpeed = 1.0f;
    public float jumpForce = 1.0f;

    public ScoreController scoreController;
    public GameOverController gameOverController;


    void Start()
    {
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        colliderSizeOriginal = capsuleCollider2D.size;
        colliderOffsetOriginal = capsuleCollider2D.offset;
        currentHealth = maxHealth;
    }
    void Update()
    {
        PlayerMovement();
        PlayerCrouch();
    }

    public int GetHealth()
    {
        return currentHealth;
    }
    public void DecreaseHealth() 
    {
        if (currentHealth > 0) {
            currentHealth -= 1;
        }
        if(currentHealth > 0)
        {
            animator.SetTrigger("isHurt");
        }
        else
        {
            KillPlayer();
        }
    }
    private void KillPlayer()
    {
        animator.SetTrigger("isDead");
    }
    private void ReturntoIdle()
    {
        animator.SetTrigger("isIdle");
    }
    private void ReloadMenu()
    {
        gameOverController.ReloadMenu();
    }

    private void PlayerMovement()
    {
        // Left and Right Movements
        horizontal = Input.GetAxisRaw("Horizontal");
        PlayerFacingDirection();
        xMovement = horizontal * moveSpeed * Time.deltaTime;
        if (!isCrouching)
        { 
            transform.position = new Vector3(transform.position.x + xMovement, transform.position.y, 0.0f);
            animator.SetFloat("moveSpeed", Mathf.Abs(horizontal));

            // Jump Movement
            isGrounded = GroundCheck();
            vertical = Input.GetAxisRaw("Vertical");
            yMovement = vertical * jumpForce;
            if (vertical > 0.0f && isGrounded)
            {
                rb.AddForce(new Vector2(0.0f, yMovement), ForceMode2D.Force);
                rb.velocity = new Vector2(rb.velocity.x, 0f);
            }
            animator.SetFloat("jumpForce", vertical);
        }
    }

    private void PlayerCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;
            if (isCrouching)
            {
                capsuleCollider2D.size = new Vector2(capsuleCollider2D.size.x / crouchingWidthSizeRatio, capsuleCollider2D.size.y * crouchingHeightRatio);
                capsuleCollider2D.offset = new Vector2(capsuleCollider2D.offset.x / crouchingWidthOffsetRatio, capsuleCollider2D.offset.y * crouchingHeightRatio);
            }
            else
            {
                capsuleCollider2D.size = colliderSizeOriginal;
                capsuleCollider2D.offset = colliderOffsetOriginal;
            }
            animator.SetBool("isCrouching", isCrouching);
        }
    }

    private void PlayerFacingDirection()
    {
        if (horizontal < 0.0f)
        {
            if (!isFacingLeft)
            {
                transform.Rotate(0, 180, 0);
            }
            isFacingLeft = true;

        }

        else if (horizontal > 0.0f)
        {
            if (isFacingLeft)
            {
                transform.Rotate(0, 180, 0);
            }
            isFacingLeft = false;
        }
    }

    bool GroundCheck()
    {
        // Cast a ray downward to check for ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);

        // If the ray hits something on the ground layer, return true
        return hit.collider != null;
    }

    internal void PickupKey()
    {
        scoreController.IncreaseScore(10);
    }
}
