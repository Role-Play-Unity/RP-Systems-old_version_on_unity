using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    //Mouse variables
    public int MinHeadRotation = -30;
    public int MaxHeadRotation = 30;
    public int MouseSensivity = 2;

    float mouseX;
    float mouseY;

    //Movement variables
    public int Speed = 7;

    //Other variables
    public Transform Cam;

    void Update()
    {
        //Mouse movement
        mouseX = Input.GetAxis("Mouse X") * MouseSensivity;
        mouseY -= Input.GetAxis("Mouse Y") * MouseSensivity;

        //Vertical clamp
        mouseY = Mathf.Clamp(mouseY, MinHeadRotation, MaxHeadRotation);

        //Camera movement
        Cam.localEulerAngles = new Vector3(mouseY, 0f, 0f);

        //Keyboard input
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        //Player input movement
        Vector3 move = new Vector3(x, 0, z) * Speed * Time.deltaTime;

        //Player movement
        transform.Translate(move);
        transform.Rotate(0f, mouseX, 0f);
    }
}
