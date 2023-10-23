using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PhysicalMovement))]
public class ArcherBehavior : EnemyBehaviorBase
{
    [SerializeField] private GameObject ArrowPrefab;
    private float velocity;
    private Animator enemyAnimator;
    private LayerMask enemyLayer;
    [SerializeField] float widthArcherToSeePlayer = 0.5f;

    private AudioSource audio;
    [SerializeField] private AudioClip swoosh;

    private protected override void Awake()
    {
        base.Awake();
        enemyAgent = GetComponent<NavMeshAgent>();
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
        enemyAnimator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemyLayer = LayerMask.GetMask("Enemy");
        audio = GetComponent<AudioSource>();
        audio.volume = PlayerPrefs.GetFloat("EffectVolume");
        physicalMovement = GetComponent<PhysicalMovement>();
        // Debug.Log(enemyAgent);

    }
    private void Update()
    {

        // Debug.Log(enemyAgent);
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
    }

    /// <summary>
    /// The archer's attack method.
    /// </summary>
    public override void Attack()
    {
        attackTimer = 0f;
        enemyAnimator.SetTrigger("IsAttacking");
        // Debug.Log("Enemy attack");
        // audio.Play();
        Shoot();
        audio.PlayOneShot(swoosh);
    }

    private void Shoot()
    {
        GameObject arrow = Instantiate(ArrowPrefab, transform.position + Vector3.up + 0.5f * transform.forward, RotationArrow());
        arrow.GetComponent<Arrow>().SetDirection((player.transform.position + Vector3.up) - (transform.position + Vector3.up + 0.5f * transform.forward));
    }

    /// <summary>
    /// Determines the behavior of the archer.
    /// </summary>
    public override void Movement()
    {
        if (attackTimer >= attackCooldown)
        {
            if (HasLineOfSightWithPlayer())
            {
                enemyAgent.isStopped = true;
                Attack();
                enemyAgent.isStopped = false;
            }
            else
            {
                enemyAgent.SetDestination(player.transform.position);
            }
        }
        else
        {
            if (HasLineOfSightWithPlayer())
            {
                Vector3 direction = gameObject.transform.position - player.transform.position;
                direction.y = 0;
                direction.Normalize();
                enemyAgent.Move(direction * enemyAgent.speed * Time.deltaTime * 2);
            }
            else
            {
                enemyAgent.SetDestination(player.transform.position);
            }
        }
        velocity = Mathf.Sqrt(Mathf.Pow(enemyAgent.velocity.x, 2) + Mathf.Pow(enemyAgent.velocity.z, 2));
        enemyAnimator.SetFloat("ForwardSpeed", velocity / enemyAgent.speed);

    }

    public bool HasLineOfSightWithPlayer()
    {
        bool hitPlayer = false;
        Vector3 directionSphereCast = (player.transform.position + Vector3.up) - (transform.position + Vector3.up + 0.5f * transform.forward);
        RaycastHit hit;

        if (Physics.SphereCast(transform.position + Vector3.up + 0.5f * transform.forward, 0.5f, directionSphereCast, out hit, 200, ~enemyLayer))
        {
            if (hit.transform.gameObject.tag == "Player")
            {
                hitPlayer = true;
            }
        }

        if (hitPlayer)
        {
            return true;
        }
        else
        {
            return false;
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

    private Quaternion RotationArrow()
    {
        Vector3 playerPosition = Camera.main.WorldToScreenPoint(player.transform.position);
        Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position);
        float x_diff = playerPosition.x - object_pos.x;
        float y_diff = playerPosition.y - object_pos.y;
        float height_diff = transform.position.y - player.transform.position.y;
        float distance = Vector3.Distance(player.transform.position, transform.position);
        float angle = Mathf.Atan2(x_diff, y_diff) * Mathf.Rad2Deg;
        float anglex = Mathf.Asin(height_diff / distance) * Mathf.Rad2Deg;
        return Quaternion.Euler(new Vector3(anglex, angle, 0));
    }
}
