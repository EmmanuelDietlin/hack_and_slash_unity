using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{

    private NavMeshAgent playerAgent;
    private Animator playerAnimator;
    private float speed;
    private Vector3 previousPosition;
    private float positionDelta;
    private PhysicalMovement physicalMovement;
    private protected Rigidbody rigidBody;

    public event Action jumpFinished;
    private bool isParalized;

    private void Start()
    {
        playerAgent = GetComponent<NavMeshAgent>();
        speed = playerAgent.speed;
        physicalMovement = GetComponent<PhysicalMovement>();
        rigidBody = GetComponent<Rigidbody>();
        StatesManager.paralizePlayer += ParalizePlayer;
        StatesManager.unparalizePlayer += UnparalizePlayer;
        isParalized = false;


        if (gameObject.name.Contains("Warrior")) playerAnimator = gameObject.GetComponent<WarriorBehavior>().GetPlayerAnimator();
        if (gameObject.name.Contains("Mage")) playerAnimator = gameObject.GetComponent<MageBehavior>().GetPlayerAnimator();
        if (gameObject.name.Contains("Rogue")) playerAnimator = gameObject.GetComponent<RogueBehavior>().GetPlayerAnimator();
        // Debug.Log(playerAnimator);

    }
    private void Update()
    {
        previousPosition = transform.position;
        Rotate();

        if (physicalMovement.IsJumping())
        {
            if (physicalMovement.TimeMinJumping() && physicalMovement.ShouldEndJump())
            {
                playerAgent.enabled = true;
                rigidBody.isKinematic = true;
                physicalMovement.SetJumping(false);
                physicalMovement.ResetTimeJumping();
                jumpFinished?.Invoke();
            }
        }
        else if (physicalMovement.IsBeingPushed())
        {
            if (rigidBody.velocity.magnitude <= 0.1f && physicalMovement.TimeMinBeingPushed())
            {
                playerAgent.enabled = true;
                rigidBody.isKinematic = true;
                physicalMovement.SetBeingPushed(false);
                physicalMovement.ResetTimeBeingPushed();
            }
        }
        else
        {
            if (!isParalized)
            {
                if (Input.GetKey("z"))
                {
                    playerAgent.Move(Vector3.forward * speed * Time.deltaTime);
                }
                if (Input.GetKey("s"))
                {
                    playerAgent.Move(Vector3.back * speed * Time.deltaTime);
                }
                if (Input.GetKey("q"))
                {
                    playerAgent.Move(Vector3.left * speed * Time.deltaTime);
                }
                if (Input.GetKey("d"))
                {
                    playerAgent.Move(Vector3.right * speed * Time.deltaTime);
                }
            }
            positionDelta = Mathf.Abs(previousPosition.x - transform.position.x) + Mathf.Abs(previousPosition.y - transform.position.y) + Mathf.Abs(previousPosition.z - transform.position.z);
            if (positionDelta > 0.001)
            {
                playerAnimator.SetBool("IsMoving", true);
                //Vector3 walkVelocity = rigidBody.velocity;
                Vector3 walkVelocity = new Vector3(previousPosition.x - transform.position.x, previousPosition.y - transform.position.y, previousPosition.z - transform.position.z);
                Vector3 relative = Vector3.Normalize(transform.InverseTransformDirection(walkVelocity));
                float walkX = relative.x;
                //Debug.Log("x " + walkX);
                float walkZ = relative.z;
                //Debug.Log("z " + walkZ);
                playerAnimator.SetFloat("WalkX", walkX);
                playerAnimator.SetFloat("WalkZ", walkZ);
            }
            else
            {
                playerAnimator.SetBool("IsMoving", false);
            }
        }
    }

    private void Rotate()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position);
        float x_diff = mousePosition.x - object_pos.x;
        float y_diff = mousePosition.y - object_pos.y;
        float angle = Mathf.Atan2(x_diff, y_diff) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        // Debug.DrawRay(transform.position, transform.forward * 100f, Color.red);
    }

    public void MultiplySpeed(float number)
    {
        speed *= number;
    }

    public void DivideSpeed(float number)
    {
        speed /= number;
    }

    private protected virtual void ParalizePlayer()
    {
        isParalized = true;
    }

    private protected virtual void UnparalizePlayer()
    {
        isParalized = false;
    }

    public void OnDestroy()
    {
        StatesManager.paralizePlayer -= ParalizePlayer;
        StatesManager.unparalizePlayer -= UnparalizePlayer;
    }

}
