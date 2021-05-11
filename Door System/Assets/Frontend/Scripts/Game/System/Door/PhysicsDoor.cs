// NULLcode Studio © 2015
// null-code.ru

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(HingeJoint))]

public class PhysicsDoor : MonoBehaviour {

	public bool isOpen = false;
	public float force = 50; // добавление силы при открытии замка и для закрытия двери
	public float distance = 20;

	private Rigidbody body;
	private HingeJoint hinge;
	private Vector3 pos;
	private Quaternion rot;
	private Transform target;

	void Awake () 
	{
		pos = transform.localPosition;
		rot = transform.localRotation;

		hinge = GetComponent<HingeJoint>();
		body = GetComponent<Rigidbody>();

		if(!isOpen) body.isKinematic = true;

		JointMotor motor = hinge.motor;
		motor.force = force;
		motor.targetVelocity = force;
		motor.freeSpin = false;
		hinge.motor = motor;

		enabled = false;
	}
	
	public void Invert(Transform player)
	{
		target = player;

		if(!isOpen) 
		{
			body.isKinematic = false;
			body.AddForce((transform.position - Vector3.forward).normalized * force);
		}
		else
		{
			hinge.useMotor = true;
		}

		isOpen = !isOpen;
	}
	
	void Update ()
	{
		if(hinge.angle < 1 && hinge.useMotor)
		{
			hinge.useMotor = false;
			body.isKinematic = true;
			transform.localPosition = pos;
			transform.localRotation = rot;
			isOpen = false;
		}

		if(target)
		{
			float dis = Vector3.Distance(transform.position, target.position);
			if(dis > distance) enabled = false;
		}
	}
}
