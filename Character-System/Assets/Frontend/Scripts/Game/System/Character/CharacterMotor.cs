using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMotor : MonoBehaviour
{
    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    //Gets a movement vector
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }
    //Gets a rotation vector
    public void Rotation(Vector3 _rotation)
    {
        rotation = _rotation;
    }
    //Run every phisics iteration
    void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    //Perform movement base on velocity variable
    void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }

    //Perform rotation base on velocity variable
    void PerformRotation()
    {
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }
}
