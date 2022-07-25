using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Unity Setup")]
    
    public AnimationCurve xCurve;

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
    public float force = 7000f;
    float reloadTime;
    public float reloadClip;
    public float clipSize;
    [HideInInspector] public float currentClip;
    public float countdown;

    public GameObject grenade;

    public float turnSpeed = 0.01f;
    Quaternion rotGoal;
    Vector3 direction;

    [HideInInspector] public Vector3 mousePositionInWorldSpace;
    Vector3 movement;

    [HideInInspector] public bool canShoot; 

    [HideInInspector] public bool canMove;

    public void ToggleCanMove(bool plsMove) {
        canMove = plsMove;
    }
    public void ToggleCanShoot(bool plsShoot) {
        canShoot = plsShoot;
    }

    bool overrideVelocity = false;

    public Bullet b;

    public bool canGrenadeShoot;
    public CameraFollow follow;

    public bool inverse;

    public bool isController = false;

    public RightJoyTurn turn;

    public static float sensitivity = 1f;

    public float deadzone;

    public float xPos;
    public float yPos;
    public Vector3 pos;
    private Vector3 turnVelocity = Vector3.zero;
    public RectTransform box;

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
        reloadClip = b.reloadClip; 
        clipSize = b.clipSize; 
        currentClip = clipSize;
    }

    public IEnumerator Countdown(float time) {
        canMove = false;
        yield return new WaitForSeconds(time);
        canMove = true;
    }

    void Update()
    {
        if(!canMove) return;
        if(isController) {
            Turn();
        } else {
            Rotate();
        }
        if(Input.GetButton("Fire1") && canShoot) {
            if(EventSystem.current.IsPointerOverGameObject()) return;
            canShoot = false;
            StartCoroutine(Shoot(reloadTime));
        }
     }

     public IEnumerator ReloadPause(float reload) {
         canShoot = false;
         yield return new WaitForSeconds(reload);
         currentClip = clipSize;
         canShoot = true;
     }

    
    public void Turn()
    {
        float x = Input.GetAxis("Horizontal1");
        if(x < deadzone && x > -deadzone) {
            x = 0;
        }
        float y = Input.GetAxis("Vertical1");
        if(y < deadzone && y > -deadzone) {
            y = 0;
        }

        yPos += y * sensitivity;
        xPos += -x * sensitivity;

        pos = Vector3.SmoothDamp(pos, new Vector3(xPos, 2f, yPos), ref turnVelocity, 0.04f);
        direction = (pos - transform.position).normalized;
        rotGoal = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotGoal, turnSpeed);
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
        if(isController) {

            float x = Input.GetAxisRaw("Horizontal2");
            if(x < deadzone && x > -deadzone) {
                x = 0;
            }
            float y = Input.GetAxisRaw("Vertical2");
            if(y < deadzone && y > -deadzone) {
                y = 0;
            }

            movement = new Vector3(x, 0f, y);
            rb.velocity = 1.17f * movement * speed * (inverse ? -1f : 1f);
        } else {
            movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            rb.velocity = 1.17f * movement * speed * (inverse ? -1f : 1f);
        }
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
    public IEnumerator Shoot(float time) {
        currentClip --;

        GameObject newBullet = (GameObject) Instantiate(bullet, firePoint.position, firePoint.rotation); 
        newBullet.GetComponent<Bullet>().player = true;

        StartCoroutine(shake.Shake(0.5f, 0.01f * b.damage));

        overrideVelocity = true;
        rb.AddForce(force * -transform.forward, ForceMode.Impulse);

        cylinder.transform.localPosition = new Vector3(0f ,0f ,0.3f);


        yield return new WaitForSeconds(time / 2f);

        StartCoroutine(CylinderMove(time));

        overrideVelocity = false;
        StartCoroutine(BlendMove());

        yield return new WaitForSeconds(time / 2f);

        overrideVelocity = false;
        canShoot = true;

        
        if(currentClip == 0) {
            StartCoroutine(ReloadPause(reloadClip));
        }
    }

    public IEnumerator CylinderMove(float duration) {
        float timeElapsed = 0f;
        while(timeElapsed <= duration) {
            cylinder.transform.localPosition = new Vector3(0f, 0f, xCurve.Evaluate(timeElapsed / duration));
            timeElapsed += Time.deltaTime;
        }
        yield return null;
    }
    public IEnumerator BlendMove() {
        float timeElapsed = 0f;
        while(timeElapsed <= 0.5f) {
            movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            rb.velocity = Vector3.Lerp(rb.velocity, movement * speed, 0.5f + timeElapsed);
            timeElapsed += Time.deltaTime;
        }
        yield return null;
    }

}

