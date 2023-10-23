using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PhysicalMovement))]
public class SoldierBehavior : EnemyBehaviorBase
{
    [SerializeField] private int damage;
    [SerializeField] private float attackDistance; //Distance à partir de laquelle le soldat commence à attaquer
    [SerializeField] private float attackReach; //rayon de la sphère de dégâts de l'ennemi
    private float velocity;
    private Animator enemyAnimator;
    private enum State { Moving, Attacking };
    private State state;
    private LayerMask playerLayer;
    private AudioSource audio;
    [SerializeField] private AudioClip attackSound;

    private protected override void Awake()
    {
        base.Awake();
        state = State.Moving;
        enemyAgent = GetComponent<NavMeshAgent>();
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
        attackDistance = attackDistance + transform.localScale.z / 2;
        attackReach = attackReach + transform.localScale.z / 2;
        enemyAnimator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerLayer = LayerMask.GetMask("Player");
        audio = GetComponent<AudioSource>();
        physicalMovement = GetComponent<PhysicalMovement>();
        audio.volume = PlayerPrefs.GetFloat("EffectVolume");
        // Debug.Log(enemyAgent);
    }
    private void Update()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (!player) return;

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
                enemyAgent.SetDestination(player.transform.position);
                Movement();
            }
        }
        if (attackTimer <= 5 * attackCooldown)
        {
            attackTimer += Time.deltaTime;
        }
        if (state == State.Attacking)
        {
            if (enemyTargetable)
            {
                Attack();
            }
        }

    }

    /// <summary>
    /// The soldier's attack method.
    /// </summary>
    public override void Attack()
    {
        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;
            enemyAnimator.SetTrigger("IsAttacking");
            bool sphereCheck = Physics.CheckSphere(transform.position + transform.forward * (attackReach) / 2 + Vector3.up * 3, (attackReach - transform.localScale.z) / 2, playerLayer);
            if (sphereCheck)
            {
                player.GetComponent<DamageManager>().TakeDamage(damage);
            }
            // Debug.Log("Enemy attack");
            audio.PlayOneShot(attackSound);
        }
    }

    /// <summary>
    /// Determines the behavior of the soldier, based on the distance between him and the player.
    /// </summary>
    public override void Movement()
    {
        if (enemyAgent.remainingDistance >= attackDistance)
        {
            state = State.Moving;
            enemyAgent.isStopped = false;
        }
        else
        {
            state = State.Attacking;
            enemyAgent.isStopped = true;
        }
        velocity = Mathf.Sqrt(Mathf.Pow(enemyAgent.velocity.x, 2) + Mathf.Pow(enemyAgent.velocity.z, 2));
        enemyAnimator.SetFloat("ForwardSpeed", velocity / enemyAgent.speed);

    }
}
