using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MouseController
{
    /*IF VISIBLE = TRUE | Non Lock and Cursor Vivble - true*/ /*IF VISIBLE = FALSE | Locked and Cursor Vivble - false*/
    public static bool isVisible; 
    public static bool CursorVisible()
    {
        return CursorVisible(!isVisible);
    }
    public static bool CursorVisible(bool action)
    {
        isVisible = action;
        if (action) Cursor.lockState = CursorLockMode.Confined;
        else Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = action;
        return isVisible;
    }

    public static void SetTextureForCursor(Texture2D texture, Vector2 hotSpot)
    {
        Cursor.SetCursor(texture, hotSpot, CursorMode.Auto);
    }
    
    public static void SetDefautTextureForCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
