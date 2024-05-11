using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    // Player state variables
    private bool isFacingLeft = false;
    private bool isCrouching = false;
    private bool isGrounded = true;
    private bool isJumping = false;
    private bool canDoubleJump = true;
    private bool isShielded = false;
    private bool isSpeedBoosted = false;

    // Player health and movement variables
    private int currentHealth = 0;
    private float horizontal = 0.0f;
    private float xMovement = 0.0f;
    private float vertical = 0.0f;
    private float yMovement = 0.0f;
    public int maxHealth = 3;
    public float moveSpeed = 1.0f;
    public float jumpForce = 1.0f;

    // Player effects and thresholds
    private float shieldTimeRemaining = 0;
    private float speedBoostTimeRemaining = 0;
    private float lastCheckPositionY;
    private float checkThreshold = 0.01f;
    public float doubleJumpCooldown = 0.5f;
    private float crouchingHeightRatio = 0.6f;
    private float crouchingWidthSizeRatio = 0.7f;
    private float crouchingWidthOffsetRatio = .025f;

    // Collider configurations
    private Vector2 colliderOffsetOriginal = Vector2.zero;
    private Vector2 colliderSizeOriginal = Vector2.zero;

    // Layer and distance for ground check
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.001f;

    // External references
    public Animator animator;
    public ScoreController scoreController;
    public GameOverController gameOverController;
    public ParticleSystemController particleSystemController;

    private CapsuleCollider2D capsuleCollider2D;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    #endregion

    #region Unity Functions
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
        UpdateStates();
        PlayerMovement();
        GroundCheck();
        PlayerJump();
        PlayerCrouch();
    }
    #endregion

    #region Player State Updates
    private void UpdateStates()
    {
        // Handles shield time decrement
        if (isShielded)
        {
            shieldTimeRemaining -= Time.deltaTime;
            if (shieldTimeRemaining <= 0)
            {
                isShielded = false;
            }
        }
        // Handles speed boost time decrement
        if (isSpeedBoosted)
        {
            speedBoostTimeRemaining -= Time.deltaTime;
            if (speedBoostTimeRemaining <= 0)
            {
                moveSpeed /= 2;
                isSpeedBoosted = false;
            }
        }
    }
    #endregion

    #region Player Actions
    public void ActivateShield()
    {
        isShielded = true;
        shieldTimeRemaining = 5.0f;
    }
    public void ActivateSpeedBoost()
    {
        if (!isSpeedBoosted)
        {
            moveSpeed *= 2;
        }
        isSpeedBoosted = true;
        speedBoostTimeRemaining = 5.0f;
    }
    public int GetHealth()
    {
        return currentHealth;
    }
    public void DecreaseHealth()
    {
        if (isShielded)
            return;
        if (currentHealth > 1)
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
        particleSystemController.PlayFailParticleEffect();
        animator.SetTrigger("isDead");
        SoundManager.Instance.PlayEffect(SoundType.LevelFail);
        StartCoroutine(KillPlayerWait());
    }
    IEnumerator KillPlayerWait()
    {
        yield return new WaitForSeconds(2f);
        DisablePlayer();
    }
    public void DisablePlayer()
    {
        spriteRenderer.enabled = false;
        rb.simulated = false;
        capsuleCollider2D.enabled = false;
        animator.enabled = false;
        this.enabled = false;
    }
    #endregion

    #region Movement and Interaction Functions
    private void PlayerMovement()
    {
        // Handles player horizontal movement
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
    public void PickupKey()
    {
        SoundManager.Instance.PlayEffect(SoundType.ItemPickup);
        scoreController.IncreaseScore(10);
    }
    private void ReturntoIdle()
    {
        animator.SetTrigger("isIdle");
    }
    public void ReloadMenu()
    {
        gameOverController.ReloadMenu();
    }
    private void PlayerJump()
    {
        // Handles player jumping logic
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
        // Handles player crouching logic
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
        // Updates player facing direction based on horizontal movement
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
    private void GroundCheck()
    {
        // Verifies if player is on the ground
        if (Mathf.Abs(transform.position.y - lastCheckPositionY) > checkThreshold)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
            isGrounded = hit.collider != null;
            lastCheckPositionY = transform.position.y;
        }
    }
    #endregion
}