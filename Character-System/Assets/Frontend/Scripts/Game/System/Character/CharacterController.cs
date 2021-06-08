using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MLAPI;
using UnityEditor;

[RequireComponent(typeof(MouseLook))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CharacterMotor))]
[RequireComponent(typeof(CharacterHeadShake))]
public class CharacterController : NetworkBehaviour
{
    #region Variables
    [SerializeField] private float m_DefaultSpeed = 3f;
    [SerializeField] private float m_RunningSpeed = 6f;
    [SerializeField] private float m_JumpForces = 2f;
    [SerializeField] private LayerMask layerMask;
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
        if (IsLocalPlayer)
        {
            m_FovKick.Setup(m_Camera);

            m_MouseLook = GetComponent<MouseLook>();
            m_MouseLook.CursorVisible(false);

            m_Motor = GetComponent<CharacterMotor>();
            m_AudioSource = GetComponent<AudioSource>();


            m_Motor.OnMoveEnent += PlayFootStepSound;
            m_Motor.OnMoveEnent += MoveFovKick;

            layerMask = 1 << gameObject.layer | 1 << 2;
            layerMask = ~layerMask;
        }
        else //IsLocalPlayer = false
        {
            m_Camera.enabled = false;
            m_Camera.GetComponent<AudioListener>().enabled = false;
        }
    }

    #region Inputs
    public void OnMove(InputAction.CallbackContext ctx) => CharacterMove(ctx.ReadValue<Vector2>());
    public void OnRotation(InputAction.CallbackContext ctx) => CharacterBodyRotation(ctx.ReadValue<Vector2>());
    public void OnAction(InputAction.CallbackContext ctx) => Action();
    public void OnJamp(InputAction.CallbackContext ctx) => CharacterJump();
    public void OnOpenUI(InputAction.CallbackContext ctx) => UIManager();
    #endregion

    #region Character Movement and Rotation
    private void CharacterMove(Vector2 _movementInput)
    {
        if (!IsLocalPlayer) return;
        if (!m_IsGround) return;

        //Calculate movement velocity as a 3D vector
        Vector3 _movHorizontal = transform.right * _movementInput.x;
        Vector3 _movVertical = transform.forward * _movementInput.y;

        // Final movement vector
        Vector3 _velocity = (_movHorizontal + _movVertical).normalized;
        //Vector3 _velocity = (_movHorizontal + _movVertical);

        if (_movementInput.y > 0.7f)
        {
            _velocity *= m_RunningSpeed; //Run move
            m_IsRun = true;
        }
        else
        {
            _velocity *= m_DefaultSpeed; //Defaut
            m_IsRun = false;
        }

        // Animate movement
        //animator.SetFloat("", _movementInput.y);

        //Apply movement
        m_Motor.Move(_velocity);
    }
    private void MoveFovKick()
    {
        if (m_IsRun) m_FovKick.FOVKickDown();
        else m_FovKick.FOVKickUp();
    }
    private void CharacterBodyRotation(Vector2 _rotationInput)
    {
        if (!IsLocalPlayer) return;
        CharacterHeadRotation(_rotationInput);
        if (!m_IsGround) return;

        //Calculate rotation as a 3D vector (turning around)
        _yHeadRot = _rotationInput.x * m_MouseLook.Sensitivity.x;

        Vector3 rotation = new Vector3(0f, _yHeadRot, 0f);

        //Apply rotation
        m_Motor.BodyRotate(rotation);
    }
    private void CharacterHeadRotation(Vector2 _headRotationInput)
    {
        //Calculate head rotation as a 3D vector (turning around)
        _xHeadRot += _headRotationInput.y * m_MouseLook.Sensitivity.y;

        _xHeadRot = Mathf.Clamp(_xHeadRot, m_MouseLook.LimitY.x, m_MouseLook.LimitY.y);
        Vector3 headRotation = new Vector3(_xHeadRot, 0f);

        //Apply camera rotation
        m_Motor.HeadRotate(headRotation);
    }
    private void CharacterJump()
    {
        if (!m_IsGround) return;
        if (!IsLocalPlayer) return;

        m_Motor.PerformJump(m_JumpForces);
        PlayJumpSound();

        m_IsGround = false;
    }
    #endregion

    private void UIManager() { m_MouseLook.CursorVisible(); }
    private void Action() { }

    #region Trigger and Collision
    void OnCollisionEnter(Collision other)
    {
        // If we have collided with the platform
        //if (other.gameObject.tag == "Ground") { }
        // Then we must be on the ground
        m_IsGround = true;
    }
    private void OnTriggerEnter(Collider col)
    {

    }
    private void OnTriggerExit(Collider col)
    {

    }
    #endregion

    #region Sound
    private void PlayFootStepSound()
    {
        if (!GetJump()) return;
        if (m_AudioSource.isPlaying) return;
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = Random.Range(0, m_FootstepSounds.Length);
        m_AudioSource.clip = m_FootstepSounds[n];
        m_AudioSource.PlayOneShot(m_AudioSource.clip);
    }
    private void PlayJumpSound()
    {
        if (m_IsGround) return;
        m_AudioSource.clip = m_JumpSound;
        m_AudioSource.Play();
    }
    #endregion


    #region Get
    bool GetJump()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out hit, m_JumpForces, layerMask))
        {
            return true;
        }

        return false;
    }
    #endregion
}




// Custom Editor
#if UNITY_EDITOR
[CustomEditor(typeof(CharacterController)), InitializeOnLoadAttribute]
public class CharacterControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        GUILayout.Label("RP First Person Controller", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 });
        GUILayout.Label("By Life is Wolf", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Normal, fontSize = 12 });
        GUILayout.Label("version 0.0.1", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Normal, fontSize = 12 });
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }
}
#endif