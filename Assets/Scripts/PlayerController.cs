using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    bool isFacingLeft = false;
    bool isCrouching = false;
    bool isGrounded = true;
    float horizontal = 0.0f;
    float xMovement = 0.0f;
    float vertical = 0.0f;
    float yMovement = 0.0f;
    float crouchingHeightRatio = 0.6f;
    float crouchingWidthSizeRatio = 0.7f;
    float crouchingWidthOffsetRatio = .025f;
    Vector2 colliderOffsetOriginal = Vector2.zero;
    Vector2 colliderSizeOriginal = Vector2.zero;

    public LayerMask groundLayer;
    public float groundCheckDistance = 0.001f;

    public Animator animator;
    private CapsuleCollider2D capsuleCollider2D;
    private Rigidbody2D rb;

    public float moveSpeed = 1.0f;
    public float jumpForce = 1.0f;


    void Start()
    {
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        colliderSizeOriginal = capsuleCollider2D.size;
        colliderOffsetOriginal = capsuleCollider2D.offset;
    }
    void Update()
    {
        PlayerMovement();
        PlayerCrouch();
    }

    private void PlayerMovement()
    {
        // Left and Right Movements
        horizontal = Input.GetAxisRaw("Horizontal");
        PlayerFacingDirection();
        xMovement =  horizontal * moveSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x + xMovement, transform.position.y, 0.0f);
        animator.SetFloat("moveSpeed", Mathf.Abs(horizontal));

        // Jump Movement
        isGrounded = GroundCheck();
        vertical = Input.GetAxisRaw("Vertical");
        yMovement = vertical * jumpForce;
        if (vertical  > 0.0f && isGrounded && !isCrouching)
        {
            rb.AddForce(new Vector2(0.0f, yMovement), ForceMode2D.Force);
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
        animator.SetFloat("jumpForce", vertical);
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
}
