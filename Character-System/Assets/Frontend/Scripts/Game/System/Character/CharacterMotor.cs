using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMotor : MonoBehaviour
{
    [SerializeField] private Transform head;
    private Vector3 velocity = Vector3.zero;
    private Vector3 bodyRotation = Vector3.zero;
    private Vector3 headRotation;

    private Rigidbody rb;

    public delegate void OnMove();
    public event OnMove EnentOnMove;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //Gets a movement vector
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }
    //Gets a rotational vector for the head
    public void HeadRotate(Vector2 _headRotation)
    {
        headRotation = _headRotation;
    }
    
    //Gets a rotational vector
    public void BodyRotate(Vector3 _bodyRotation)
    {
        bodyRotation = _bodyRotation;
    }

    //Run every phisics iteration
    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
        PerformHeadRotation();
    }

    //Perform movement base on velocity variable
    void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity);
            EnentOnMove.Invoke();
        }
    }

    //Perform rotation base on velocity variable
    void PerformRotation() => rb.MoveRotation(rb.rotation * Quaternion.Euler(bodyRotation));
    

    //Perform head rotation base on velocity variable
    void PerformHeadRotation()
    {
        if(head != null)
        {
            head.localEulerAngles =-headRotation;
        }
    }

    //Perform jamp base on velocity variable
    public void PerformJump(Vector3 _jump)
    {
        rb.AddForce(rb.mass * _jump);
    }
}
