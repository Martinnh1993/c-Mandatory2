using UnityEngine;

public class Ball : MonoBehaviour
{
    public Transform ballPosition; // Assign this to a transform that indicates where the ball should be relative to the player
    public bool HasBall { get; set; } // Allow Player.cs to change this property
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    } 
    void FixedUpdate()
    {
        // Keep the ball at the ballPosition if it's a child (if the player has the ball)
        if (this.transform.parent == ballPosition)
        {
            this.transform.position = ballPosition.position;
        }
    }

    public void AttachToPlayer()
    {
        rb.isKinematic = true;
        this.transform.position = ballPosition.position; // Set the ball's position to the ballPosition
        this.transform.SetParent(ballPosition); // Attach the ball to the ballPosition
    }

    public void Shoot(Vector3 direction, float force)
    {
        if (HasBall) // Only shoot if the player has the ball
        {
            HasBall = false; // The player no longer has the ball
            transform.SetParent(null); // Detach the ball from the player
            rb.isKinematic = false; // Re-enable physics
            rb.AddForce(direction * force, ForceMode.Impulse); // Apply the shooting force
        }
    }

   
}
