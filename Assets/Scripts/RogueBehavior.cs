using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RogueBehavior : PlayerBehaviorBase
{
    private Collider[] enemyColliders;
    private float attack2CooldownTotalAction = 2f;
    private float attack2TimerTotalAction;
    private bool attack2Occuring;

    [SerializeField] private GameObject roguePiece1;
    [SerializeField] private GameObject roguePiece2;
    [SerializeField] private GameObject roguePiece3;
    [SerializeField] private GameObject roguePiece4;




    public event EventHandler attack1;
    public event EventHandler attack2;
    public event EventHandler attack3;



    public void OnAttack1(EventArgs e)
    {
        EventHandler handler = attack1;
        handler?.Invoke(this, e);
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
    private protected override void Update()
    {
        base.Update();
        if (attack2Occuring)
        {
            attack2TimerTotalAction += Time.deltaTime;
            if(attack2TimerTotalAction >= attack2CooldownTotalAction)
            {
                attack2TimerTotalAction = 0f;
                attack2Occuring = false;
                StatesManager.IsVisible();
                roguePiece1.SetActive(true);
                roguePiece2.SetActive(true);
                roguePiece3.SetActive(true);
                roguePiece4.SetActive(true);
            }
        }
    }
    public override void Attack1()
    {
        attack1Timer = 0f;
        playerAnimator.SetTrigger("Attack1");
        enemyColliders = Physics.OverlapSphere(transform.position + attack1Reach * transform.forward, attack1Reach, enemyLayer);
        float distanceMin = float.PositiveInfinity;
        GameObject enemyToAttack = new GameObject();
        bool attack = false;
        foreach (Collider enemy in enemyColliders)
        {
            attack = true;
            if (GetMagnitude(enemy) <= distanceMin)
            {
                distanceMin = GetMagnitude(enemy);
                enemyToAttack = enemy.gameObject;
            }
        }
        if (attack)
        {
            enemyToAttack.GetComponent<DamageManager>().TakeDamage(attack1Damage);
        }
        OnAttack1(new EventArgs());
    }

    private float GetMagnitude(Collider enemy)
    {
        return (enemy.gameObject.transform.position - gameObject.transform.position).magnitude;
    }

    public override void Attack2()
    {
        attack2Timer = 0f;
        attack2Occuring = true;
        StatesManager.IsInvisible();
        roguePiece1.SetActive(false);
        roguePiece2.SetActive(false);
        roguePiece3.SetActive(false);
        roguePiece4.SetActive(false);

    }

    public override void Attack3()
    {
        OnAttack3(new EventArgs());
        // Debug.Log("Attack 3");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.tag == "Enemy")
            {
                attack3Timer = 0f;
                if (hit.transform.gameObject.name.Contains("Boss"))
                {
                    gameObject.transform.position = hit.transform.gameObject.transform.position + 2 * transform.forward;
                    hit.transform.gameObject.GetComponent<DamageManager>().DamagePercent(10);
                }
                else
                {
                    gameObject.transform.position = hit.transform.gameObject.transform.position;
                    hit.transform.gameObject.GetComponent<DamageManager>().Kill();
                }
            }
        }
    }
}
