using PlatformerFight.CharacterThings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemySpawn;

    [SerializeField]
    private Transform spawnLocation;

    public static bool enemyHoldScore = true;

    private void Awake()
    {
        enemyHoldScore = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("MainCamera"))
        {
            BaseEnemy enemy = Instantiate(enemySpawn).GetComponent<BaseEnemy>();
            enemy.transform.position = spawnLocation.transform.position;
            enemy.SetScorable(enemyHoldScore);
        }
    }
}
