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
    private float m_DefaultSpeed = 3f;
    [SerializeField]
    private float m_RunningSpeed = 6f;
    [SerializeField]
    private float m_JumpForces = 2f;
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
    private CharacterMotor m_Motor;
    private Vector3 _velocity;

    private float _xHeadRot;
    private float _yHeadRot;
    private bool m_IsGround = true;
    private bool m_IsRun = false;
    #endregion

    private void Start()
    {
        m_FovKick.Setup(m_Camera);
        
        m_MouseLook = GetComponent<MouseLook>();
        m_MouseLook.CursorVisible(false);

        m_Motor = GetComponent<CharacterMotor>();
        m_AudioSource = GetComponent<AudioSource>();


        m_Motor.EnentOnMove += PlayFootStepSound;
        m_Motor.EnentOnMove += MoveFovKick;
    }

    #region Inputs
    public void OnMove(InputAction.CallbackContext ctx) => CharacterMove(ctx.ReadValue<Vector2>());
    public void OnRotation(InputAction.CallbackContext ctx) => CharacterRotation(ctx.ReadValue<Vector2>());
    public void OnAction(InputAction.CallbackContext ctx) => Action();
    public void OnJamp(InputAction.CallbackContext ctx) => CharacterJamp();
    public void OnOpenUI(InputAction.CallbackContext ctx) => UIManager();
    #endregion

    #region Character
    private void CharacterMove(Vector2 _movementInput)
    {
        //Calculate movement velocity as a 3D vector
        Vector3 _movHorizontal = transform.right * _movementInput.x;
        Vector3 _movVertical = transform.forward * _movementInput.y;
        _velocity = (_movHorizontal + _movVertical).normalized;
        //Apply movement
        if (_movementInput.y > 0.7f)
        {
            m_Motor.Move(_velocity * m_RunningSpeed); //Run move
        }
        else
        {
            m_Motor.Move(_velocity * m_DefaultSpeed); //Defaut move
        }
        Debug.Log("momevent input " + _movementInput);
    }
    private void MoveFovKick()
    {
        if (m_IsRun) m_FovKick.FOVKickDown();
        else m_FovKick.FOVKickUp();  
    }
    private void CharacterRotation(Vector2 _rotationInput)
    {
        _yHeadRot = _rotationInput.x * m_MouseLook.Sensitivity.x;
        Vector3 rotation = new Vector3(0f, _yHeadRot, 0f);
        //Apply rotation
        m_Motor.Rotate(rotation);
        CharacterHeadRotation(_rotationInput);
        Debug.Log("rotation input " + _rotationInput);
    }
    private void CharacterHeadRotation(Vector2 _headRotationInput)
    {
        //Calculate head rotation as a 3D vector (turning around)
        _xHeadRot += _headRotationInput.y * m_MouseLook.Sensitivity.y;
        _xHeadRot = Mathf.Clamp(_xHeadRot, m_MouseLook.LimitY.x, m_MouseLook.LimitY.y);
        Vector3 headRotation = new Vector3(_xHeadRot, 0f);
        //Apply rotation
        m_Motor.HeadRotate(headRotation);
    }
    private void CharacterJamp()
    {
        if (!m_IsGround) return;
        Vector3 _jamp = new Vector3(0f, 100f, 0f) * m_JumpForces;
        m_Motor.PerformJamp(_jamp);
        PlayJumpSound();
        Debug.Log("Jamp");
    }
    #endregion

    private void UIManager() { m_MouseLook.CursorVisible(); }
    private void Action() {}

    #region Trigger
    private void OnTriggerEnter(Collider col)
    {
        m_IsGround = true;
    }
    private void OnTriggerExit(Collider col)
    {
        m_IsGround = false;
    }
    #endregion

    #region Sound
    private void PlayFootStepSound()
    {
        if (!m_IsGround) return;
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(0, m_FootstepSounds.Length);
        m_AudioSource.clip = m_FootstepSounds[n];
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
    }
    private void PlayJumpSound()
    {
        m_AudioSource.clip = m_JumpSound;
        m_AudioSource.Play();
    }
    #endregion
}
