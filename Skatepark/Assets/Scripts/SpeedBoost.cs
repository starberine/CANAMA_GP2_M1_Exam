using UnityEngine;
using System.Collections;

public class SpeedBoost : MonoBehaviour
{
    public float boostAmount = 12.0f; // Speed boost amount
    public float boostDuration = 2.0f; // How long the boost lasts (2 seconds)
    public float respawnTime = 3.0f; // Time before the power-up respawns
    public Transform[] spawnPoints; // List of spawn points for the power-up

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                StartCoroutine(playerMovement.ActivateSpeedBoost(boostAmount, boostDuration));
            }

            // Start the respawn process
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        // Hide the power-up
        Debug.Log("Power-up collected. Starting respawn process.");
        gameObject.SetActive(false);

        // Wait for respawn time
        yield return new WaitForSeconds(respawnTime);

        // Respawn at a random location
        if (spawnPoints.Length > 0)
        {
            Transform randomSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            transform.position = randomSpawn.position;
            Debug.Log("Power-up respawned at: " + randomSpawn.position);
        }
        else
        {
            Debug.LogWarning("No spawn points available for respawning.");
        }

        // Reactivate the power-up
        gameObject.SetActive(true);
        Debug.Log("Power-up is now active.");
    }
}
