using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using UnityEditor;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Rigidbody))]
public class CharacterMotor : NetworkBehaviour
{
    [SerializeField] private Transform head;
    private Vector3 velocity = Vector3.zero;
    private Vector3 bodyRotation = Vector3.zero;
    private Vector3 headRotation;

    private Rigidbody rb;

    public delegate void OnMove();
    public event OnMove OnMoveEnent;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    //Gets a movement vector
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }
    //Gets a rotational vector for the head
    public void HeadRotate(Vector2 _headRotation)
    {
        headRotation = _headRotation;
    }
    
    //Gets a rotational vector
    public void BodyRotate(Vector3 _bodyRotation)
    {
        bodyRotation = _bodyRotation;
    }

    //Run every phisics iteration
    void FixedUpdate()
    {
        if(IsLocalPlayer)
        {
            PerformMovement();
            PerformRotation();
            PerformHeadRotation();

        }
    }

    //Perform movement base on velocity variable
    void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            //rb.AddForce(rb.position + velocity * Time.fixedDeltaTime, ForceMode.Impulse);
            rb.MovePosition(rb.position + rb.rotation.eulerAngles + velocity * Time.fixedDeltaTime);
            //rb.MovePosition(velocity * Time.fixedDeltaTime);
            OnMoveEnent.Invoke();
        }
    }

    //Perform rotation base on velocity variable
    //void PerformRotation() => transform.Rotate(bodyRotation);
    void PerformRotation() => rb.MoveRotation(rb.rotation * Quaternion.Euler(bodyRotation));


    //Perform head rotation base on velocity variable
    void PerformHeadRotation()
    {
        if(head != null)
        {
            head.localEulerAngles =-headRotation;
        }
    }

    //Perform jamp base on velocity variable
    public void PerformJump(float jumpForces)
    {
        if (IsLocalPlayer)
            rb.AddForce(Vector3.up * jumpForces);
        //rb.AddForce(rb.mass * _jump);
    }
}

// Custom Editor
#if UNITY_EDITOR
[CustomEditor(typeof(CharacterMotor)), InitializeOnLoadAttribute]
public class CharacterMotorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        GUILayout.Label("RP First Person Controller - Character Motor", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 });
        GUILayout.Label("By Life is Wolf", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Normal, fontSize = 12 });
        GUILayout.Label("version 0.0.1", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Normal, fontSize = 12 });
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }
}
#endif