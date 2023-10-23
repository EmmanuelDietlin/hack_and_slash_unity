using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DamageManager : MonoBehaviour
{
    [SerializeField] private int hp;
    private int maxHP;
    private EnemySpawnManager enemySpawnManager;
    private float minHigh = -10f;

    public event EventHandler playerDie;
    public event EventHandler damageTaken;
    public event EventHandler updatePlayerLifeBar;

    private Animator anim;

    private AudioSource audio;

    [SerializeField] private AudioClip takeDamage;
    [SerializeField] private AudioClip death;

    public event Action died;

    
    public void OnUpdatePlayerLifeBar(EventArgs e)
    {
        EventHandler handler = updatePlayerLifeBar;
        handler?.Invoke(this, e);
    }
    public void OnPlayerDie(EventArgs e)
    {
        EventHandler handler = playerDie;
        handler?.Invoke(this, e);
        audio.PlayOneShot(death);
        anim.SetTrigger("Death");
    }

    public void OnDamageTaken(EventArgs e)
    {
        EventHandler handler = damageTaken;
        handler?.Invoke(this, e);
        // audio.PlayOneShot(takeDamage);
    }

    private void Awake()
    {
        maxHP = hp;
        enemySpawnManager = GameObject.Find("EnemySpawnManager").GetComponent<EnemySpawnManager>();
        audio = GetComponent<AudioSource>();
        audio.volume = PlayerPrefs.GetFloat("EffectVolume");
        anim = GetComponentInChildren<Animator>();
    }
    
    private void Update()
    {
        if(gameObject.transform.position.y <= minHigh)
        {
            Kill();
        }
    }
    public void TakeDamage(int damage)
    {
        hp -= damage;
        OnDamageTaken(new EventArgs());
        TestDie();

        // Debug.Log(damage + " dmg taken");
        // Debug.Log(hp + " hp left");
    }

    public void Kill()
    {
        TakeDamage(hp);
    }

    public void DamagePercent(int damage)
    {
        int damageDone = maxHP * damage / 100;
        TakeDamage(damageDone);
    }

    public void TestDie()
    {
        if (hp <= 0)
        {
            if (gameObject.tag == "Player")
            {
                OnPlayerDie(new EventArgs());
                // Debug.Log("Defeat");
            }
            if (!gameObject.GetComponent<SoldierForSummonerBehavior>())
            {
                enemySpawnManager.enemyKilled();
            }
            died?.Invoke();
            //Destroy(gameObject);
            StartCoroutine(Dying());
        }

    }

    private IEnumerator Dying()
    {
        audio.PlayOneShot(death);
        anim.SetTrigger("Death");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    public float HPRemainingRatio()
    {
        return (float)hp / (float)maxHP;
    }
    public int GetHP()
    {
        return hp;
    }

    public int GetMaxHP()
    {
        return maxHP;
    }

    public void ResetHP()
    {
        hp = maxHP;
        //OnDamageTaken(new EventArgs());
        OnUpdatePlayerLifeBar(new EventArgs());
    }


}
