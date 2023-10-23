using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatsManager : MonoBehaviour
{
    [SerializeField] private GameObject warriorWavesText;
    [SerializeField] private GameObject warriorGamesText;
    [SerializeField] private GameObject warriorMedal;
    [SerializeField] private GameObject mageWavesText;
    [SerializeField] private GameObject mageGamesText;
    [SerializeField] private GameObject mageMedal;
    [SerializeField] private GameObject rogueWavesText;
    [SerializeField] private GameObject rogueGamesText;
    [SerializeField] private GameObject rogueMedal;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("WarriorWaves")) PlayerPrefs.SetInt("WarriorWaves",0);
        warriorWavesText.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("WarriorWaves").ToString();
        if (!PlayerPrefs.HasKey("WarriorGames")) PlayerPrefs.SetInt("WarriorGames", 0);
        warriorGamesText.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("WarriorGames").ToString();

        if (!PlayerPrefs.HasKey("MageWaves")) PlayerPrefs.SetInt("MageWaves", 0);
        mageWavesText.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("MageWaves").ToString();
        if (!PlayerPrefs.HasKey("MageGames")) PlayerPrefs.SetInt("MageGames", 0);
        mageGamesText.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("MageGames").ToString();

        if (!PlayerPrefs.HasKey("RogueWaves")) PlayerPrefs.SetInt("RogueWaves", 0);
        rogueWavesText.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("RogueWaves").ToString();
        if (!PlayerPrefs.HasKey("RogueGames")) PlayerPrefs.SetInt("RogueGames", 0);
        rogueGamesText.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("RogueGames").ToString();

        if (PlayerPrefs.GetInt("WarriorWaves") == 10) warriorMedal.SetActive(true);
        if (PlayerPrefs.GetInt("MageWaves") == 10) mageMedal.SetActive(true);
        if (PlayerPrefs.GetInt("RogueWaves") == 10) rogueMedal.SetActive(true);

        /* Debug.Log("WarriorWaves : " + PlayerPrefs.GetInt("WarriorWaves").ToString());
        Debug.Log("WarriorGames : " + PlayerPrefs.GetInt("WarriorGames").ToString());
        Debug.Log("MageWaves : " + PlayerPrefs.GetInt("MageWaves").ToString());
        Debug.Log("MageGames : " + PlayerPrefs.GetInt("MageGames").ToString());
        Debug.Log("RogueWaves : " + PlayerPrefs.GetInt("RogueWaves").ToString());
        Debug.Log("RogueGames : " + PlayerPrefs.GetInt("RogueGames").ToString()); */

    }

}
