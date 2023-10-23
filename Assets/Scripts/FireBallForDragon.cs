using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallForDragon : MonoBehaviour
{
    [SerializeField]
    public float speed;
    private Vector3 direction;
    [SerializeField] int fireBallDamage;

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
        if (other.gameObject.tag != "Enemy")
        {
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<DamageManager>().TakeDamage(fireBallDamage);
                // other.gameObject.GetComponent<PhysicalMovement>().PushedByEntity(gameObject);
            }
            Destroy(gameObject);
        }
    }
}
