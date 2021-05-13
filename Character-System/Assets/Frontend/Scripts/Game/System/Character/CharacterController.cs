using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private float DefaultSpeed = 3f;
    [SerializeField]
    private float RunningSpeed = 6f;
    [SerializeField]
    private float CroughtSpeed = 1f;
    [SerializeField]
    private float mouseSensivity = 5f;
    [SerializeField]
    private float jampForces = 2f;
    [SerializeField] private Vector2 headLimitRotation;

    private CharacterMotor motor;
    [SerializeField] private CharacterHeadShake headShake;

    private float _xRotMouse;
    private float _yRotMouse;

    private void Start()
    {
        motor = GetComponent<CharacterMotor>();
    }

    private void Update()
    {
        //Calculate movement velocity as a 3D vector
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        // 1/2 final movement vector
        Vector3 _velocity = (_movHorizontal + _movVertical).normalized;

        //Apply movement
        if (Input.GetKey(KeyCode.LeftShift)) // Run move
            motor.Move(_velocity * RunningSpeed);
        else if (Input.GetKey(KeyCode.LeftControl)) //Crought move
            motor.Move(_velocity * CroughtSpeed);
        else
            motor.Move(_velocity * DefaultSpeed); //Defaut move

        //
        headShake.ShakeRotateCamera(0.1f, 0.5f, Vector2.right + Vector2.down);
        //
        //Apply movement
        /*motor.Move(_velocity);*/

        //Calculate rotation as a 3D vector
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * mouseSensivity;

        //Apply rotation
        motor.Rotate(_rotation);
        
        //Calculate head rotation as a 3D vector (turning around)
        _xRotMouse += Input.GetAxis("Mouse Y") * mouseSensivity;

        _xRotMouse = Mathf.Clamp(_xRotMouse, headLimitRotation.x, headLimitRotation.y);

        Vector3 _headRotation = new Vector3(_xRotMouse, 0f);

        //Apply rotation
        motor.HeadRotate(_headRotation);

        Vector3 _jamp = new Vector3(0f, 1f, 0f) * jampForces;

        // motor.Jamp(_jamp);

        if (Input.GetKeyDown(KeyCode.Backspace))
            MouseController.CursorVisible();
    }
}
