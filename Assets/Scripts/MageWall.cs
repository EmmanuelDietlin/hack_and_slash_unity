using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageWall : MonoBehaviour
{
    private Vector3 direction;
    private Collider[] enemyColliders;
    private LayerMask enemyLayer;
    private float timeToLive = 5f;
    private float lifeTimer = 0f;

    private AudioSource audio;
    [SerializeField] private AudioClip wallSound;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        audio.volume = PlayerPrefs.GetFloat("EffectVolume");
    }

    private void Start()
    {
        audio.PlayOneShot(wallSound);
    }

    private void Update()
    {
        lifeTimer += Time.deltaTime;
        if(lifeTimer >= timeToLive)
        {
            Destroy(gameObject);
        }
    }

    public void PushEnemiesWhenSpawn()
    {
        enemyColliders = Physics.OverlapBox(transform.position, new Vector3(2.5f, 1f, 0.5f), transform.rotation);
        foreach (Collider enemy in enemyColliders)
        {
            if (enemy.gameObject.tag == "Enemy")
            {
                enemy.gameObject.GetComponent<PhysicalMovement>().PushedByEntity(gameObject);
            }
        }
    }
}
