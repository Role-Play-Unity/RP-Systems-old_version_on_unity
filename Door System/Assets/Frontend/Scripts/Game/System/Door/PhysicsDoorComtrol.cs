// NULLcode Studio © 2015
// null-code.ru

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]

public class PhysicsDoorComtrol : MonoBehaviour {

	public float distance = 5;
	public string doorTag = "Door";
	public KeyCode key = KeyCode.F;
	private Camera cam;
	
	void Awake () 
	{
		cam = GetComponent<Camera>();
	}
	
	void Update () 
	{
		if(Input.GetKeyDown(key))
		{
			RaycastHit hit;
			Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
			if (Physics.Raycast(ray, out hit, distance))
			{
				if(hit.collider.tag == doorTag)
				{
					hit.transform.GetComponent<PhysicsDoor>().enabled = true;
					hit.transform.GetComponent<PhysicsDoor>().Invert(transform);
				}
			}
		}
	}
}
