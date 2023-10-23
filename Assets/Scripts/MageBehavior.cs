using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageBehavior : PlayerBehaviorBase
{
    [SerializeField] private GameObject fireBallPrefab;
    [SerializeField] private GameObject wallPrefab;

    public event EventHandler attack1;
    public event EventHandler attack2;
    public event EventHandler attack3;

    private AudioSource audio;
    [SerializeField] private AudioClip fireBall;
    [SerializeField] private AudioClip teleportation;

    public void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.volume = PlayerPrefs.GetFloat("EffectVolume");
    }

    public void OnAttack1(EventArgs e)
    {
        EventHandler handler = attack1;
        handler?.Invoke(this, e);
        audio.PlayOneShot(fireBall);
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
        audio.PlayOneShot(teleportation);
    }
    public override void Attack1()
    {
        attack1Timer = 0f;
        OnAttack1(new EventArgs());
        playerAnimator.SetTrigger("Attack1");
        // Debug.Log("Attack 1");
        Shoot();
    }

    private void Shoot()
    {
        GameObject fireBall = Instantiate(fireBallPrefab, transform.position + transform.forward + Vector3.up, Quaternion.identity);
        fireBall.GetComponent<FireBall>().SetDirection(GetFireBallDirection());
    }

    public override void Attack2()
    {
        attack2Timer = 0f;
        OnAttack2(new EventArgs());
        playerAnimator.SetTrigger("Attack2");
        // Debug.Log("Attack 2");
        SpawnWall();
    }

    private void SpawnWall()
    {
        GameObject mageWall = Instantiate(wallPrefab, GetWallPosition() + Vector3.up, RotationWall());
        mageWall.GetComponent<MageWall>().PushEnemiesWhenSpawn();
    }

    private Vector3 GetFireBallDirection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return (hit.point - (transform.position + transform.forward + Vector3.up)).normalized;
        }
        else
        {
            return transform.forward;
        }
    }

    private Vector3 GetWallPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        else
        {
            return transform.position + 3 * transform.forward;
        }
    }

    private Quaternion RotationWall()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position);
        float x_diff = mousePosition.x - object_pos.x;
        float y_diff = mousePosition.y - object_pos.y;
        float angle = Mathf.Atan2(x_diff, y_diff) * Mathf.Rad2Deg;
        return Quaternion.Euler(new Vector3(0, angle, 0));
    }

    public override void Attack3()
    {
        OnAttack3(new EventArgs());
        playerAnimator.SetTrigger("Attack3");
        // Debug.Log("Attack 3");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.tag == "Enemy")
            {
                attack3Timer = 0f;
                Vector3 PositionGameObjectUnderMouse = hit.transform.gameObject.transform.position;
                hit.transform.gameObject.transform.position = gameObject.transform.position;
                gameObject.transform.position = PositionGameObjectUnderMouse;
            }
        }
    }
}
