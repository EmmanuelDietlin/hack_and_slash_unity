using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossBehavior : MonoBehaviour
{
    [SerializeField] private GameObject fireBallForDragonPrefab;
    [SerializeField] private GameObject jawPrefab;
    [SerializeField] private GameObject fireFX;
    [SerializeField] private float attack1_1Cooldown;
    [SerializeField] private float attack1_1Reach;
    [SerializeField] private int attack1_1Damage;
    private float attack1_1Distance;
    [SerializeField] private float attack1_2Cooldown;
    [SerializeField] private float attack1_2CooldownTotalAction;
    private float attack1_2TimerTotalAction;
    private bool attack1_2Occuring;
    [SerializeField] private float attack1_2SpeedMultiplier;
    [SerializeField] private float attack2_1Cooldown;
    [SerializeField] private float attack2_1CooldownBetweenAttacks;
    private float attack2_1TimerBetweenAttacks;
    [SerializeField] private float attack2_1CooldownTotalAction;
    private float attack2_1TimerTotalAction;
    private bool attack2_1Occuring;
    [SerializeField] private float attack2_1Reach;
    [SerializeField] private int attack2_1Damage;
    private float attack2_1Distance;
    [SerializeField] private float attack2_2Cooldown;
    [SerializeField] private float attack2_2CooldownTotalAction;
    private float attack2_2TimerTotalAction;
    private bool attack2_2Occuring;
    [SerializeField] private float attack2_2Reach;
    [SerializeField] private int attack2_2Damage;
    private float attack2_2Distance;
    [SerializeField] private float attack3_1Cooldown;
    [SerializeField] private float attack3_1Reach;
    [SerializeField] private int attack3_1Damage;
    [SerializeField] private float attack3_2Cooldown;

    [SerializeField] private float attack3_2CooldownTotalAction;
    private float attack3_2TimerTotalAction;
    private bool attack3_2Occuring;
    [SerializeField] private float attack3_2FleeSpeed;


    private float attack1_1Timer;
    private float attack1_2Timer;
    private float attack2_1Timer;
    private float attack2_2Timer;
    private float attack3_1Timer;
    private float attack3_2Timer;
    private float hp;
    private GameObject player;
    private enum Phase { Phase1, Phase2, Phase3 };
    private Phase status;
    private Animator enemyAnimator;
    private protected NavMeshAgent enemyAgent;
    private protected Rigidbody rigidBody;
    private protected PhysicalMovement physicalMovement;
    private LayerMask playerLayer;
    private protected bool enemyTargetable;
    private AudioSource audio;
    [SerializeField] private AudioClip firrre;
    [SerializeField] private AudioClip roar;
    [SerializeField] private AudioClip roarLong;

    private void Awake()
    {
        status = Phase.Phase1;
        attack1_1Timer = attack1_1Cooldown;
        attack1_1Distance = attack1_1Reach - 0.5f;
        attack1_2Timer = attack1_2Cooldown;
        attack2_1Timer = attack2_1Cooldown;
        attack2_1Distance = attack2_1Reach - 0.5f;
        attack2_2Timer = attack2_2Cooldown;
        attack2_2Distance = attack2_2Reach - 0.5f;
        attack3_1Timer = attack3_1Cooldown;
        attack3_2Timer = attack3_2Cooldown;

        attack1_2TimerTotalAction = 0f;
        attack1_2Occuring = false;

        attack2_1TimerTotalAction = 0f;
        attack2_1TimerBetweenAttacks = attack2_1CooldownBetweenAttacks;
        attack2_1Occuring = false;

        attack2_2TimerTotalAction = 0f;
        attack2_2Occuring = false;

        player = GameObject.FindGameObjectWithTag("Player");
        StatesManager.invisible += EnemyUntargetable;
        StatesManager.visible += EnemyTargetable;
        enemyTargetable = true;
        enemyAgent = GetComponent<NavMeshAgent>();
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
        enemyAnimator = GetComponentInChildren<Animator>();
        playerLayer = LayerMask.GetMask("Player");
        audio = GetComponent<AudioSource>();
        physicalMovement = GetComponent<PhysicalMovement>();
    }

    private void Update()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (!player) return;

        CheckHp();
        Timer();

        if (attack1_2Occuring)
        {
            attack1_2TimerTotalAction += Time.deltaTime;
            if (attack1_2TimerTotalAction >= attack1_2CooldownTotalAction)
            {
                attack1_2TimerTotalAction = 0f;
                attack1_2Occuring = false;
                enemyAgent.speed /= attack1_2SpeedMultiplier;
            }
        }

        if (attack2_1Occuring)
        {
            fireFX.SetActive(true);
            attack2_1TimerBetweenAttacks += Time.deltaTime;
            attack2_1TimerTotalAction += Time.deltaTime;
            if (attack2_1TimerTotalAction >= attack2_1CooldownTotalAction)
            {
                attack2_1TimerTotalAction = 0f;
                fireFX.SetActive(false);
                attack2_1TimerBetweenAttacks = attack2_1CooldownBetweenAttacks;
                attack2_1Occuring = false;
                gameObject.GetComponent<PlayerMovement>().MultiplySpeed(1.5f);
            }
            else
            {
                if (attack2_1TimerBetweenAttacks >= attack2_1CooldownBetweenAttacks)
                {
                    attack2_1TimerBetweenAttacks = 0f;
                    Attack2_1Attack();
                }
            }
        }

        if (attack2_2Occuring)
        {
            attack2_2TimerTotalAction += Time.deltaTime;
            if (attack2_2TimerTotalAction >= attack2_2CooldownTotalAction)
            {
                attack2_2TimerTotalAction = 0f;
                attack2_2Occuring = false;
                StatesManager.UnparalizePlayer();
            }
        }

        if (attack3_2Occuring)
        {
            attack3_2TimerTotalAction += Time.deltaTime;
            if (attack3_2TimerTotalAction >= attack3_2CooldownTotalAction)
            {
                attack3_2TimerTotalAction = 0f;
                attack3_2Occuring = false;
                Rotate();
                Shoot();
            }
            else
            {
                Flee();
            }
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
                Behaviour();
            }
            if (enemyAgent.velocity.magnitude >= 0.1f)
            {
                enemyAnimator.SetBool("IsMoving", true);
            }
            else 
            { 
                enemyAnimator.SetBool("IsMoving", false); 
            }
        }

        
    }

    private void EnemyUntargetable()
    {
        enemyTargetable = false;
        enemyAgent.isStopped = true;
    }

    private void EnemyTargetable()
    {
        enemyTargetable = true;
        enemyAgent.isStopped = false;
    }

    private void Behaviour()
    {
        if (status == Phase.Phase1)
        {
            BehaviourPhase1();
        }
        else if (status == Phase.Phase2)
        {
            BehaviourPhase2();
        }
        else if (status == Phase.Phase3)
        {
            BehaviourPhase3();
        }
    }

    private void BehaviourPhase1()
    {
        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        if (distance <= attack1_1Distance && attack1_1Timer >= attack1_1Cooldown)
        {
            enemyAgent.isStopped = true;
            Attack1();
            enemyAgent.isStopped = false;
        }
        else
        {
            if (attack1_2Timer >= attack1_2Cooldown)
            {
                enemyAgent.isStopped = true;
                Attack2();
                enemyAgent.isStopped = false;
            }
        }
        enemyAgent.SetDestination(player.transform.position);
    }

    private void BehaviourPhase2()
    {
        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        if (distance <= attack2_1Distance && attack2_1Timer >= attack2_1Cooldown)
        {
            enemyAgent.isStopped = true;
            Attack1();
            enemyAgent.isStopped = false;
        }
        else
        {
            if (distance <= attack2_2Distance && attack2_2Timer >= attack2_2Cooldown)
            {
                enemyAgent.isStopped = true;
                Attack2();
                enemyAgent.isStopped = false;
            }
        }
        enemyAgent.SetDestination(player.transform.position);
    }

    private void BehaviourPhase3()
    {
        enemyAgent.ResetPath();
        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        if (attack3_2Timer >= attack3_2Cooldown)
        {
            Attack2();
        }
        else if (distance <= attack3_1Reach && attack3_1Timer >= attack3_1Cooldown)
        {
            Attack1();
        }
    }

    private void Flee()
    {
        Vector3 direction = gameObject.transform.position - player.transform.position;
        direction.y = 0;
        direction.Normalize();
        enemyAgent.Move(direction * enemyAgent.speed * Time.deltaTime * attack3_2FleeSpeed);
    }

    private void Timer()
    {
        if (status == Phase.Phase1)
        {
            attack1_1Timer += Time.deltaTime;
            attack1_2Timer += Time.deltaTime;
        }
        else if (status == Phase.Phase2)
        {
            attack2_1Timer += Time.deltaTime;
            attack2_2Timer += Time.deltaTime;
        }
        else if (status == Phase.Phase3)
        {
            attack3_1Timer += Time.deltaTime;
            attack3_2Timer += Time.deltaTime;
        }
    }

    private void Attack1()
    {
        if (status == Phase.Phase1)
        {
            attack1_1Timer = 0f;
            enemyAnimator.SetTrigger("11");
            audio.PlayOneShot(roar);
            bool sphereCheck = Physics.CheckSphere(transform.position + transform.forward * (attack1_1Reach) / 2 + Vector3.up * 3, (attack1_1Reach - transform.localScale.z) / 2, playerLayer);
            if (sphereCheck)
            {
                player.GetComponent<DamageManager>().TakeDamage(attack1_1Damage);
                // player.GetComponent<PhysicalMovement>().PushedByEntity(gameObject);
            }
            // Debug.Log("Enemy attack");
            // audio.Play();
        }
        else if (status == Phase.Phase2)
        {
            attack2_1Timer = 0f;
            enemyAnimator.SetTrigger("21");
            audio.PlayOneShot(firrre);
            attack2_1Occuring = true;
            gameObject.GetComponent<PlayerMovement>().DivideSpeed(1.5f);
        }
        else if (status == Phase.Phase3)
        {
            enemyAnimator.SetTrigger("31");
            player.GetComponent<DamageManager>().TakeDamage(attack1_1Damage);
            player.GetComponent<PhysicalMovement>().PushedByEntity(gameObject);
        }
    }

    public void Attack2_1Attack()
    {
        //enemyAnimator.SetTrigger("21");
        float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        if (distance <= attack2_1Reach)
        {
            player.GetComponent<DamageManager>().TakeDamage(attack2_1Damage);
        }
        // Debug.Log("Enemy attack");
        // saudio.Play();
    }

    private void Attack2()
    {
        if (status == Phase.Phase1)
        {
            // Debug.Log("Attack2");
            enemyAnimator.SetTrigger("12");
            audio.PlayOneShot(roarLong);
            attack1_2Timer = 0f;
            enemyAgent.speed *= attack1_2SpeedMultiplier;
            attack1_2Occuring = true;
            // enemyAnimator.SetTrigger("IsAttacking");
        }
        else if (status == Phase.Phase2)
        {
            attack2_2Timer = 0f;
            enemyAnimator.SetTrigger("22");
            audio.PlayOneShot(roar);
            bool sphereCheck = Physics.CheckSphere(transform.position + transform.forward * (attack2_2Reach) / 2 + Vector3.up * 3, (attack2_2Reach - transform.localScale.z) / 2, playerLayer);
            if (sphereCheck)
            {
                player.GetComponent<DamageManager>().TakeDamage(attack2_2Damage);
                attack2_2Occuring = true;
                StatesManager.ParalizePlayer();
            }
            // Debug.Log("Enemy attack");
            // saudio.Play();
        }
        else if (status == Phase.Phase3)
        {
            enemyAnimator.SetTrigger("32");
            attack3_2Timer = 0f;
            attack3_2Occuring = true;
        }
    }

    private void Shoot()
    {
        GameObject fireBallForDragon = Instantiate(fireBallForDragonPrefab, jawPrefab.transform.position, Quaternion.identity);
        fireBallForDragon.GetComponent<FireBallForDragon>().SetDirection(((player.transform.position + Vector3.up) - jawPrefab.transform.position).normalized);
    }

    private void CheckHp()
    {
        hp = gameObject.GetComponent<DamageManager>().HPRemainingRatio();
        if (hp <= 0.5 && hp >= 0.25)
        {
            status = Phase.Phase2;
        }
        else if (hp < 0.25)
        {
            status = Phase.Phase3;
        }

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

    public void OnDestroy()
    {
        StatesManager.invisible -= EnemyUntargetable;
        StatesManager.visible -= EnemyTargetable;
    }
}
