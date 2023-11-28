using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalkeeperMovement : MonoBehaviour
{
    public Transform ball; // No need to assign in the inspector if you use Start to find it
    public float speed = 2f;
    public float minX = -2f;
    public float maxX = 2f;

    void Start()
    {
        // Find and assign the ball's transform at the start of the game
        ball = GameObject.FindGameObjectWithTag("Ball").transform;
    }
    private void Update()
    {
        if (ball != null)
        {
            // Determine the target x position based on the ball's x, but clamped within the goal width
            float targetX = Mathf.Clamp(ball.position.x, minX, maxX);

            // Move the goalkeeper towards the target x position
            Vector3 currentPosition = transform.position;
            Vector3 targetPosition = new Vector3(targetX, currentPosition.y, currentPosition.z);
            transform.position = Vector3.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);
        }
    }
}

