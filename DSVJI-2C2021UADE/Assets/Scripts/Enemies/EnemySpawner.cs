using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Vector3 spawnPoint;
    
    
    public void SpawnEnemy()
    {
        var enemy = Instantiate(enemyPrefab, spawnPoint, transform.rotation);
        
    }
}
