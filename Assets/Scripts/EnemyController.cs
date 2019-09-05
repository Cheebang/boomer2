using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;

public class EnemyController : MonoBehaviour {
    public int hp = 100;
    public float moveSpeed = 3f;
    public float minDist = 10f;
    public float maxDist = 300f;
    public GameObject projectile;
    public bool dead = false;
    public bool dumb;

    private Animator anim;
    private FirstPersonController player;
    private NavMeshAgent agent;
    private float hurtAnimationLength = 0.5f;
    private bool isAttacking = false;
    private float attackCoolDownTime = 2f;
    private float projectileSpeed = 15;
    private bool knowsPlayerPosition;
    private float sightDistance = 50f;
    private Vector3 initialPos;
    private int startHp;

    void Start() {
        anim = GetComponent<Animator>();
        player = FindObjectOfType<FirstPersonController>();
        agent = GetComponent<NavMeshAgent>();

        initialPos = transform.position;
        startHp = hp;
    }

    void Update() {
        if (dumb) {
            return;
        }

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
            Vector3 v = new Vector3(0, player.transform.position.y - transform.position.y, 0);
            transform.LookAt(player.transform.position - v);
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

    internal void Reset() {
        transform.position = initialPos;
        dead = false;
        hp = startHp;
        GetComponent<Animator>().SetBool("dead", false);
        GetComponent<Animator>().Play("Idle", -1, 0f);
        GetComponent<Collider>().enabled = true;
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

    public void Die() {
        dead = true;
        GetComponent<Animator>().SetBool("dead", true);
        GetComponent<Collider>().enabled = false;
    }

    IEnumerator ReactToDamage() {
        yield return new WaitForSeconds(hurtAnimationLength);
        anim.SetBool("hurt", false);
    }
}
