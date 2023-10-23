using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PhysicalMovement))]
public class SummonerBehavior : EnemyBehaviorBase
{
    [SerializeField] private GameObject SoldierForSummonerPrefab;
    private float velocity;
    private Animator enemyAnimator;
    [SerializeField] float distanceMinPlayer;
    private int currentnbInvocation = 0;
    [SerializeField] private int nbMaxInvocation = 6;

    private AudioSource audio;
    [SerializeField] private AudioClip hahaha;

    private Light light;
    private float initialIntensity; 
    [SerializeField] private float lightTimer = 0.5f;
    [SerializeField] private float lightSpeed = 10f;
    private float timer = 0f;

    private protected override void Awake()
    {
        base.Awake();
        enemyAgent = GetComponent<NavMeshAgent>();
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
        enemyAnimator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        physicalMovement = GetComponent<PhysicalMovement>();
        audio = GetComponent<AudioSource>();
        audio.volume = PlayerPrefs.GetFloat("EffectVolume");
        light = GetComponent<Light>();
        initialIntensity = light.intensity;
        // Debug.Log(enemyAgent);
    }
    private void Update()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (!player) return;

        if (enemyTargetable)
        {
            Rotate();
        }

        if (physicalMovement.IsBeingPushed())
        {
            if (rigidBody.velocity.magnitude <= 0.1f && physicalMovement.TimeMinBeingPushed())
            {
                enemyAgent.enabled = true;
                rigidBody.isKinematic = true;
                physicalMovement.SetBeingPushed(false);
                physicalMovement.ResetTimeBeingPushed();
            }
        }
        else
        {
            if (enemyTargetable)
            {
                Movement();
            }
        }
        if (attackTimer <= 5 * attackCooldown)
        {
            attackTimer += Time.deltaTime;
        }

        if (timer > 0)
        {
            timer += Time.deltaTime;
            if (timer <= lightTimer / 2)
            {
                light.intensity += Time.deltaTime * lightSpeed;
            }
            else
            {
                light.intensity -= Time.deltaTime * lightSpeed;
            }
            if (timer >= lightTimer)
            {
                timer = 0;
                light.intensity = initialIntensity;
            }
        }
    }

    /// <summary>
    /// The summoner's attack method.
    /// </summary>
    public override void Attack()
    {
        attackTimer = 0f;
        //enemyAnimator.SetTrigger("IsAttacking");
        // Debug.Log("Enemy attack");
        // audio.Play();
        Invoke();
        timer += Time.deltaTime;
    }

    private void Invoke()
    {
        GameObject soldierForSummoner = Instantiate(SoldierForSummonerPrefab, transform.position + 2 * transform.forward, Quaternion.identity);
        soldierForSummoner.GetComponent<SoldierForSummonerBehavior>().isSummoned = true;
        currentnbInvocation += 1;
        soldierForSummoner.GetComponent<DamageManager>().died += InvocationDied;
    }

    private void InvocationDied()
    {
        currentnbInvocation -= 1;
    }

    /// <summary>
    /// Determines the behavior of the summoner.
    /// </summary>
    public override void Movement()
    {
        if ((player.transform.position - transform.position).magnitude <= distanceMinPlayer)
        {
            Vector3 direction = gameObject.transform.position - player.transform.position;
            direction.y = 0;
            direction.Normalize();
            enemyAgent.Move(direction * enemyAgent.speed * Time.deltaTime * 2);
            // audio.PlayOneShot(hahaha);
        }
        else
        {
            if (attackTimer >= attackCooldown && currentnbInvocation < nbMaxInvocation)
            {
                enemyAgent.isStopped = true;
                Attack();
                enemyAgent.isStopped = false;
            }
        }
        velocity = Mathf.Sqrt(Mathf.Pow(enemyAgent.velocity.x, 2) + Mathf.Pow(enemyAgent.velocity.z, 2));
        //enemyAnimator.SetFloat("ForwardSpeed", velocity / enemyAgent.speed);

    }

    private void Rotate()
    {
        Vector3 playerPosition = Camera.main.WorldToScreenPoint(player.transform.position);
        Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position);
        float x_diff = playerPosition.x - object_pos.x;
        float y_diff = playerPosition.y - object_pos.y;
        float angle = Mathf.Atan2(x_diff, y_diff) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
    }
}
