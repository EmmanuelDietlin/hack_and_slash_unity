using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class EnemySpawnManager : MonoBehaviour
{
    private GameObject[] enemySpawners;
    private int[] spawnNumber;
    private int enemiesKilled;
    private int enemiesSpawned;
    private int waveNumber;
    private bool finishedSpawn;
    private System.Random random;
    private float spawnTimer;
    private GameObject boss;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private AudioClip bossMusic;

    public event EventHandler victory;
    public event EventHandler<NewWaveEventArgs> newWave;
    public event EventHandler<BossSpawnEventArgs> bossSpawn;

    [SerializeField] private int bossWave = 10;

    public void OnBossSpawn(BossSpawnEventArgs e)
    {
        EventHandler<BossSpawnEventArgs> handler = bossSpawn;
        handler?.Invoke(this, e);
    }

    public void OnVictory(EventArgs e)
    {
        EventHandler handler = victory;
        handler?.Invoke(this, e);
    }

    public void OnNewWave(NewWaveEventArgs e)
    {
        EventHandler<NewWaveEventArgs> handler = newWave;
        handler?.Invoke(this, e);
    }

    private void Awake()
    {
        spawnNumber = new int[] { 1, 1, 2, 3, 5, 8, 13, 21, 34, 55 };
        enemiesSpawned = 0;
        enemiesKilled = 0;
        waveNumber = 1;
        finishedSpawn = false;
        spawnTimer = 1f;
        random = new System.Random();
        enemySpawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
    }

    private void Update()
    {
        SpawnEnemies();
    }

    public void enemyKilled()
    {
        enemiesKilled += 1;
    }



    private void SpawnEnemies()
    {
        if (!finishedSpawn)
        {
            //Debug.Log(waveNumber);
            if (enemiesSpawned < spawnNumber[waveNumber - 1])
            {
                if (spawnTimer >= 1f)
                {
                    int pos = random.Next(0, enemySpawners.Length);
                    enemySpawners[pos].GetComponent<EnemySpawn>().SpawnEnemy();
                    enemiesSpawned += 1;
                    spawnTimer = 0f;
                }
                else
                {
                    spawnTimer += Time.deltaTime;
                }
            }
            else
            {
                finishedSpawn = true;
            }
        }
        else if (enemiesKilled >= enemiesSpawned)
        {
            if (!boss)
            {
                waveNumber += 1;
                finishedSpawn = false;
                enemiesSpawned = 0;
                enemiesKilled = 0;
                GameObject[] summons = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject summon in summons)
                {
                    Destroy(summon);
                    // Debug.Log("summon destroyed");
                }

                NewWaveEventArgs args = new NewWaveEventArgs();
                args.waveNumber = waveNumber;
                OnNewWave(args);

                if (waveNumber == bossWave)
                {
                    boss = Instantiate(bossPrefab, gameObject.transform);
                    BossSpawnEventArgs e = new BossSpawnEventArgs();
                    e.boss = boss;
                    OnBossSpawn(e);
                    Camera.main.GetComponent<AudioSource>().clip = bossMusic;
                    Camera.main.GetComponent<AudioSource>().loop = true;
                    Camera.main.GetComponent<AudioSource>().Play();
                }

                if (waveNumber == bossWave+1)
                {
                    // Debug.Log("Victory ! ");
                    OnVictory(new EventArgs());
                    gameObject.SetActive(false);

                }
            }
        }
        //Debug.Log("Enemies spawned = " + enemiesSpawned);
        //Debug.Log("Enemies killed = " + enemiesKilled);
    }
}
