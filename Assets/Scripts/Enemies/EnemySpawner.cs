using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemySpawn;

    [SerializeField]
    private Transform spawnLocation;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MainCamera"))
        {
            Instantiate(enemySpawn).transform.position = spawnLocation.transform.position;
        }
    }
}
