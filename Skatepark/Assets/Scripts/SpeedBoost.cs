using UnityEngine;
using System.Collections;

public class SpeedBoost : MonoBehaviour
{
    public float boostAmount = 12.0f; 
    public float boostDuration = 2.0f; 
    public float respawnTime = 3.0f; 
    public Transform[] spawnPoints; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                StartCoroutine(playerMovement.ActivateSpeedBoost(boostAmount, boostDuration));
            }

            
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        
        Debug.Log("Power-up collected. Starting respawn process.");
        gameObject.SetActive(false);

        
        yield return new WaitForSeconds(respawnTime);

        
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

        
        gameObject.SetActive(true);
        Debug.Log("Power-up is now active.");
    }
}
