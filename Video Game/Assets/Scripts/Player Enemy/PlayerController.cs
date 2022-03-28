using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Unity Setup")]

    public float radius = 0.2f;
    public float force = 7000f;

    public Rigidbody rb;

    public bool isPlayer2 = false;
    public Camera cam;
    public CameraShake shake;
    [Space(15)]
    public GameObject bullet;
    public Transform firePoint;
    [Space(15)]
    public GameObject cylinder;
    public Vector3 cylinderPos;
    public Vector3 cylinderRot;
    [Space(10)]
    public Vector3 startPos;

    [Header("Parameters")]
    public float speed;
    float reloadTime;
    public float recoil;
    public float recoilTime;
    public float countdown;

    public GameObject grenade;

    public float turnSpeed = 0.01f;
    Quaternion rotGoal;
    Vector3 direction;

    [HideInInspector] public Vector3 mousePositionInWorldSpace;
    Vector3 movement;

    bool canShoot; 
    bool canMove;
    bool overrideVelocity = false;

    public Bullet b;

    public bool canGrenadeShoot;
    public CameraFollow follow;

    public bool inverse;


    public void Awake() {
        GameObject cylinderGO = (GameObject) Instantiate(cylinder, transform, false);
    }

    public void Start() {
        canShoot = true;
        Reset();
    }

    public void OnEnable() {
        Reset();
    }

    public void Reset() {
        // Reset position, rotation, and velocity
        transform.position = new Vector3(15f * (float) (Random.Range(0, 2) * 2 - 1), 2f, 15f);
        rb.velocity = Vector3.zero;
        Vector3 lookDirection = Vector3.Lerp(transform.position, new Vector3(0f, 0f, 0f), Time.deltaTime);
        transform.LookAt(lookDirection); 

        StartCoroutine(Countdown(countdown)); // be able to move only after the countdown
        
        // make sure all stats are correct
        b = bullet.GetComponent<Bullet>();
        speed /= b.weight;
        reloadTime = b.reload;  
    }

    public IEnumerator Countdown(float time) {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    void Update()
    {
        if(!canMove) return;
        Rotate();
        if(Input.GetButton("Fire1") && canShoot) {
            if(EventSystem.current.IsPointerOverGameObject()) return;
            canShoot = false;
            StartCoroutine(Shoot(reloadTime));
        }
     }

    // Update is called once per frame
    void FixedUpdate()
    {
        follow.Follow();
        if(!canMove) return;
        Move();
    }

    public void Move() {
        if (overrideVelocity) {return;}
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        if (isPlayer2) {
            movement = new Vector3(Input.GetAxisRaw("Horizontal1"), 0f, Input.GetAxisRaw("Vertical1"));
        }
        rb.velocity = movement * speed * (inverse ? -1f : 1f);
    }

    public void Rotate() {
        // Using the mousePosition, makes a line going out from the camera
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, Mathf.Infinity)) { // Whenever the line finally hits the ground
            // This is the point where it hit the ground
            // we use transform.position.y instead of hit.point.y, because we want our player to point straight and not at the ground
            // transform.position.y is lined up with the player
            mousePositionInWorldSpace = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            // Debug.DrawLine(ray.origin, mousePosition, Color.blue); // Uncomment this code if you want to see the line its creating
        }
        // Turns the player to look at your mousePosition
        
        // Vector3 lookDirection = Vector3.Lerp(transform.position, mousePositionInWorldSpace, Time.deltaTime);
        // transform.LookAt(lookDirection);    

        direction = (mousePositionInWorldSpace - transform.position).normalized;
        rotGoal = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotGoal, turnSpeed);

    }
    // TODO: Animate the cylinder to go back when shooting like a recoil effect thing
    public IEnumerator Shoot(float time) {
        GameObject newBullet = (GameObject) Instantiate(bullet, firePoint.position, firePoint.rotation); 
        newBullet.GetComponent<Bullet>().player = true;
        StartCoroutine(shake.Shake(0.5f, 0.01f * b.damage));
        overrideVelocity = true;
        rb.AddForce(force * -transform.forward, ForceMode.Impulse);
        yield return new WaitForSeconds(time/2f);
        overrideVelocity = false;
        yield return new WaitForSeconds(time/2f);
        canShoot = true;
    }
}

