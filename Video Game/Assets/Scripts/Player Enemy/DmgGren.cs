using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgGren : MonoBehaviour
{
   public float delay = 6f;
   public float radius = 5f;
   public float force = 700f;

   float countdown;
   bool hasExploded = false;

   void Start() {
       countdown = delay;
   }

   void Update() {
       countdown -= Time.deltaTime;
       if (countdown <= 0f && !hasExploded) {
           Explode();
           hasExploded = true;
       }
   }

   void Explode() {
       
       Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

       foreach (Collider nearbyObject in colliders) {

           Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();

           if (rb != null) {

               rb.AddExplosionForce(force, transform.position, radius);
           }
       }

            // Dmg

       Destroy(gameObject);

   }
}
