using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WarriorBehavior : PlayerBehaviorBase
{
    private Collider[] enemyColliders;
    private float attack3CooldownBetweenAttacks = 0.5f;
    private float attack3TimerBetweenAttacks;
    private float attack3CooldownTotalAction = 3f;
    private float attack3TimerTotalAction;
    private bool attack3Occuring;

    public event EventHandler attack1;
    public event EventHandler attack2;
    public event EventHandler attack3;

    private AudioSource audio;
    [SerializeField] private AudioClip hit;
    [SerializeField] private AudioClip saut;

    private Quaternion rotationBeforeSpin;
    public void OnAttack1(EventArgs e)
    {
        EventHandler handler = attack1;
        handler?.Invoke(this,e);
    }

    public void OnAttack2(EventArgs e)
    {
        EventHandler handler = attack2;
        handler?.Invoke(this, e);
    }

    public void OnAttack3(EventArgs e)
    {
        EventHandler handler = attack3;
        handler?.Invoke(this, e);
    }

    private protected override void Awake()
    {
        base.Awake();
        attack3Occuring = false;
        attack3TimerBetweenAttacks = attack3CooldownBetweenAttacks;
        attack3TimerTotalAction = 0f;
        audio = GetComponent<AudioSource>();
        audio.volume = PlayerPrefs.GetFloat("EffectVolume");
    }

    private protected override void Update()
    {
        base.Update();
        if (attack3Occuring)
        {
            Attack3Timers();
            if (attack3TimerTotalAction >= attack3CooldownTotalAction)
            {
                attack3TimerTotalAction = 0f;
                attack3TimerBetweenAttacks = attack3CooldownBetweenAttacks;
                attack3Occuring = false;
                gameObject.GetComponent<PlayerMovement>().DivideSpeed(1.5f);
                playerAnimator.SetBool("Attack3", false);
                // transform.rotation = rotationBeforeSpin;
            }
            else
            {
                if (attack3TimerBetweenAttacks >= attack3CooldownBetweenAttacks)
                {
                    attack3TimerBetweenAttacks = 0f;
                    Attack3Attack();
                }
            }
        }
    }

    

    public override void Attack1()
    {
        attack1Timer = 0f;
        OnAttack1(new EventArgs());
        playerAnimator.SetTrigger("Attack1");
        enemyColliders = Physics.OverlapSphere(transform.position + attack1Reach * transform.forward, attack1Reach, enemyLayer);
        // Debug.Log(enemyColliders);
        foreach (Collider enemy in enemyColliders)
        {
            enemy.gameObject.GetComponent<DamageManager>().TakeDamage(attack1Damage);
            enemy.gameObject.GetComponent<PhysicalMovement>().PushedByEntity(gameObject);
            // Debug.Log(enemy.gameObject.name);
        }
        audio.PlayOneShot(hit);
    }

    public override void Attack2()
    {
        attack2Timer = 0f;
        OnAttack2(new EventArgs());
        playerAnimator.SetTrigger("Attack2");
        // Debug.Log("Attaque 2");
        GetComponent<PlayerMovement>().jumpFinished += Attack2Attack;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            // rajouter condition endroit touché est du sol
            physicalMovement.Jump(hit.point);
        }
        audio.PlayOneShot(saut);
    }

    public void Attack2Attack()
    {
        // playerAnimator.SetTrigger("Attack2Attack");
        enemyColliders = Physics.OverlapSphere(transform.position, attack2Reach, enemyLayer);
        // Debug.Log(enemyColliders);
        foreach (Collider enemy in enemyColliders)
        {
            enemy.gameObject.GetComponent<DamageManager>().TakeDamage(attack2Damage);
            enemy.gameObject.GetComponent<PhysicalMovement>().PushedByEntity(gameObject);
            // Debug.Log(enemy.gameObject.name);
        }
        GetComponent<PlayerMovement>().jumpFinished -= Attack2Attack;
    }

    public override void Attack3()
    {
        attack3Timer = 0f;
        // playerAnimator.SetTrigger("Attack3");
        OnAttack3(new EventArgs());
        // rotationBeforeSpin = transform.rotation;
        // StartCoroutine(SpinAttack(gameObject));
        attack3Occuring = true;
        gameObject.GetComponent<PlayerMovement>().MultiplySpeed(1.5f);
    }

    public void Attack3Attack()
    {
        // playerAnimator.SetTrigger("Attack3Attack");
        //playerAnimator.SetBool("Attack3", true);
        playerAnimator.SetTrigger("Attack3");
        enemyColliders = Physics.OverlapSphere(transform.position, attack3Reach, enemyLayer);
        // Debug.Log(enemyColliders);
        foreach (Collider enemy in enemyColliders)
        {
            enemy.gameObject.GetComponent<DamageManager>().TakeDamage(attack3Damage);
            // Debug.Log(enemy.gameObject.name);
        }
    }

    private void Attack3Timers()
    {
        attack3TimerBetweenAttacks += Time.deltaTime;
        attack3TimerTotalAction += Time.deltaTime;
    }

    private IEnumerator SpinAttack(GameObject spinObject)
    {
        Quaternion startRotation = spinObject.transform.rotation;
        float t = 0f;
        while (t < 3)
        {
            t += Time.deltaTime;
            transform.rotation = startRotation * Quaternion.AngleAxis(t / 3 * 1440, Vector3.up);
            yield return null;
        }
 

    }
}
