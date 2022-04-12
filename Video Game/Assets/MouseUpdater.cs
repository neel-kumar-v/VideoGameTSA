using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseUpdater : MonoBehaviour
{
    public GameObject crosshair;
    public void ToggleCrosshair(bool enabled) {
        crosshair.SetActive(enabled);
        Cursor.visible = !enabled;
    }

}
