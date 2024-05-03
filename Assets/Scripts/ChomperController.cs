using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChomperController : MonoBehaviour 
{
    public float patrolDistance = 4f;
    public float moveSpeed = 2f;

    private Vector2 originalPosition;

    private bool isMovingRight = true;

    private float directionFactor;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        // Calculate the left and right patrol points based on the original position
        float leftPoint = originalPosition.x - patrolDistance / 2f;
        float rightPoint = originalPosition.x + patrolDistance / 2f;

        directionFactor = isMovingRight ? 1 : -1;

        if (isMovingRight)
        {
            transform.Translate(Vector2.right * directionFactor * moveSpeed * Time.deltaTime);
            if (transform.position.x >= rightPoint)
            {
                isMovingRight = false;
                transform.Rotate(0, 180, 0);
            }
        }
        else
        {
            transform.Translate(Vector2.left * directionFactor * moveSpeed * Time.deltaTime);
            if (transform.position.x <= leftPoint)
            {
                isMovingRight = true;
                transform.Rotate(0, 180, 0);
            }
        }
            animator.SetFloat("moveSpeed",moveSpeed);
    }
}
