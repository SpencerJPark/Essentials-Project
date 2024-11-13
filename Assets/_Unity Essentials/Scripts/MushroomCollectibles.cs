using UnityEngine;

public class MushroomCollectibles : MonoBehaviour
{
    public float rotationSpeed = 0.5f;
    public GameObject onCollectEffect; // Particle effect for collection

    private GameManager gameManager; // Reference to the GameManager

    private void Start()
    {
        // Find the GameManager in the scene
        gameManager = Object.FindFirstObjectByType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene. Please ensure there is a GameManager object.");
        }
    }

    private void Update()
    {
        // Rotate the mushroom collectible for a visual effect
        transform.Rotate(0, rotationSpeed, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player collected a mushroom.");

            // Notify GameManager about the collection, which will play the sound
            if (gameManager != null)
            {
                gameManager.CollectMushroom();
            }

            // Instantiate the particle effect on collection
            Instantiate(onCollectEffect, transform.position, transform.rotation);

            // Destroy the collectible immediately after collection
            Destroy(gameObject);
        }
    }
}
