using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    bool isFacingLeft = false;
    bool isCrouching = false;
    bool isGrounded = true;
    float moveSpeed = 0.0f;
    float jumpForce = 0.0f;
    float crouchingHeightRatio = 0.6f;
    float crouchingWidthSizeRatio = 0.7f;
    float crouchingWidthOffsetRatio = .025f;
    Vector2 colliderOffsetOriginal = Vector2.zero;
    Vector2 colliderSizeOriginal = Vector2.zero;

    float jumpingHeightRatio = 0.7f;
    float afterJumpWaitInSeconds = 0.4f;

    public LayerMask groundLayer;
    public float groundCheckDistance = 0.1f;

    public Animator animator;
    private CapsuleCollider2D capsuleCollider2D;


    void Start()
    {
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        colliderSizeOriginal = capsuleCollider2D.size;
        colliderOffsetOriginal = capsuleCollider2D.offset;
    }
    void Update()
    {
        moveSpeed = Input.GetAxisRaw("Horizontal");

        isGrounded = GroundCheck();

        if (Input.GetAxisRaw("Vertical") > 0.0f && isGrounded && !isCrouching && jumpForce == 0.0f)
        {
            jumpForce = afterJumpWaitInSeconds;
            capsuleCollider2D.transform.position = new Vector3(capsuleCollider2D.transform.position.x, capsuleCollider2D.transform.position.y + jumpingHeightRatio, 0.0f);
        }
        jumpForce -= Time.deltaTime;
        if (jumpForce < 0.0f && !isCrouching)
        {
            jumpForce = 0.0f;
        }
    
        animator.SetFloat("jumpForce", jumpForce);       

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;
            if (isCrouching)
            {
                capsuleCollider2D.size = new Vector2(capsuleCollider2D.size.x / crouchingWidthSizeRatio, capsuleCollider2D.size.y * crouchingHeightRatio);
                capsuleCollider2D.offset = new Vector2(capsuleCollider2D.offset.x / crouchingWidthOffsetRatio, capsuleCollider2D.offset.y * crouchingHeightRatio);
            }
            else {
                capsuleCollider2D.size = colliderSizeOriginal;
                capsuleCollider2D.offset = colliderOffsetOriginal;
            }
            animator.SetBool("isCrouching", isCrouching);
        }


        if (moveSpeed < 0.0f)
        {
            moveSpeed *= -1f;
            if (!isFacingLeft)
            {
                transform.Rotate(0, 180, 0);
            }
            isFacingLeft = true;
           
        }

        else if (moveSpeed > 0.0f)
        {
            if (isFacingLeft)
            {
                transform.Rotate(0, 180, 0);
            }
            isFacingLeft = false;
        }

        animator.SetFloat("moveSpeed", moveSpeed);

    }

    bool GroundCheck()
    {
        // Cast a ray downward to check for ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);

        // If the ray hits something on the ground layer, return true
        return hit.collider != null;
    }
}
