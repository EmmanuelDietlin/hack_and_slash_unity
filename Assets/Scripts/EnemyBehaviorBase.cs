using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBehaviorBase : MonoBehaviour
{
    [SerializeField] private protected float attackCooldown;
    private protected float attackTimer;
    private protected GameObject player;
    private protected NavMeshAgent enemyAgent;
    private protected Rigidbody rigidBody;
    private protected PhysicalMovement physicalMovement;
    private protected bool enemyTargetable;

    private protected virtual void Awake()
    {
        attackTimer = attackCooldown;
        StatesManager.invisible += EnemyUntargetable;
        StatesManager.visible += EnemyTargetable;
        enemyTargetable = true;
        
    }

    private protected virtual void EnemyUntargetable()
    {

        enemyTargetable = false;
        enemyAgent.isStopped = true;
        
    }

    private protected virtual void EnemyTargetable()
    {
        enemyTargetable = true;
        enemyAgent.isStopped = false;
    }

    public abstract void Movement();

    public abstract void Attack();

    public void OnDestroy()
    {
        StatesManager.invisible -= EnemyUntargetable;
        StatesManager.visible -= EnemyTargetable;
    }


}
