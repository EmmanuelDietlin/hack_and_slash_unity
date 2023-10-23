using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UI : MonoBehaviour
{
    private PlayerSpawn playerSpawner;
    [SerializeField] private Image playerLifeBar;
    private GameObject player;
    [SerializeField] private GameObject time;
    private float elapsedTime;
    [SerializeField] private GameObject waveNumber;
    private EnemySpawnManager enemySpawnManager;
    [SerializeField] private GameObject warriorIcons;
    [SerializeField] private GameObject mageIcons;
    [SerializeField] private GameObject rogueIcons;
    [SerializeField] private GameObject bossLifeBarContainer;
    [SerializeField] private Image bossLifeBar;

    private Image attack1Icon;
    private Image attack2Icon;
    private Image attack3Icon;
    private GameObject boss;




    private void Awake()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("MainVolume");
        playerSpawner = GameObject.FindGameObjectWithTag("PlayerSpawner").GetComponent<PlayerSpawn>();
        playerSpawner.SpawnPlayer();
        enemySpawnManager = GameObject.Find("EnemySpawnManager").GetComponent<EnemySpawnManager>();
        enemySpawnManager.newWave += UpdateWaveNumber;
        enemySpawnManager.bossSpawn += BossSpawned;
        elapsedTime = 0f;
        findPlayer();
        playerLifeBar.fillAmount = player.GetComponent<DamageManager>().HPRemainingRatio();
        Camera.main.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");

        attack1Icon = GameObject.Find("Attack1Icon").GetComponent<Image>();
        attack2Icon = GameObject.Find("Attack2Icon").GetComponent<Image>();
        attack3Icon = GameObject.Find("Attack3Icon").GetComponent<Image>();


    }

    private void Update()
    {
        updateTime();
        findPlayer();
        UpdateIcons();
        UpdateBossLifeBar();

    }


    private void findPlayer()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player)
            {
                player.GetComponent<DamageManager>().damageTaken += updateLifeBar;
                player.GetComponent<DamageManager>().updatePlayerLifeBar += updateLifeBar;
                LoadIcons();
            }
        }
        if (!player) return;
    }

    private void updateLifeBar(object sender, EventArgs e)
    {
        playerLifeBar.fillAmount = player.GetComponent<DamageManager>().HPRemainingRatio();
    }

    private void updateTime()
    {
        int hour = (int)(elapsedTime / 3600);
        int min = (int)((elapsedTime - hour * 3600) / 60);
        int sec = (int)((elapsedTime - hour * 3600 - min * 60));
        time.GetComponent<TextMeshProUGUI>().text = hour.ToString() + ":" + min.ToString() + ":" + sec.ToString();
        elapsedTime += Time.deltaTime;
    }

    public void UpdateWaveNumber(object sender, EventArgs e)
    {
        try
        {
            int result = Int32.Parse(waveNumber.GetComponent<TextMeshProUGUI>().text);
            waveNumber.GetComponent<TextMeshProUGUI>().text = (result + 1).ToString();
        }
        catch (FormatException)
        {
            Debug.Log("Unable to parse");
        }
    }

    public string GetTime()
    {
        return time.GetComponent<TextMeshProUGUI>().text;
    }

    private void LoadIcons()
    {
        if (player.name.Contains("Warrior"))
        {
            warriorIcons.SetActive(true);
            player.GetComponent<WarriorBehavior>().attack1 += updateIcon1;
            player.GetComponent<WarriorBehavior>().attack2 += updateIcon2;
            player.GetComponent<WarriorBehavior>().attack3 += updateIcon3;
        }
        if (player.name.Contains("Mage"))
        {
            mageIcons.SetActive(true);
            player.GetComponent<MageBehavior>().attack1 += updateIcon1;
            player.GetComponent<MageBehavior>().attack2 += updateIcon2;
            player.GetComponent<MageBehavior>().attack3 += updateIcon3;
        }
        if (player.name.Contains("Rogue"))
        {
            rogueIcons.SetActive(true);
            player.GetComponent<RogueBehavior>().attack1 += updateIcon1;
            player.GetComponent<RogueBehavior>().attack2 += updateIcon2;
            player.GetComponent<RogueBehavior>().attack3 += updateIcon3;
        }
    }

    private void updateIcon1(object sender, EventArgs e)
    {
        attack1Icon.fillAmount = 0;
    }
    private void updateIcon2(object sender, EventArgs e)
    {
        attack2Icon.fillAmount = 0;
    }
    private void updateIcon3(object sender, EventArgs e)
    {

        attack3Icon.fillAmount = 0;
    }

    private void UpdateIcons()
    {
        if (!player) return;
        if (player.name.Contains("Warrior"))
        {
            attack1Icon.fillAmount = Mathf.Min(1, player.GetComponent<WarriorBehavior>().GetAttacksTimer()[0] / player.GetComponent<WarriorBehavior>().GetAttacksCooldown()[0]);
            attack2Icon.fillAmount = Mathf.Min(1, player.GetComponent<WarriorBehavior>().GetAttacksTimer()[1] / player.GetComponent<WarriorBehavior>().GetAttacksCooldown()[1]);
            attack3Icon.fillAmount = Mathf.Min(1, player.GetComponent<WarriorBehavior>().GetAttacksTimer()[2] / player.GetComponent<WarriorBehavior>().GetAttacksCooldown()[2]);
        }
        if (player.name.Contains("Mage"))
        {

            attack1Icon.fillAmount = Mathf.Min(1, player.GetComponent<MageBehavior>().GetAttacksTimer()[0] / player.GetComponent<MageBehavior>().GetAttacksCooldown()[0]);
            attack2Icon.fillAmount = Mathf.Min(1, player.GetComponent<MageBehavior>().GetAttacksTimer()[1] / player.GetComponent<MageBehavior>().GetAttacksCooldown()[1]);
            attack3Icon.fillAmount = Mathf.Min(1, player.GetComponent<MageBehavior>().GetAttacksTimer()[2] / player.GetComponent<MageBehavior>().GetAttacksCooldown()[2]);
        }
        if (player.name.Contains("Rogue"))
        {
            attack1Icon.fillAmount = Mathf.Min(1, player.GetComponent<RogueBehavior>().GetAttacksTimer()[0] / player.GetComponent<RogueBehavior>().GetAttacksCooldown()[0]);
            attack2Icon.fillAmount = Mathf.Min(1, player.GetComponent<RogueBehavior>().GetAttacksTimer()[1] / player.GetComponent<RogueBehavior>().GetAttacksCooldown()[1]);
            attack3Icon.fillAmount = Mathf.Min(1, player.GetComponent<RogueBehavior>().GetAttacksTimer()[2] / player.GetComponent<RogueBehavior>().GetAttacksCooldown()[2]);
        }


    }

    private void BossSpawned(object sender, BossSpawnEventArgs e)
    {
        boss = e.boss;
        // Debug.Log("boss spawn");
        bossLifeBarContainer.SetActive(true);
    }

    private void UpdateBossLifeBar()
    {
        if (boss)
        {
            bossLifeBar.fillAmount = boss.GetComponent<DamageManager>().HPRemainingRatio();
        }
    }











}
