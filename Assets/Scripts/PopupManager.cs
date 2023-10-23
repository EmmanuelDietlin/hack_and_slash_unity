using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class PopupManager : MonoBehaviour
{
    [SerializeField] private GameObject defeatPopup;
    [SerializeField] private GameObject victoryPopup;
    [SerializeField] private GameObject wavePopup;
    [SerializeField] private GameObject defeatWave;
    [SerializeField] private GameObject defeatTime;
    private GameObject player;
    private GameObject enemySpawnManager;
    private float wavePopupTimer;
    private int currentWave;

    private AudioSource audio;
    [SerializeField] private AudioClip sirene;

    private void Awake()
    {
        currentWave = 1;
        enemySpawnManager = GameObject.Find("EnemySpawnManager");
        enemySpawnManager.GetComponent<EnemySpawnManager>().victory += VictoryPopup;
        enemySpawnManager.GetComponent<EnemySpawnManager>().newWave += NewWavePopup;
        player = GameObject.FindGameObjectWithTag("Player");
        if (!player) return;
        player.GetComponent<DamageManager>().playerDie += DefeatPopup;
        audio = GetComponent<AudioSource>();
        audio.volume = PlayerPrefs.GetFloat("EffectVolume");
    }

    private void Update()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (!player)
            {
                return;
            }
            player.GetComponent<DamageManager>().playerDie += DefeatPopup;
        }
        WavePopup();
    }

    public void NewWavePopup(object sender, NewWaveEventArgs e)
    {
        wavePopup.SetActive(true);
        currentWave = e.waveNumber;
        wavePopupTimer = 0f;
    }

    private void DefeatPopup(object sender, EventArgs e)
    {
        defeatPopup.SetActive(true);
        // Debug.Log(gameObject.GetComponent<UI>().GetTime());
        UpdatePlayerWaves(currentWave);
        defeatTime.GetComponent<TextMeshProUGUI>().text = gameObject.GetComponent<UI>().GetTime();
        defeatWave.GetComponent<TextMeshProUGUI>().text = currentWave.ToString();
        StartCoroutine(Defeat());



    }
    private IEnumerator Defeat()
    {
        yield return new WaitForSeconds(4);
        if (player)
        {
            Destroy(player);
        }
        GameObject playerSpawner = GameObject.FindGameObjectWithTag("PlayerSpawner");
        if (playerSpawner)
        {
            Destroy(playerSpawner);
        }
        yield return SceneManager.LoadSceneAsync("MainMenu");
    }

    private void VictoryPopup(object sender, EventArgs e)
    {
        victoryPopup.SetActive(true);
        // Debug.Log(gameObject.GetComponent<UI>().GetTime());
        UpdatePlayerWaves(10);
        StartCoroutine(Victory());
        victoryPopup.GetComponentInChildren<TextMeshProUGUI>().text = gameObject.GetComponent<UI>().GetTime();

    }
    private IEnumerator Victory()
    {
        yield return new WaitForSeconds(4);
        if (player)
        {
            Destroy(player);
        }
        GameObject playerSpawner = GameObject.FindGameObjectWithTag("PlayerSpawner");
        if (playerSpawner)
        {
            Destroy(playerSpawner);
        }
        yield return SceneManager.LoadSceneAsync("MainMenu");
    }

    private void WavePopup()
    {
        if (wavePopup.activeSelf == true)
        {
            if (wavePopupTimer < 2f)
            {
                wavePopupTimer += Time.deltaTime;
            }
            else
            {
                wavePopup.SetActive(false);
            }
        }
    }

    private void UpdatePlayerWaves(int waveNumber)
    {
        // Debug.Log(waveNumber);
        /*if (PlayerPrefs.GetInt(player.name + "Waves") < waveNumber)
        {
            PlayerPrefs.SetInt(player.name + "Waves", waveNumber);
        }*/
        if (player.name.Contains("Warrior") && PlayerPrefs.GetInt("WarriorWaves") < waveNumber) PlayerPrefs.SetInt("WarriorWaves", waveNumber);
        if (player.name.Contains("Mage") && PlayerPrefs.GetInt("MageWaves") < waveNumber) PlayerPrefs.SetInt("MageWaves", waveNumber);
        if (player.name.Contains("Rogue") && PlayerPrefs.GetInt("RogueWaves") < waveNumber) PlayerPrefs.SetInt("RogueWaves", waveNumber);
    }
}
