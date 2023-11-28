using StarterAssets;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Ball ball; // Reference to the Ball script
    public Transform ballPosition; // The transform where the ball should stay relative to the player
    public float shootingForce = 20f; // Modify this value to control the shooting strength
    public float pickupDistance = 1f;

    // Reference to the input system to detect the shoot command
    private StarterAssetsInputs playerInput;
    // Add a public field for respawn position if you want to set this from the inspector
    public Vector3 respawnPosition = new Vector3(0f, 1f, -15f);
    public float proximityThreshold = 0.1f;
    public AudioSource kickAudioSource;
    public AudioClip kickAudioClip;
    public int DeathCount { get; private set; } = 0;
    public int MaxDeaths = 5; // You can set this in the inspector if you like


    void Awake()
    {
        playerInput = GetComponent<StarterAssetsInputs>();
        if (playerInput == null)
        {
            Debug.LogError("StarterAssetsInputs component not found on the player GameObject.");
        }
    }

    void Update()
    {
        GameObject[] opponents = GameObject.FindGameObjectsWithTag("Opponent");
        foreach (GameObject opponent in opponents)
        {
            float distanceToOpponent = Vector3.Distance(transform.position, opponent.transform.position);

            if (distanceToOpponent < proximityThreshold)
            {
                RespawnPlayer();
                break;
            }
        }

        // Check the distance between the player and the ball to pick it up
        if (!ball.HasBall && Vector3.Distance(transform.position, ball.transform.position) <= pickupDistance)
        {
            PickupBall();
        }

        // If the shoot button is pressed and the player has the ball, shoot it
        if (playerInput.shoot && ball.HasBall)
        {
            ShootBall();
            playerInput.shoot = false; // Reset the input to prevent continuous shooting
        }
    }
    private void PickupBall()
    {
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        ballRigidbody.velocity = Vector3.zero; // Clear any existing movement
        ballRigidbody.angularVelocity = Vector3.zero; // Clear any existing rotation
        ball.HasBall = true;
        ball.transform.SetParent(ballPosition);
        ball.transform.localPosition = Vector3.zero; // Reset local position
        ballRigidbody.isKinematic = true; // Disable physics while the ball is held
    }

    private void ShootBall()
    {
        kickAudioSource.PlayOneShot(kickAudioClip);
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        ball.HasBall = false;
        ball.transform.SetParent(null); // Detach the ball from the player
        ballRigidbody.isKinematic = false; // Re-enable physics

        // Separate the forward force and the upward force for better control
        Vector3 forwardForce = transform.forward * shootingForce;
        Vector3 upwardForce = Vector3.up * (shootingForce * 0.1f); // Reduced upward force

        // Apply the combined force to the ball
        ballRigidbody.AddForce(forwardForce + upwardForce, ForceMode.Impulse); // Shoot the ball
    }

    private void RespawnPlayer()
    {
        transform.position = respawnPosition;
        // Add any additional logic needed for respawning the player
        DeathCount++;
    }


}
