using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    // Variables to maintain the player's state during gameplay.
    private bool isFacingLeft = false; // Indicates if the player is facing left.
    private bool isCrouching = false; // Indicates if the player is crouching.
    private bool isGrounded = true; // Indicates if the player is on the ground.
    private bool isJumping = false; // Indicates if the player is in the process of jumping.
    private bool canDoubleJump = true; // Allows the player to perform a double jump.
    private bool isShielded = false; // Indicates if the player is currently shielded.
    private bool isSpeedBoosted = false; // Indicates if the player is under the effect of a speed boost.

    // Health and movement variables for managing gameplay mechanics.
    private int currentHealth = 0; // Current health of the player.
    private float horizontal = 0.0f; // Horizontal movement input value.
    private float xMovement = 0.0f; // Calculated horizontal movement.
    private float vertical = 0.0f; // Vertical movement input value.
    private float yMovement = 0.0f; // Calculated vertical movement for jumps.
    public int maxHealth = 3; // Maximum health a player can have.
    public float moveSpeed = 1.0f; // Movement speed of the player.
    public float jumpForce = 1.0f; // Force applied when the player jumps.

    // Time-related and size modification variables for effects.
    private float shieldTimeRemaining = 0; // Remaining duration of the shield.
    private float speedBoostTimeRemaining = 0; // Remaining duration of the speed boost.
    private float lastCheckPositionY; // Last y-position used for ground check.
    private float checkThreshold = 0.01f; // Threshold for change in position to reassess ground status.
    public float doubleJumpCooldown = 0.5f; // Cooldown time before another double jump is allowed.
    private float crouchingHeightRatio = 0.6f; // Height ratio when crouching.
    private float crouchingWidthSizeRatio = 0.7f; // Width ratio when crouching.
    private float crouchingWidthOffsetRatio = .025f; // Width offset when crouching.

    // Collider configurations for normal and modified collider states.
    private Vector2 colliderOffsetOriginal = Vector2.zero; // Initializing original collider offset.
    private Vector2 colliderSizeOriginal = Vector2.zero; // Initializing original collider size.

    // Layer and distance settings for detecting if the player is grounded.
    public LayerMask groundLayer; // Layer that determines what is considered ground.
    public float groundCheckDistance = 0.001f; // Distance to check for ground.

    // References to other components and controllers used in the game.
    public Animator animator; // Animator component for player animations.
    public ScoreController scoreController; // Reference to the score controller.
    public GameOverController gameOverController; // Reference to the game over controller.
    public ParticleSystemController particleSystemController; // Reference to particle system controller.

    private CapsuleCollider2D capsuleCollider2D; // Capsule collider component for player.
    private Rigidbody2D rb; // Rigidbody component for physics calculations.
    private SpriteRenderer spriteRenderer; // Sprite renderer for the player.
    #endregion

    #region Unity Functions
    private void Awake()
    {
        // Initialize components on object awake.
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // Setup initial conditions such as collider size and health.
        colliderSizeOriginal = capsuleCollider2D.size;
        colliderOffsetOriginal = capsuleCollider2D.offset;
        currentHealth = maxHealth; // Set health to max at the start of the game.
    }

    void Update()
    {
        // Update method called once per frame to handle player input and state.
        UpdateStates(); // Update player states such as shield and speed.
        PlayerMovement(); // Handle player movement.
        GroundCheck(); // Check if player is on the ground.
        PlayerJump(); // Handle player jumping.
        PlayerCrouch(); // Handle player crouching.
    }
    #endregion

    #region Player State Updates
    private void UpdateStates()
    {
        // Decrement timers and update states for shields and speed boosts.
        if (isShielded)
        {
            shieldTimeRemaining -= Time.deltaTime;
            if (shieldTimeRemaining <= 0)
            {
                isShielded = false; // Disable shield when time expires.
            }
        }

        if (isSpeedBoosted)
        {
            speedBoostTimeRemaining -= Time.deltaTime;
            if (speedBoostTimeRemaining <= 0)
            {
                moveSpeed /= 2; // Revert speed boost.
                isSpeedBoosted = false;
            }
        }
    }
    #endregion

    #region Player Actions
    public void ActivateShield()
    {
        // Activate the shield, setting its duration.
        isShielded = true;
        shieldTimeRemaining = 5.0f; // Shield active for 5 seconds.
    }

    public void ActivateSpeedBoost()
    {
        // Activate the speed boost, increasing player speed and setting duration.
        if (!isSpeedBoosted)
        {
            moveSpeed *= 2; // Double the movement speed.
        }
        isSpeedBoosted = true;
        speedBoostTimeRemaining = 5.0f; // Speed boost active for 5 seconds.
    }

    public int GetHealth()
    {
        // Return the current health of the player.
        return currentHealth;
    }

    public void DecreaseHealth()
    {
        // Decrease the player's health, handle damage and check for game over.
        if (isShielded)
            return; // No effect if shielded.

        if (currentHealth > 1)
        {
            currentHealth -= 1; // Decrease health by one.
            SoundManager.Instance.PlayEffect(SoundType.PlayerHurt); // Play hurt sound.
            animator.SetTrigger("isHurt"); // Trigger hurt animation.
            ActivateShield(); // Activate shield upon taking damage.
            ActivateSpeedBoost(); // Activate speed boost upon taking damage.
        }
        else
        {
            currentHealth -= 1; // Last health point.
            KillPlayer(); // Call to execute player death.
        }
    }

    public void KillPlayer()
    {
        // Handle player death: play sounds, animations, and manage game over sequence.
        SoundManager.Instance.PlayEffect(SoundType.PlayerDeath); // Play death sound.
        particleSystemController.PlayFailParticleEffect(); // Play failure effects.
        animator.SetTrigger("isDead"); // Trigger death animation.
        SoundManager.Instance.PlayEffect(SoundType.LevelFail); // Play level failure sound.
        StartCoroutine(KillPlayerWait()); // Start coroutine to handle post-death sequence.
    }

    private IEnumerator KillPlayerWait()
    {
        // Coroutine to wait after death before disabling player.
        yield return new WaitForSeconds(2f); // Wait for 2 seconds.
        DisablePlayer(); // Disable player components.
    }

    public void DisablePlayer()
    {
        // Disable all player components making player non-interactive.
        spriteRenderer.enabled = false; // Disable sprite rendering.
        rb.simulated = false; // Disable physics simulation.
        capsuleCollider2D.enabled = false; // Disable collider.
        animator.enabled = false; // Disable animations.
        this.enabled = false; // Disable this script.
    }
    #endregion

    #region Movement and Interaction Functions
    private void PlayerMovement()
    {
        // Calculate and apply horizontal player movement.
        horizontal = Input.GetAxisRaw("Horizontal"); // Get horizontal input.
        PlayerFacingDirection(); // Update the direction the player is facing.
        if (!isCrouching)
        {
            xMovement = horizontal * moveSpeed * Time.deltaTime; // Calculate movement.
            if (xMovement != 0.0f)
            {
                SoundManager.Instance.PlayEffect(SoundType.PlayerMove); // Play movement sound.
            }
            transform.position = new Vector3(transform.position.x + xMovement, transform.position.y, 0.0f); // Apply movement.
            animator.SetFloat("moveSpeed", Mathf.Abs(horizontal)); // Update animator with movement speed.
        }
    }

    public void PickupKey()
    {
        // Function to handle key pickup.
        SoundManager.Instance.PlayEffect(SoundType.ItemPickup); // Play pickup sound.
        scoreController.IncreaseScore(10); // Increase score by 10.
    }

    private void ReturntoIdle()
    {
        // Trigger return to idle animation.
        animator.SetTrigger("isIdle");
    }

    public void ReloadMenu()
    {
        // Call game over controller to reload the menu.
        gameOverController.ReloadMenu();
    }

    private void PlayerJump()
    {
        // Handle player jumping, including initial and double jumps.
        vertical = Input.GetAxisRaw("Vertical"); // Get vertical input for jumping.
        if (isGrounded && isJumping)
        {
            isJumping = false; // Reset jumping state when grounded.
            animator.SetBool("isJumping", false); // Update animator.
        }

        if (isGrounded && !isJumping)
        {
            canDoubleJump = true; // Reset double jump capability.
        }

        if (vertical > 0.0f)
        {
            if (isGrounded && !isJumping)
            {
                isJumping = true; // Set jumping state.
                PerformJump(); // Perform jump.
                animator.SetBool("isJumping", true); // Update animator.
            }
            else if (!isGrounded && canDoubleJump && isJumping)
            {
                isJumping = true; // Continue in jumping state.
                PerformJump(); // Perform double jump.
                StartCoroutine(DoubleJumpCooldown()); // Start double jump cooldown.
                animator.SetBool("isJumping", true); // Update animator.
            }
        }
    }

    private IEnumerator DoubleJumpCooldown()
    {
        // Coroutine to handle double jump cooldown.
        yield return new WaitForSeconds(doubleJumpCooldown); // Wait for cooldown period.
        canDoubleJump = false; // Disable double jumping.
    }

    private void PerformJump()
    {
        // Perform a jump applying force to the Rigidbody.
        rb.velocity = new Vector2(rb.velocity.x, 0f); // Reset vertical velocity.
        yMovement = vertical * jumpForce; // Calculate jump force.
        SoundManager.Instance.PlayEffect(SoundType.PlayerJump); // Play jump sound.
        rb.AddForce(new Vector2(0.0f, yMovement), ForceMode2D.Impulse); // Apply jump force.
    }

    private void PlayerCrouch()
    {
        // Toggle player crouching state and adjust collider.
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching; // Toggle crouching state.
            if (isCrouching)
            {
                capsuleCollider2D.size = new Vector2(capsuleCollider2D.size.x / crouchingWidthSizeRatio, capsuleCollider2D.size.y * crouchingHeightRatio); // Adjust collider size for crouching.
                capsuleCollider2D.offset = new Vector2(capsuleCollider2D.offset.x / crouchingWidthOffsetRatio, capsuleCollider2D.offset.y * crouchingHeightRatio); // Adjust collider offset for crouching.
            }
            else
            {
                capsuleCollider2D.size = colliderSizeOriginal; // Revert to original collider size.
                capsuleCollider2D.offset = colliderOffsetOriginal; // Revert to original collider offset.
            }
            animator.SetBool("isCrouching", isCrouching); // Update animator with crouching state.
        }
    }

    private void PlayerFacingDirection()
    {
        // Update the player's facing direction based on the horizontal input.
        if (horizontal < 0.0f)
        {
            if (!isFacingLeft)
            {
                transform.Rotate(0, 180, 0); // Rotate the sprite to face left.
            }
            isFacingLeft = true; // Set facing left.

        }

        else if (horizontal > 0.0f)
        {
            if (isFacingLeft)
            {
                transform.Rotate(0, 180, 0); // Rotate the sprite to face right.
            }
            isFacingLeft = false; // Set facing right.
        }
    }

    private void GroundCheck()
    {
        // Check if the player is currently on the ground.
        if (Mathf.Abs(transform.position.y - lastCheckPositionY) > checkThreshold)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer); // Perform a raycast downward to detect ground.
            isGrounded = hit.collider != null; // Set grounded state based on raycast.
            lastCheckPositionY = transform.position.y; // Update last checked position.
        }
    }
    #endregion
}