using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodStain : MonoBehaviour {
    private ParticleSystem part;
    public List<ParticleCollisionEvent> collisionEvents;
    public GameObject bloodStain;

    void Start() {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other) {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        bool map = other.CompareTag("Map");
        for (int i = 0; i < numCollisionEvents; i++) {
            if (map) {
                Vector3 hitPoint = collisionEvents[i].intersection;
                GameObject stain = Instantiate(bloodStain);
                stain.transform.position = hitPoint;
                stain.transform.rotation = Quaternion.FromToRotation(Vector3.up, collisionEvents[i].normal);
            }
        }
    }
}