using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgGren : MonoBehaviour
{
    public Rigidbody ball;
    [HideInInspector] public Transform target;

    public float h = 25;
    public float gravity = -18;

    public bool debugPath;

    public LineRenderer line;
    [Range(0, 2)] public float lineLength;

    public static bool isShot;

    public float arc;

    public Vector3 mousePos;

    public PlayerController controller;

    public bool player;


    void Start() {
        ball.useGravity = false;
        isShot = false;
    }

  
    public void Launch(Vector3 newMousePos) {
        ball.velocity = new Vector3(0f, 0f, 0f);
        mousePos = newMousePos;
        StartCoroutine(LaunchC(0.5f));
    }

    IEnumerator LaunchC(float time) {
        yield return new WaitForSeconds(time);
        Physics.gravity = Vector3.up * gravity;
        ball.useGravity = true;
        LaunchData data = CalculateLaunchData();
        ball.velocity = data.initialVelocity;
        isShot = true;
        StartCoroutine(DisableShoot(data.timeToTarget));
    }

    IEnumerator DisableShoot(float time) {
        yield return new WaitForSeconds(time);
        isShot = false;
    }

    LaunchData CalculateLaunchData() {
        float displacementY = mousePos.y + 5f;
        Vector3 displacementXZ = new Vector3(mousePos.x - ball.position.x, 0, mousePos.z - ball.position.z);
        float time = Mathf.Sqrt(Mathf.Abs((-2 * h / gravity))) + Mathf.Sqrt(Mathf.Abs((2 * (displacementY - h) / gravity)));
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(Mathf.Abs((-4 * gravity * h)));
        Vector3 velocityXZ = (displacementXZ / time) / arc;

        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), lineLength * time);
    }

    struct LaunchData {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget) {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }

    }
}
