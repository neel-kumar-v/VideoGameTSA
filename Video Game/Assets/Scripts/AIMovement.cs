using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    [Header("Parameters")]
    public float speed;
    public float chaseDistance;
    public float retreatDistance;
    public float reloadTime;
    public float range;
    public float offset;
    public float dodgeRange;
    [Range(0f, 1f)] public float dodgeReaction;


    [Header("Unity Setup")]
    public GameObject bullet;
    public Transform firePoint;
    public Transform rangePoint;
    public LayerMask layerMask;
    public float countdown;
    public Transform player;
    public Vector3 startPos;

    private Rigidbody rb;
    private bool canShoot;
    private bool canDodge;
    private Bullet b;

    private Vector3 posToStartRangeAt;
    private float distance;
    private Vector3 vel;

    private Renderer pRend;

    public void OnEnable() {
        Reset();
    }

    public void Reset() {
        b = bullet.GetComponent<Bullet>(); // Reset the variable b just in case the bullet changed because the player bought a new gun
        speed /= b.weight; // Reset the speed variable correctly
        reloadTime = b.reload; // Reset the reload variable correctly

        // Reset player position and rotation
        transform.position = startPos; 
        transform.LookAt(Vector3.Lerp(transform.position, new Vector3(0f, 0f, 0f), Time.deltaTime));

        if(rb != null) rb.velocity = Vector3.zero; // Reset velocity
        
        // These 2 variables will be found again in the countdown function
        player = null;
        rb = null;

        StartCoroutine(Countdown(countdown));
    }

    public IEnumerator Countdown(float time) {
        canShoot = false; // Can't shoot until after the time has passed

        transform.position = new Vector3(15f * (float) (Random.Range(0, 2) * 2 - 1), 2f, -15f); // Picks between 3 random positions 

        yield return new WaitForSeconds(time); // This waits for the time to pass

        canShoot = true; // After the time has passed, now it can shoot

        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        pRend = player.gameObject.GetComponent<Renderer>();
    }

    void FixedUpdate() {
        if(player == null || rb == null || !pRend.enabled) return; // If any of these things are null or disabled, it means the game currently isn't playing, so don't chase

        // if(AvoidBullets()) return; --This code can be enabled if we want the AI to dodge all of the player's bullets, but it doesn't look right so i disabled it

        distance = Vector3.Distance(transform.position, player.position); // The distance in between the player and the AI
        vel = (transform.position - player.position).normalized; // The direction to chase the player in

        if(distance > chaseDistance) { // If far away, chase the player
            rb.velocity = vel * -speed;
        } else if(distance < chaseDistance && distance > retreatDistance) { // If midrange slow down
            rb.velocity *= 0.95f;   
        } else if(distance < retreatDistance) { // If too close, run away
            rb.velocity = vel * speed;
        } else { // Otherwise, just chase the player
            rb.velocity = vel * -speed;
        }
    }

    public bool AvoidBullets() {
        Bullet[] bullets = GameObject.FindObjectsOfType<Bullet>();

        Bullet correctBullet = null;
        distance = Mathf.Infinity;

        foreach (Bullet bul in bullets)
        {
            if(bul.player) {
                float newDistance = Vector3.Distance(transform.position, bul.transform.position);
                if(distance > newDistance) {
                    distance = newDistance;
                    correctBullet = bul;
                }
            }
        }
        Debug.Log(correctBullet != null);
        if(correctBullet != null) {
            vel = (transform.position - correctBullet.transform.position).normalized;

            if(distance < dodgeRange) {
                rb.AddForce(vel, ForceMode.Impulse);
                vel = vel * speed;
            }
        }
        return false;
    }




    void Update() {
        if(player == null) return; // Don't run code unless we found the player
        
        transform.LookAt(Vector3.Lerp(transform.position, player.position, Time.deltaTime));
        if(CheckAim() && canShoot) {
            canShoot = false;
            StartCoroutine(Shoot(reloadTime));
        }
    }

    public IEnumerator Shoot(float time) {
        GameObject newBullet = (GameObject) Instantiate(bullet, firePoint.position, firePoint.rotation); 
        newBullet.GetComponent<Bullet>().player = false;
        yield return new WaitForSeconds(time);
        canShoot = true;
    }

    public bool CheckAim() {
        // -- This code used to make the AI only chase the player if they were directly in the line of sight --
        // TODO: try putting this inside the if loop using player position instead of a forward vector
            // RaycastHit hit;
            // if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 200f, layerMask)) {
            //     Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //     return true;
            // } 
            // return false;
        posToStartRangeAt = rangePoint.position; // This makes it so that the range starts at the end of the gun and not at the player, which worked better
        Collider[] colliders = Physics.OverlapSphere(posToStartRangeAt, range); // Find all objects within range
        foreach(Collider collider in colliders) {
            GameObject possiblePlayer = collider.gameObject; // Get the game object of each object
            if(possiblePlayer.CompareTag("Player")) { // If the game object is tagged "Player", then it is the player
                return true;
            }
        }
        return false;
    }
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position to show the range of the player in Unity, but not in the gane
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(posToStartRangeAt, range);
    }
}
