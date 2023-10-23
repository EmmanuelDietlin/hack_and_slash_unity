using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class PlayerBehaviorBase : MonoBehaviour
{
    [SerializeField] private protected float attack1Cooldown;
    [SerializeField] private protected float attack2Cooldown;
    [SerializeField] private protected float attack3Cooldown;
    [SerializeField] private protected int attack1Damage;
    [SerializeField] private protected int attack2Damage;
    [SerializeField] private protected int attack3Damage;
    [SerializeField] private protected float attack1Reach;
    [SerializeField] private protected float attack2Reach;
    [SerializeField] private protected float attack3Reach;
    private protected float attack1Timer;
    private protected float attack2Timer;
    private protected float attack3Timer;
    private protected LayerMask enemyLayer;
    private protected Animator playerAnimator;
    private protected Rigidbody Rigidbody;
    private protected NavMeshAgent playerAgent;
    private protected PhysicalMovement physicalMovement;


    private protected virtual void Awake()
    {
        attack1Timer = attack1Cooldown;
        attack2Timer = attack2Cooldown;
        attack3Timer = attack3Cooldown;
        enemyLayer = LayerMask.GetMask("Enemy");
        playerAnimator = gameObject.GetComponentInChildren<Animator>();
        // Debug.Log(playerAnimator);
        Rigidbody = GetComponent<Rigidbody>();
        playerAgent = GetComponent<NavMeshAgent>();
        physicalMovement = GetComponent<PhysicalMovement>();
    }

    private protected virtual void Update()
    {
        AttacksTimer();
        if (Input.GetKeyDown(KeyCode.Mouse0) && attack1Timer >= attack1Cooldown)
        {
            Attack1();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && attack2Timer >= attack2Cooldown)
        {
            Attack2();
        }
        if (Input.GetKeyDown(KeyCode.Space) && attack3Timer >= attack3Cooldown)
        {
            Attack3();
        }
    }


    public abstract void Attack1();
    public abstract void Attack2();
    public abstract void Attack3();

    private void AttacksTimer()
    {
        attack1Timer += Time.deltaTime;
        attack2Timer += Time.deltaTime;
        attack3Timer += Time.deltaTime;
    }

    public Animator GetPlayerAnimator()
    {
        return playerAnimator;
    }

    public float[] GetAttacksCooldown()
    {
        return new float[] { attack1Cooldown, attack2Cooldown, attack3Cooldown };
    }

    public float[] GetAttacksTimer()
    {
        return new float[] { attack1Timer, attack2Timer, attack3Timer };
    }
}
