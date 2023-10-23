using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField]
    public float speed = 1f;
    private Vector3 direction;
    private Collider[] enemyColliders;
    [SerializeField] float fireBallReach;
    [SerializeField] int fireBallDamage;
    private LayerMask enemyLayer;


    private void Awake()
    {
        enemyLayer = LayerMask.GetMask("Enemy");
    }
    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection;
    }

    private void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            enemyColliders = Physics.OverlapSphere(transform.position, fireBallReach, enemyLayer);
            foreach (Collider enemy in enemyColliders)
            {
                enemy.gameObject.GetComponent<DamageManager>().TakeDamage(fireBallDamage);
                if (!other.gameObject.name.Contains("Boss"))
                {
                    enemy.gameObject.GetComponent<PhysicalMovement>().PushedByEntity(gameObject);
                }
            }
            Destroy(gameObject);
        }
    }
}
