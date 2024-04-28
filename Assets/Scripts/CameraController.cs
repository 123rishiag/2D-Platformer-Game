using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 relativePosition;
    public float moveSpeed = 5.0f;

    void LateUpdate()
    {
        // Calculate the camera's position relative to the player's position
        Vector3 targetPosition = player.position + relativePosition;

        // Smoothly move the camera towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
