using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Transform transformPlayer;
    [SerializeField] private Transform playerBallPosition;
 
    public bool IsAttached { get; private set; } = false;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        // Check the distance to the player
        if (!IsAttached && Vector3.Distance(transform.position, transformPlayer.position) < 0.5)
        {
            AttachBall();
        }
    }

    private void AttachBall()
    {
        // Attach the ball to the playerBallPosition
        this.transform.position = playerBallPosition.position;
        this.transform.parent = playerBallPosition;
        IsAttached = true;
        // Disable physics while attached if desired
        this.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void DetachBall()
    {
        // Detach the ball from the player
        this.transform.parent = null;
        IsAttached = false;

        // Re-enable physics
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }
        else
        {
            Debug.LogError("No Rigidbody found on the ball.");
        }
    }

    public void PrepareForKick()
    {
        // Make sure the ball is ready to be kicked
        rb.isKinematic = false;
    }

    public void Kick(Vector3 force)
    {
        // Apply the kicking force
        rb.AddForce(force, ForceMode.Impulse);
    }
}
