using UnityEngine;

public class OpponentMovement : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed of the opponent's movement
    public float activationDistance = 10f; // Distance at which the opponent starts moving towards the player
    private float initialZ; // The initial z-axis position
    private Transform player; // The player's transform

    private void Start()
    {
        initialZ = transform.position.z;
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player by tag
    }

    private void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(new Vector3(transform.position.x, 0f, transform.position.z), 
                                                    new Vector3(player.position.x, 0f, player.position.z));

            if (distanceToPlayer < activationDistance)
            {
                // Calculate the target x position based on the player's x position
                float targetX = Mathf.Clamp(player.position.x, -6f, 6f); // Assuming the x-range is between -6 and 6

                // Calculate the target z position based on the player's z position, but clamp it within the range
                float targetZ = Mathf.Clamp(player.position.z, initialZ - 1f, initialZ + 1f);

                // Determine the new target position
                Vector3 targetPosition = new Vector3(targetX, transform.position.y, targetZ);

                // Move the opponent towards the target position
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
        }
    }
}
