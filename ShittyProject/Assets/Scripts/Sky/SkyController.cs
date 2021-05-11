using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyController : MonoBehaviour
{
    public Transform Sun;
    public Transform Moon;

    public float CurrentTime = 0f;
    public float TimeSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.right, TimeSpeed * Time.deltaTime);
        transform.LookAt(Vector3.zero);
    }

    void SetTime(float t)
    {
        float multiplier = 6;
        transform.rotation = Quaternion.Euler(t * multiplier, transform.rotation.y, transform.rotation.z);
    }
}
