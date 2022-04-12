using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedCrossHair : MonoBehaviour
{
    // Start is called before the first frame update
    public Image image;
    public PlayerController controller;
    public Camera cam;

    void Start()
    {
        //Hide mouse cursor
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = controller.isController ? cam.WorldToScreenPoint(controller.pos) : Input.mousePosition;
        image.color = controller.canShoot ? Color.white : Color.red;
    }
}
