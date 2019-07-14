using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;

public class EnemyController : MonoBehaviour {
    public int hp = 100;
    public GameObject blood;

    public float moveSpeed = 3f;
    public float minDist = 10f;
    public float maxDist = 3000f;

    private Animator anim;
    private FirstPersonController player;
    private NavMeshAgent agent;
    private float hurtAnimationLength = 0.5f;
    private bool dead = false;

    void Start() {
        anim = GetComponent<Animator>();
        player = FindObjectOfType<FirstPersonController>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        Vector3 v = new Vector3(0, player.transform.position.y - transform.position.y, 0);
        transform.LookAt(player.transform.position - v);

        if (dead) {
            agent.isStopped = true;
        }
        else {
            agent.isStopped = false;
            float playerDistance = Vector3.Distance(transform.position, player.transform.position);
            if (playerDistance >= minDist && playerDistance <= maxDist) {
                agent.SetDestination(player.gameObject.transform.position);
            }
            else {
                agent.isStopped = true;
            }
        }
    }

    public void TakeDamage(int damage) {
        hp -= damage;
        if (hp <= 0) {
            Die();
        }
        else {
            anim.SetBool("hurt", true);
            StartCoroutine(ReactToDamage());
        }
    }

    private void Die() {
        dead = true;
        anim.SetBool("dead", true);
        Destroy(GetComponent<BoxCollider>());
    }

    IEnumerator ReactToDamage() {
        yield return new WaitForSeconds(hurtAnimationLength);
        anim.SetBool("hurt", false);
    }
}
