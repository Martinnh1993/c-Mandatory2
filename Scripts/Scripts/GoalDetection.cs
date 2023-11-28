using UnityEngine;
using TMPro;
using System.Linq;

public class GoalDetection : MonoBehaviour
{
    public TextMeshProUGUI levelText; // Use this type for TextMeshPro elements
    private int currentLevel = 0; // Start at Level 0
    public GameObject goalKeeperPrefab; // Assign in inspector
     public GameObject[] opponentPrefabs;
    public GameObject player; // Assign the player GameObject in the inspector
    public GameObject ball;
    public AudioSource cheerAudioSource;
    public AudioClip cheerAudioClip;
    public GameObject gameOverImage;
    private Player playerScript;

    void Awake()
    {
        // Initialize the level text on awake to "Level 1"
        levelText.text = "Level " + currentLevel.ToString();
        gameOverImage.SetActive(false); // Hide the game over image initially
        playerScript = FindObjectOfType<Player>(); // Find the Player script in the scene
        // Load all GameObjects from a specific folder within Resources
        GameObject[] allPrefabs = Resources.LoadAll<GameObject>("OpponentPrefabs");

        // Filter the loaded GameObjects by the tag "Opponent"
        opponentPrefabs = allPrefabs.Where(prefab => prefab.tag == "Opponent").ToArray();

    }

    private void Update()
    {
        // Check if the death count has reached the maximum allowed deaths
        if (playerScript.DeathCount >= playerScript.MaxDeaths)
        {
            gameOverImage.SetActive(true); // Show the game over image
            Time.timeScale = 0; // Stop the game by setting the time scale to zero
            // Optionally, disable any player input or other gameplay elements here
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball")) // Make sure the ball has the tag "Ball"
        {
            cheerAudioSource.PlayOneShot(cheerAudioClip);
            currentLevel++; // Increment the level
            levelText.text = "Level " + currentLevel.ToString(); // Update the level text
            UpdateLevel(currentLevel); // Update the level, which includes spawning NPCs
            ResetPlayerAndBall(); // Reset player and ball positions
        }
    }


    private void UpdateLevel(int level)
    {
        ClearNPCs(); // Clear any existing NPCs.
        levelText.text = "Level " + level.ToString(); // Update the level text display.

        // Always spawn the goalkeeper first.
        SpawnGoalKeeper();

        // If the level is greater than 1, spawn additional opponents.
        if (level > 1)
        {
            SpawnOpponents(level - 1); // Subtracting one because the goalkeeper is already considered one NPC.
        }
        // Additional logic for when the level updates can be included here if necessary.
    }

    private void SpawnGoalKeeper()
    {
        // Define the goalkeeper's specific spawn position here
        Vector3 goalkeeperPosition = new Vector3(0f, 0.8f, 23.5f); // Replace with the actual goalkeeper position
        Quaternion goalKepperRotation = Quaternion.Euler(0f, 180f, 0f); 
        Instantiate(goalKeeperPrefab, goalkeeperPosition, goalKepperRotation);
    }

    private void SpawnOpponents(int numberOfOpponents)
    {
        for (int i = 0; i < numberOfOpponents; i++)
        {
            Vector3 opponentPosition = GetRandomPosition();
            Quaternion opponentRotation = Quaternion.Euler(0f, 180f, 0f); // Rotate 180 degrees around the y-axis

            // Choose a random prefab from the array
            GameObject chosenPrefab = opponentPrefabs[Random.Range(0, opponentPrefabs.Length)];
            
            // Instantiate the random opponent prefab
            Instantiate(chosenPrefab, opponentPosition, opponentRotation);
        }
    }


    private Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(-6.5f, 6.5f);
        float randomZ = Random.Range(0f, 10f); // Adjust the 10f to whatever max z value you desire
        return new Vector3(randomX, 0.8f, randomZ);
    }

    private void ClearNPCs()
    {
        // Find and destroy all NPCs tagged as "Opponent"
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("Opponent");
        foreach (var npc in npcs)
        {
            Destroy(npc);
        }
        // Optionally, add a specific tag for the goalkeeper if you want to keep it between levels
    }

    private void ResetPlayerAndBall()
    {
        // Disable the Character Controller before moving the player, if it exists
        CharacterController controller = player.GetComponent<CharacterController>();
        if (controller != null)
        {
            controller.enabled = false;
        }

        // Generate a random position within the specified ranges for the player
        Vector3 randomPlayerPosition = new Vector3(
            Random.Range(-6f, 6f), // x between -6 and 6
            1f, // y is always 1
            Random.Range(-20f, -10f) // z between -20 and -10
        );
        
        // Set the player's position to the random position
        player.transform.position = randomPlayerPosition;

        // Re-enable the Character Controller after the player has been moved
        if (controller != null)
        {
            controller.enabled = true;
        }

        // Generate a random position for the ball
        Vector3 randomBallPosition = new Vector3(
            Random.Range(-6f, 6f), // x between -6 and 6
            1f, // y is always 1
            Random.Range(-12f, -6f) // z between -10 and -5
        );
        
        // Reset the ball's position to the random position
        ball.transform.position = randomBallPosition;

        // Reset the ball's rotation and physics
        ResetBall();
    }

    private void ResetBall()
    {
        // Reset the ball's rotation to default
        ball.transform.rotation = Quaternion.identity;

        // Reset the ball's physics if it has a Rigidbody
        Rigidbody ballRb = ball.GetComponent<Rigidbody>();
        if (ballRb != null)
        {
            ballRb.velocity = Vector3.zero;
            ballRb.angularVelocity = Vector3.zero;
        }
    }






}
