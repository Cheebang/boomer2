using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    public int hp = 100;
    public GameObject blood;

    public float minDist = 2;
    public float moveSpeed = 3;
    public float maxDist = 10;

    private Animator anim;
    private FireWeapon player;
    private NavMeshAgent agent;
    private float hurtAnimationLength = 0.5f;
    private bool dead = false;

    void Start() {
        anim = GetComponent<Animator>();
        player = FindObjectOfType<FireWeapon>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        //transform.LookAt(player.gameObject.transform);

        Vector3 v = player.gameObject.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(player.gameObject.transform.position - v);

        if (dead) {
            agent.isStopped = true;
        }
        else {
            agent.isStopped = false;
            if (Vector3.Distance(transform.position, player.transform.position) >= minDist) {
                agent.SetDestination(player.gameObject.transform.position);

                if (Vector3.Distance(transform.position, player.transform.position) <= maxDist) {
                    // TODO attack player
                }
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
