using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float moveSpeed = 0.0f;
    public Animator animator;
    bool isFacingLeft = false;
    // Update is called once per frame
    void Update()
    {
        moveSpeed = Input.GetAxisRaw("Horizontal");

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
}
