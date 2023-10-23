using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject selectMenu;
    [SerializeField] private GameObject paramsMenu;
    [SerializeField] private GameObject warrior;
    [SerializeField] private GameObject mage;
    [SerializeField] private GameObject rogue;
    [SerializeField] private GameObject menuMusic;
    [SerializeField] private GameObject resolutionDropdown;

    private System.Random random;
    private string[] level;

    private PlayerSpawn playerSpawn;
    
    private void Awake()
    {
        Screen.SetResolution(1920, 1080, true);
        random = new System.Random();
        level = new string[] { "Arena1", "Arena2", "Arena3"};
        playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawner").GetComponent<PlayerSpawn>();
        menuMusic.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("MusicVolume");
    }


    public void PlayerMenu()
    {
        startMenu.SetActive(false);
        selectMenu.SetActive(true);
        paramsMenu.SetActive(false);
    }

    public void StarttMenu()
    {
        startMenu.SetActive(true);
        selectMenu.SetActive(false);
        paramsMenu.SetActive(false);
    }

    public void ParamsMenu()
    {
        startMenu.SetActive(false);
        selectMenu.SetActive(false);
        paramsMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SelectWarrior()
    {
        playerSpawn.SetPlayerType(warrior);
        PlayerPrefs.SetInt("WarriorGames", PlayerPrefs.GetInt("WarriorGames") + 1);
        LaunchLevel();
    }

    public void SelectMage()
    {
        playerSpawn.SetPlayerType(mage);
        PlayerPrefs.SetInt("MageGames", PlayerPrefs.GetInt("MageGames") + 1);
        LaunchLevel();
    }

    public void SelectRogue()
    {
        playerSpawn.SetPlayerType(rogue);
        PlayerPrefs.SetInt("RogueGames", PlayerPrefs.GetInt("RogueGames") + 1);
        LaunchLevel();
    }

    private void LaunchLevel()
    {
        int levelSelect = random.Next(3);
        SceneManager.LoadSceneAsync(level[levelSelect]);
    }

    public void ChangeResolution()
    {
        if (resolutionDropdown.GetComponent<TMP_Dropdown>().value == 0)
        {
            Screen.SetResolution(1920, 1080, true);
            // Debug.Log("FullHD");
        }
        if (resolutionDropdown.GetComponent<TMP_Dropdown>().value == 1)
        {
            Screen.SetResolution(1280, 720, true);
            // Debug.Log("HD");
        }
        if (resolutionDropdown.GetComponent<TMP_Dropdown>().value == 2)
        {
            Screen.SetResolution(720, 480, true);
            // Debug.Log("DVD");
        }
    }




}
