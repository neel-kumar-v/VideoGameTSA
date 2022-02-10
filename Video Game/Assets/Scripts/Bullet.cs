using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Stats")]
    public float reload;
    public float speed;
    public float damage;
    public bool explosive;
    public float explosionDamage;
    public float explosionRadius;
    public float weight;
    public float pierce;

    [Header("Unity Setup")] 
    public bool player;
    public Rigidbody rb;
    public GameObject particles;
    public Transform playerPos;


    public void Start() {
        Shoot(); // Move the bullet forward

        // If this is a player bullet, find the enemy transform, otherwise find the player's transform
        playerPos = player ? GameObject.FindGameObjectWithTag("Player").transform : GameObject.FindGameObjectWithTag("Enemy").transform;
        transform.LookAt(playerPos);
        transform.rotation *= Quaternion.Euler(0, -90, 0); // For some reason, line 29 gives a 90 degree offset, so this un-offsets it
        Destroy(gameObject, 5f); // Only let the bullet last for 5 seconds
    }

    void Shoot() {
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    void Update() {
        if(pierce <= 0) { // Destroy the bullet immediately after it runs out fo pierce
            DestroyBullet();
        }
    }

    public void OnTriggerEnter(Collider collider) { // When the bullet hits something

        GameObject hit = collider.gameObject;

        if(hit.CompareTag("obstacle")) { // If hit an obstacle
            DestroyBullet();
            return;
        }

        Bullet b = hit.GetComponent<Bullet>();

        bool didHitSelf = (hit.CompareTag("Player") && player) || (hit.CompareTag("Enemy") && !player);
        bool didHitOwnBullet = (hit.CompareTag("Bullet") && b.player == player);
        if(didHitSelf || didHitOwnBullet) return;

        if(hit.CompareTag("Bullet")) { // Take away pierce if hit enemy bullet
            pierce -= b.pierce;
            return;
        }
        
        ApplyDamage(hit, damage);

        if(explosive) { // If this bullet is explosive
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius); // Check all objects in the explosionRadius
            foreach (Collider col in colliders)
            {
                if(col.gameObject.CompareTag("Obstacle")) return; // As long as it isn't an obstacle, apply the damage
                ApplyDamage(col.gameObject, explosionDamage);
            }
        }
    }

    public void ApplyDamage(GameObject hit, float damage) {
        // As long as it didn;t hit its own player
        if(hit.CompareTag("Player") && !player || hit.CompareTag("Enemy") && player) {
            Health health = hit.GetComponent<Health>();
            health.ApplyDamage(damage);
            pierce = 0;
        } 
    }

    public void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius); // Shows the size of the explosion radius in the editor
    }

    void DestroyBullet() {
        Destroy((GameObject) Instantiate(particles, transform.position, transform.rotation), 3f);
        Destroy(gameObject);
    }
}
