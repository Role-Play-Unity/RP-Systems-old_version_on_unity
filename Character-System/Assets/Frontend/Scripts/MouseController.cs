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

}
