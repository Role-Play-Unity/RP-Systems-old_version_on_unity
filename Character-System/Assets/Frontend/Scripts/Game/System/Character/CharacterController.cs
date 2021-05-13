using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterMotor))]
public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private float DefaultSpeed = 3f;
    [SerializeField]
    private float RunningSpeed = 6f;
    [SerializeField]
    private float mouseSensivity = 5f;
    [SerializeField]
    private float jampForces = 2f;
    [SerializeField] private Vector4 headLimitRotation;
    [SerializeField] private CharacterHeadShake headShake;

    #region private
    private CharacterMotor motor;
    private Vector3 _velocity;

    private float _xHeadRot;
    private float _yHeadRot;
    #endregion

    private void Start()
    {
        motor = GetComponent<CharacterMotor>();
    }

    public void OnMove(InputAction.CallbackContext ctx) => CharacterMove(ctx.ReadValue<Vector2>());
    public void OnRotation(InputAction.CallbackContext ctx) => CharacterRotation(ctx.ReadValue<Vector2>());
    public void OnJamp(InputAction.CallbackContext ctx) => CharacterJamp();
    public void OnOpenUI(InputAction.CallbackContext ctx) => UIManager();

    private void CharacterMove(Vector2 _movementInput)
    {
        //Calculate movement velocity as a 3D vector
        Vector3 _movHorizontal = transform.right * _movementInput.x;
        Vector3 _movVertical = transform.forward * _movementInput.y;
        _velocity = (_movHorizontal + _movVertical).normalized;
        //Apply movement
        if (_movementInput.y > 0.5f)
        {
            motor.Move(_velocity * RunningSpeed); //Run move
            headShake.ShakeRotateCamera(0.1f, 0.5f, Vector2.right + Vector2.down);
        }
        else
        {
            motor.Move(_velocity * DefaultSpeed); //Defaut move
            headShake.ShakeRotateCamera(0.2f, 0.5f, Vector2.right + Vector2.down);
        }
        //headShake.ShakeRotateCamera(0.1f, 0.5f, Vector2.right + Vector2.down);
        Debug.Log("momevent input " + _movementInput);
    }
    private void CharacterRotation(Vector2 _rotationInput)
    {
        //_yHeadRot = _rotationInput.x * mouseSensivity;
        //_yHeadRot = Mathf.Clamp(_xHeadRot, headLimitRotation.z, headLimitRotation.w);
        Vector3 rotation = new Vector3(0f, _yHeadRot, 0f) * mouseSensivity;
        //Apply rotation
        motor.Rotate(rotation);
        CharacterHeadRotation(_rotationInput);
    }
    private void CharacterHeadRotation(Vector2 _headRotationInput)
    {
        //Calculate head rotation as a 3D vector (turning around)
        _xHeadRot += _headRotationInput.y * mouseSensivity;
        _xHeadRot = Mathf.Clamp(_xHeadRot, headLimitRotation.x, headLimitRotation.y);
        Vector3 headRotation = new Vector3(_xHeadRot, 0f);
        //Apply rotation
        motor.HeadRotate(headRotation);
    }
    private void CharacterJamp()
    {
        //Vector3 _jamp = new Vector3(0f, 1f, 0f) * jampForces;
        // motor.Jamp(_jamp);
    }
    private void UIManager() { MouseController.CursorVisible(); }

}
