using System;
using System.Collections;
using UnityEngine;

public class ExplodeScript : MonoBehaviour {
    public float timeToExplode;
    public GameObject explosionEffect;
    public float blastRadius = 5;
    public float blastForce = 1000;
    public float blastDuration = 2f;
    public bool explodeOnCollision = false;

    private bool hasExploded;

    void Update() {
        if (explodeOnCollision) {
            return;
        }
        else if (timeToExplode <= 0) {
            Explode();
        }
        else {
            timeToExplode -= Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision collision) {
        Explode();
    }

    private void Explode() {
        if (hasExploded) {
            return;
        }

        hasExploded = true;
        GetComponent<MeshRenderer>().enabled = false;

        GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
        StartCoroutine(RenderExplosion(explosion));

        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, blastRadius);
        foreach (Collider nearbyObject in nearbyObjects) {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null) {
                rb.AddExplosionForce(blastForce, transform.position, blastRadius);
                //Damage if damageable
            }
        }
    }

    IEnumerator RenderExplosion(GameObject explosion) {
        yield return new WaitForSeconds(blastDuration);
        Destroy(explosion);
        Destroy(gameObject);
    }
}
