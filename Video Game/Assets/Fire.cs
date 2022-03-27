using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public Rigidbody ball;
    public GameObject grenade;
    Vector3 target;
    public PlayerController controller;


    void Update() {
        if (Input.GetKeyDown (KeyCode.Space)) {
            Launch();
        }
    }

    void Launch() {
        GameObject newGrenade = (GameObject) Instantiate(grenade, transform.position, transform.rotation);
        ball = newGrenade.GetComponent<Rigidbody>();
        target = controller.mousePositionInWorldSpace;


        ball.velocity = new Vector3(0f, 0f, 0f);
    }
}
