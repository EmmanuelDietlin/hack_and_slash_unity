using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    private GameObject playerType;
    private GameObject EnemySpawnManager;
    private GameObject player;
    private void Awake()
    {
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("PlayerSpawner");
        if (spawners.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);
        EnemySpawnManager = GameObject.Find("EnemySpawnManager");
        if (!EnemySpawnManager)
        {
            return;
        }
        EnemySpawnManager.GetComponent<EnemySpawnManager>().newWave += ResetPlayer;
    }

    private void Update()
    {
        if (!EnemySpawnManager)
        {
            EnemySpawnManager = GameObject.Find("EnemySpawnManager");
            if (!EnemySpawnManager)
            {
                return;
            }
            EnemySpawnManager.GetComponent<EnemySpawnManager>().newWave += ResetPlayer;
        }
    }

    public void ResetPlayer(object sender, NewWaveEventArgs e)
    {
        player.transform.position = gameObject.transform.position;
        // Debug.Log("Player reset");
        player.GetComponent<DamageManager>().ResetHP();
    }

    public void SpawnPlayer()
    {
        player = Instantiate(playerType, transform);
    }

    public void SetPlayerType(GameObject type)
    {
        playerType = type;
    }
}
