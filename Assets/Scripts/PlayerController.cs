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
    private bool isJumping = false;
    private bool canDoubleJump = true;
    private int currentHealth = 0;
    private float horizontal = 0.0f;
    private float xMovement = 0.0f;
    private float vertical = 0.0f;
    private float yMovement = 0.0f;
    private float crouchingHeightRatio = 0.6f;
    private float crouchingWidthSizeRatio = 0.7f;
    private float crouchingWidthOffsetRatio = .025f;
    public float doubleJumpCooldown = 0.5f;
    private bool isShieldActive = false;
    private Vector2 colliderOffsetOriginal = Vector2.zero;
    private Vector2 colliderSizeOriginal = Vector2.zero;

    public LayerMask groundLayer;
    public float groundCheckDistance = 0.001f;

    public Animator animator;
    private CapsuleCollider2D capsuleCollider2D;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    public int maxHealth = 3;
    public float moveSpeed = 1.0f;
    public float jumpForce = 1.0f;

    public ScoreController scoreController;
    public GameOverController gameOverController;
    public ParticleSystemController particleSystemController;

    private void Awake()
    {
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        colliderSizeOriginal = capsuleCollider2D.size;
        colliderOffsetOriginal = capsuleCollider2D.offset;
        currentHealth = maxHealth;
    }
    void Update()
    {
        PlayerMovement();
        PlayerJump();
        PlayerCrouch();
    }
    public int GetHealth()
    {
        return currentHealth;
    }
    public void DecreaseHealth() 
    {
        if(isShieldActive)
            return;
        if(currentHealth > 1)
        {
            currentHealth -= 1;
            SoundManager.Instance.PlayEffect(SoundType.PlayerHurt);
            animator.SetTrigger("isHurt");
            ActivateShield();
            ActivateSpeedBoost();
        }
        else
        {
            currentHealth -= 1;
            KillPlayer();
        }
    }
    public void KillPlayer()
    {
        SoundManager.Instance.PlayEffect(SoundType.PlayerDeath);
        animator.SetTrigger("isDead");
        particleSystemController.PlayFailParticleEffect();
        StartCoroutine(KillPlayerWait());
        spriteRenderer.enabled = false;
        rb.simulated = false;
        capsuleCollider2D.enabled = false;
    }
    IEnumerator KillPlayerWait()
    {
        yield return new WaitForSeconds(0.5f);
        SoundManager.Instance.PlayEffect(SoundType.LevelFail);
        
    }
    private void ActivateShield()
    {
        isShieldActive = true;
        StartCoroutine(DeactivateShieldAfterTime(5)); // Shield lasts for 5 seconds
    }
    IEnumerator DeactivateShieldAfterTime(float shieldSeconds)
    {
        yield return new WaitForSeconds(shieldSeconds);
        isShieldActive = false;
    }
    private void ReturntoIdle()
    {
        animator.SetTrigger("isIdle");
    }
    private void ReloadMenu()
    {
        animator.enabled = false;
        this.enabled = false;
        gameOverController.ReloadMenu();
    }
    private void PlayerMovement()
    {
        // Left and Right Movements
        horizontal = Input.GetAxisRaw("Horizontal");
        PlayerFacingDirection();
        if (!isCrouching)
        {
            xMovement = horizontal * moveSpeed * Time.deltaTime;
            if (xMovement != 0.0f)
            {
                SoundManager.Instance.PlayEffect(SoundType.PlayerMove);
            }
            transform.position = new Vector3(transform.position.x + xMovement, transform.position.y, 0.0f);
            animator.SetFloat("moveSpeed", Mathf.Abs(horizontal));           
        }
    }
    public void ActivateSpeedBoost()
    {
        StartCoroutine(SpeedBoost(5));
    }
    IEnumerator SpeedBoost(float boostDuration)
    {
        moveSpeed *= 2;
        yield return new WaitForSeconds(boostDuration);
        moveSpeed /= 2;
    }
    private void PlayerJump()
    {
        if (!isCrouching)
        {
            // Jump Movement
            isGrounded = GroundCheck();
            vertical = Input.GetAxisRaw("Vertical");
            if (isGrounded && isJumping)
            {
                isJumping = false;
                animator.SetBool("isJumping", false);
            }
            if (isGrounded && !isJumping)
            {
                canDoubleJump = true;
            }
            if (vertical > 0.0f)
            {
                if (isGrounded && !isJumping)
                {
                    isJumping = true;
                    PerformJump();
                    animator.SetBool("isJumping", true);
                }
                else if (!isGrounded && canDoubleJump && isJumping)
                {
                    isJumping = true;
                    PerformJump();
                    StartCoroutine(DoubleJumpCooldown());
                    animator.SetBool("isJumping", true);
                }
            }
            animator.SetBool("isJumping", isJumping);
        }
    }
    IEnumerator DoubleJumpCooldown()
    {
        yield return new WaitForSeconds(doubleJumpCooldown);
        canDoubleJump = false;
    }
    private void PerformJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        yMovement = vertical * jumpForce;
        SoundManager.Instance.PlayEffect(SoundType.PlayerJump);
        rb.AddForce(new Vector2(0.0f, yMovement), ForceMode2D.Impulse);
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
        SoundManager.Instance.PlayEffect(SoundType.ItemPickup);
        scoreController.IncreaseScore(10);
    }
}
