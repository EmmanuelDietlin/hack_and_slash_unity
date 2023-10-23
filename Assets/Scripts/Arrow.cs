using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    public float speed = 25f;
    private Vector3 direction;
    [SerializeField] int arrowDamage;

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }

    private void Update()
    {
        transform.Translate(direction * Time.deltaTime * speed, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Enemy")
        {
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<DamageManager>().TakeDamage(arrowDamage);
            }
            Destroy(gameObject);
        }
    }
}
