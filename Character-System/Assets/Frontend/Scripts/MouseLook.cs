using System;
using System.Collections;
using System.Collections.Generic;
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