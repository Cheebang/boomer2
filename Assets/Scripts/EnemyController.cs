using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;

public class EnemyController : MonoBehaviour {
    public int hp = 100;
    public float moveSpeed = 3f;
    public float minDist = 10f;
    public float maxDist = 300f;
    public GameObject projectile;

    private Animator anim;
    private FirstPersonController player;
    private NavMeshAgent agent;
    private float hurtAnimationLength = 0.5f;
    private bool dead;
    private bool isAttacking = false;
    private float attackCoolDownTime = 2f;
    private float projectileSpeed = 15;
    private bool knowsPlayerPosition;
    private float sightDistance = 50f;

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
            return;
        }

        if (!knowsPlayerPosition) {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, sightDistance)) {
                GameObject item = hit.collider.gameObject;
                if (hit.collider.CompareTag("Player")) {
                    knowsPlayerPosition = true;
                }
            }
        }

        if (knowsPlayerPosition) {
            agent.isStopped = false;
            if (!isAttacking) {
                isAttacking = true;
                GameObject newProjectile = Instantiate(projectile, transform.position, transform.rotation);
                newProjectile.GetComponent<Rigidbody>().velocity = (player.transform.position - transform.position).normalized * projectileSpeed;
                StartCoroutine(Attack());
            }
            float playerDistance = Vector3.Distance(transform.position, player.transform.position);
            if (playerDistance >= minDist && playerDistance <= maxDist) {
                agent.SetDestination(player.gameObject.transform.position);
            }
            else {
                knowsPlayerPosition = false;
                agent.isStopped = true;
            }
        }
    }

    IEnumerator Attack() {
        yield return new WaitForSeconds(attackCoolDownTime);
        isAttacking = false;
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
