// NULLcode Studio © 2015
// null-code.ru

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class PlayerControl : MonoBehaviour {
	
	public float speed = 1.5f;
	public float acceleration = 100f;

	public Transform head;

	public float sensitivity = 5f;
	public float headMinY = -40f;
	public float headMaxY = 40f;

	private Vector3 direction;
	private float h, v;
	private Rigidbody body;
	private float rotationY;
	
	void Start () 
	{
		if(acceleration < 0) acceleration = 1;
		body = GetComponent<Rigidbody>();
		body.freezeRotation = true;
	}
	
	void FixedUpdate()
	{
		body.AddForce(direction * speed * acceleration * body.mass);
		
		// Ограничение скорости, иначе объект будет постоянно ускоряться
		if(Mathf.Abs(body.velocity.x) > speed)
		{
			body.velocity = new Vector3(Mathf.Sign(body.velocity.x) * speed, body.velocity.y, body.velocity.z);
		}
		if(Mathf.Abs(body.velocity.z) > speed)
		{
			body.velocity = new Vector3(body.velocity.x, body.velocity.y, Mathf.Sign(body.velocity.z) * speed);
		}

		if (Input.GetKey(KeyCode.Space))
		{
			body.velocity = new Vector3(0, 10, 0);
		}
	}

	void Update () 
	{
		h = Input.GetAxis("Horizontal");
		v = Input.GetAxis("Vertical");


		// Вертим головой 
		float rotationX = head.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
		rotationY += Input.GetAxis("Mouse Y") * sensitivity;
		rotationY = Mathf.Clamp (rotationY, headMinY, headMaxY);
		head.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		
		// Расчет направления движения
		direction = new Vector3(h, 0, v);
		direction = head.TransformDirection(direction);
		direction = new Vector3(direction.x, 0, direction.z);
	}
}
