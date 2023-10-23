using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PhysicalMovement : MonoBehaviour
{
    private Vector3 position;
    [SerializeField]
    private float pushForce;
    private bool isBeingPushed;
    private float timeBeingPushed = 0;
    [SerializeField]
    private float timeBeingPushedReset = 0.25f;
    private bool isJumping;
    [SerializeField]
    private float jumpForce = 20;
    [SerializeField]
    private float verticalForceJump = 20;
    private float timeJumping = 0;
    [SerializeField]
    private float timeJumpingReset = 0.25f;
    private NavMeshAgent Agent;
    private Rigidbody rigidBody;
    [SerializeField]
    private float distanceEndJump;


    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        rigidBody = GetComponent<Rigidbody>();
        isBeingPushed = false;
        isJumping = false;
    }

    public void Jump(Vector3 destination)
    {
        isJumping = true;
        // Debug.Log("Saut");
        Vector3 direction = destination - transform.position;
        direction.y = 0;
        direction.Normalize();
        direction *= jumpForce;
        direction.y = verticalForceJump;

        Agent.enabled = false;
        rigidBody.isKinematic = false;

        rigidBody.velocity = Vector3.zero;

        rigidBody.AddForce(direction, ForceMode.Impulse);
    }

    public bool ShouldEndJump()
    {
        RaycastHit hit;
        Ray downRay = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(downRay, out hit))
        {
            if(hit.distance <= distanceEndJump)
            {
                return true;
            }
        }
        return false;
    }

    public void PushedByEntity(GameObject entity)
    {
        isBeingPushed = true;
        Vector3 direction = transform.position - entity.transform.position;
        direction.y = 0;
        direction.Normalize();
        direction *= pushForce;

        Agent.enabled = false;
        rigidBody.isKinematic = false;

        rigidBody.velocity = Vector3.zero;

        rigidBody.AddForce(direction, ForceMode.Impulse);
    }

    public bool IsBeingPushed()
    {
        return this.isBeingPushed;
    }

    public void SetBeingPushed(bool value)
    {
        this.isBeingPushed = value;
    }

    public bool TimeMinBeingPushed()
    {
        if (this.timeBeingPushed >= this.timeBeingPushedReset)
        {
            return true;
        }
        else
        {
            this.timeBeingPushed += Time.deltaTime;
            return false;
        }
    }

    public void ResetTimeBeingPushed()
    {
        this.timeBeingPushed = 0f;
    }

    public bool IsJumping()
    {
        return this.isJumping;
    }

    public void SetJumping(bool value)
    {
        this.isJumping = value;
    }

    public bool TimeMinJumping()
    {
        if (this.timeJumping >= this.timeJumpingReset)
        {
            return true;
        }
        else
        {
            this.timeJumping += Time.deltaTime;
            return false;
        }
    }

    public void ResetTimeJumping()
    {
        this.timeJumping = 0f;
    }
}
