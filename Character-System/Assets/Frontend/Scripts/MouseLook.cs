using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class MouseLook : MonoBehaviour
{
    public Vector2 Sensitivity = new Vector2(2f, 2f);
    public Vector2 LimitX = new Vector2(-90F, 90F);
    public Vector2 LimitY = new Vector2(-100F, 100F);
    /*IF VISIBLE = TRUE | Non Lock and Cursor Vivble - true*/ /*IF VISIBLE = FALSE | Locked and Cursor Vivble - false*/
    public bool lockCursor = true;
    
    public bool CursorVisible()
    {
        return CursorVisible(!lockCursor);
    }
    public bool CursorVisible(bool cursorIsLocked)
    {
        lockCursor = cursorIsLocked;
        return UpdateCursorLock();
    }
    public bool UpdateCursorLock()
    {
        if (lockCursor) Cursor.lockState = CursorLockMode.Confined;
        else Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = lockCursor;
        return lockCursor;
    }
    public void SetTextureForCursor(Texture2D texture, Vector2 hotSpot)
    {
        Cursor.SetCursor(texture, hotSpot, CursorMode.Auto);
    }
    public void SetDefautTextureForCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}



// Custom Editor
#if UNITY_EDITOR
[CustomEditor(typeof(MouseLook)), InitializeOnLoadAttribute]
public class MouseLookEditor : Editor
{
    MouseLook _mouseLook;

    private void OnEnable()
    {
        _mouseLook = (MouseLook)target;
    }
    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        GUILayout.Label("Mouse Look", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 });
        GUILayout.Label("By Life is Wolf", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Normal, fontSize = 12 });
        GUILayout.Label("version 0.0.3", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Normal, fontSize = 12 });
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        #region Variabiles Setup

        GUILayout.Label("Mouse Sensitivity Setup", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();

        _mouseLook.Sensitivity = EditorGUILayout.Vector2Field(new GUIContent("Mouse Sensitivity", "Mouse sensitivity for head and character control."), _mouseLook.Sensitivity);

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Head Limit Setup", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();

        _mouseLook.LimitX = EditorGUILayout.Vector2Field(new GUIContent("Head Limit X", "Restriction for head rotation."), _mouseLook.Sensitivity);
        _mouseLook.LimitY = EditorGUILayout.Vector2Field(new GUIContent("Head Limit X", "Restriction for head rotation."), _mouseLook.Sensitivity);
        #endregion
    }
}
#endif