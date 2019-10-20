using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;
using System.Linq;

public class EnemyController : MonoBehaviour {
    public int hp = 100;
    public float moveSpeed = 3f;
    public float minDist = 10f;
    public float maxDist = 300f;
    public GameObject projectile;
    public bool dead = false;
    public bool dumb;
    public Transform[] patrolRoute;

    private int destPoint = 0;

    public string uniqueId;

    private Animator anim;
    private FirstPersonController player;
    private NavMeshAgent agent;
    private EnemyCollection enemyCollection;
    private float hurtAnimationLength = 0.5f;
    private bool isAttacking = false;
    private float attackCoolDownTime = 2f;
    private float projectileSpeed = 15;
    private bool knowsPlayerPosition;
    private float sightDistance = 50f;
    private Vector3 initialPos;
    private int startHp;
    private bool waiting = false;

    private float currentPatrolPauseTime = 0;
    public float patrolPauseLength = 3;

    void Start() {
        uniqueId = UniqueId.Generate(gameObject);

        GameEvents.LoadInitiated += Load;

        anim = GetComponentInChildren<Animator>();
        player = FindObjectOfType<FirstPersonController>();
        agent = GetComponent<NavMeshAgent>();
        enemyCollection = FindObjectOfType<EnemyCollection>();

        initialPos = transform.position;
        startHp = hp;

        agent.autoBraking = false;
        GotoNextPoint();
    }

    public void Load() {
        EnemyData matchingData = enemyCollection.enemies.First(c => c.uniqueId == uniqueId);
        if (matchingData != null) {
            hp = matchingData.hp;
            dead = matchingData.dead;
            transform.position = new Vector3(matchingData.position[0], matchingData.position[1], matchingData.position[2]);
            if (dead) {
                Die();
            }
        }
    }

    private void OnDestroy() {
        GameEvents.LoadInitiated -= Load;
    }

    void GotoNextPoint() {
        if (patrolRoute.Length == 0) {
            return;
        }
        agent.isStopped = false;
        anim.SetBool("walking", true);
        agent.destination = patrolRoute[destPoint].position;
        destPoint = (destPoint + 1) % patrolRoute.Length;
    }

    void Update() {
        if (dumb || dead) {
            agent.isStopped = true;
            return;
        }

        if (!knowsPlayerPosition) {
            Search();
        }

        if (knowsPlayerPosition) {
            ChaseAndAttack();
        }
    }

    private void Search() {
        if (waiting) {
            agent.isStopped = true;
            anim.SetBool("walking", false);
            currentPatrolPauseTime += Time.deltaTime;

            if (currentPatrolPauseTime >= patrolPauseLength) {
                waiting = false;
                currentPatrolPauseTime = 0;
                GotoNextPoint();
            }
        }
        if (!agent.pathPending && agent.remainingDistance < 0.5f) {
            waiting = true;
        }

        bool withinAngle = false;
        Vector3 targetDir = player.transform.position - transform.position;
        float angleToPlayer = Vector3.Angle(targetDir, transform.forward);
        if (angleToPlayer <= 85) {
            withinAngle = true;
        }

        if (withinAngle) {
            Ray ray = new Ray(transform.position, targetDir);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, sightDistance)) {
                GameObject item = hit.collider.gameObject;
                if (hit.collider.CompareTag("Player")) {
                    knowsPlayerPosition = true;
                }
            }
        }
    }

    private void ChaseAndAttack() {
        destPoint = 0;
        Vector3 targetDir = player.transform.position - transform.position;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDir), Time.time * (moveSpeed / 2));
        agent.isStopped = false;
        anim.SetBool("walking", true);

        if (!isAttacking) {
            isAttacking = true;
            StartCoroutine(Attack());
        }
        float playerDistance = Vector3.Distance(transform.position, player.transform.position);
        if (playerDistance >= minDist && playerDistance <= maxDist) {
            agent.SetDestination(player.gameObject.transform.position);
        }
        else {
            knowsPlayerPosition = false;
            agent.isStopped = true;
            anim.SetBool("walking", false);
        }
    }

    IEnumerator Attack() {
        yield return new WaitForSeconds(attackCoolDownTime);
        isAttacking = false;
        if (!dead) {
            Vector3 newProjectilePos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            GameObject newProjectile = Instantiate(projectile, newProjectilePos, transform.rotation);
            newProjectile.GetComponent<Rigidbody>().velocity = (player.transform.position - transform.position).normalized * projectileSpeed;
        }
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
            knowsPlayerPosition = true;
            StartCoroutine(ReactToDamage());
        }
    }

    public void Die() {
        dead = true;
        anim.SetBool("walking", false);
        anim.SetBool("dead", true);
        GetComponent<Collider>().enabled = false;
    }

    IEnumerator ReactToDamage() {
        yield return new WaitForSeconds(hurtAnimationLength);
        anim.SetBool("hurt", false);
    }
}
