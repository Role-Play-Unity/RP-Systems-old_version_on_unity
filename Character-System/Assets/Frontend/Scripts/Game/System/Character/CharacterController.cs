using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MouseLook))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CharacterMotor))]
[RequireComponent(typeof(CharacterHeadShake))]
public class CharacterController : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private float DefaultSpeed = 3f;
    [SerializeField]
    private float RunningSpeed = 6f;
    [SerializeField]
    private float mouseSensivity = 5f;
    [SerializeField]
    private float jampForces = 2f;
    [SerializeField] private Camera m_Camera;
    [SerializeField] private MouseLook m_MouseLook;
    [SerializeField] private bool m_UseFovKick;
    [SerializeField] private FOVKick m_FovKick;
    #endregion

    #region Sound Variables
    [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
    [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
    [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.
    #endregion

    #region Private Variables
    private AudioSource m_AudioSource;
    private CharacterMotor motor;
    private Vector3 _velocity;

    private float _xHeadRot;
    private float _yHeadRot;
    private bool m_IsGround;
    #endregion

    private void Start()
    {
        m_FovKick = new FOVKick();
        m_FovKick.Setup(m_Camera);
        
        m_MouseLook = GetComponent<MouseLook>();
        m_MouseLook.CursorVisible(false);

        motor = GetComponent<CharacterMotor>();
        m_AudioSource = GetComponent<AudioSource>();
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
        if (_movementInput.y > 0.7f)
        {
            motor.Move(_velocity * RunningSpeed); //Run move
            m_FovKick.FOVKickDown();
        }
        else
        {
            motor.Move(_velocity * DefaultSpeed); //Defaut move
            m_FovKick.FOVKickUp();
        }
        Debug.Log("momevent input " + _movementInput);
        PlayFootStepAudio();
    }
    private void CharacterRotation(Vector2 _rotationInput)
    {
        _yHeadRot = _rotationInput.x * mouseSensivity;
        //_yHeadRot = Mathf.Clamp(_xHeadRot, headLimitRotation.z, headLimitRotation.w);
        Vector3 rotation = new Vector3(0f, _yHeadRot, 0f) * mouseSensivity;
        //Apply rotation
        motor.Rotate(rotation);
        CharacterHeadRotation(_rotationInput);
        Debug.Log("rotation input " + _rotationInput);
    }
    private void CharacterHeadRotation(Vector2 _headRotationInput)
    {
        //Calculate head rotation as a 3D vector (turning around)
        _xHeadRot += _headRotationInput.y * mouseSensivity;
        _xHeadRot = Mathf.Clamp(_xHeadRot, m_MouseLook.LimitY.x, m_MouseLook.LimitY.y);
        Vector3 headRotation = new Vector3(_xHeadRot, 0f);
        //Apply rotation
        motor.HeadRotate(headRotation);
    }
    private void CharacterJamp()
    {
        if (!m_IsGround) return;
        Vector3 _jamp = new Vector3(0f, 100f, 0f) * jampForces;
        motor.PerformJamp(_jamp);
        Debug.Log("Jamp");
    }
    private void UIManager() { m_MouseLook.CursorVisible(); }

    private void OnTriggerEnter(Collider col)
    {
        m_IsGround = true;
    }
    private void OnTriggerExit(Collider col)
    {
        m_IsGround = false;
    }
    #region Sound
    private void PlayFootStepAudio()
    {
        if (!m_IsGround) return;
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(1, m_FootstepSounds.Length);
        m_AudioSource.clip = m_FootstepSounds[n];
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
        // move picked sound to index 0 so it's not picked next time
        m_FootstepSounds[n] = m_FootstepSounds[0];
        m_FootstepSounds[0] = m_AudioSource.clip;
    }
    #endregion
}
