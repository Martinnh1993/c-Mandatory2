using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using StarterAssets;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public StarterAssetsInputs starterAssetsInputs;
    [SerializeField] private AudioClip kickSound;
    [SerializeField] private Ball ball;
    private Animator animator;
    private readonly int KICK_LAYER = 1;
    private float kickForce = 20f;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Null checks to ensure references are set
        if (animator == null) Debug.LogError("Animator component not found on the player.");
        if (audioSource == null) Debug.LogError("AudioSource component not found on the player.");
        if (kickSound == null) Debug.LogError("Kick sound clip not assigned.");
    }

  void Update()
{
    if (starterAssetsInputs.shoot && ball.IsAttached)
    {
        // If you want to use a trigger, remove the direct Play call and ensure the trigger is set up in the Animator.
        animator.SetTrigger("shoot");

        // If the layer weight should be changed only once when the condition is first met, move this out of Update or guard it with a boolean.
        // Also, ensure that the index '1' actually corresponds to the 'Shoot' layer.
        animator.SetLayerWeight(1, 1f);
    }
}



    public void AttemptShoot()
    {
        // Only attempt the shoot if the 'shoot' trigger exists in the Animator
        if (animator != null && Animator.StringToHash("shoot") != 0)
        {
            animator.SetTrigger("shoot");
        }

        // Only play the sound if the AudioClip is not null
        if (audioSource != null && kickSound != null)
        {
            audioSource.PlayOneShot(kickSound);
        }
    }

    public void OnShootEnd()
    {
        // Apply force to the ball at the end of the shoot animation
        if (ball != null)
        {
            Vector3 force = transform.forward * kickForce;
            ball.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        }
    }
}
