using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject soldierPrefab;
    [SerializeField] private GameObject archerPrefab;
    [SerializeField] private GameObject summonerPrefab;
    private GameObject[] enemyPrefabs;

    private void Start()
    {
        enemyPrefabs = new GameObject[] { soldierPrefab, archerPrefab, summonerPrefab };
    }

    private void Update()
    {
        
    }

    public void SpawnEnemy()
    {
        System.Random random = new System.Random();
        int randomEnemy = random.Next(0, 3);
        Instantiate(enemyPrefabs[randomEnemy], transform);

    }


    
}
