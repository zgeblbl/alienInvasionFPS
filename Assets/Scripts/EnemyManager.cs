using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    public int enemyHealth = 200;
    //navmesh
    public NavMeshAgent enemyAgent;
    public Transform player;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    //patrolling
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;
    //detect player
    public float sightRange, attackRange;
    public bool inSightRange, inAttackRange;
    //attack
    public float attackDelay;
    public bool isAttacking;
    public Transform attackPoint;
    public GameObject projectile;
    public float projectileForce = 18f;
    public Animator enemyAnimator;
    private GameManager gameManager;

    public TextMeshProUGUI enemyWinText;
    public ParticleSystem deadEffect;



    void Start()
    {
        enemyAgent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        enemyAnimator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
    }


    void Update()
    {
        inSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        inAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
        if (!inSightRange && !inAttackRange)
        {
            Patrolling();
            enemyAnimator.SetBool("Patrol", true);
            enemyAnimator.SetBool("Attack", false);
            enemyAnimator.SetBool("Detect", false);
        }
        else if (inSightRange && !inAttackRange)
        {
            DetectPlayer();
            enemyAnimator.SetBool("Patrol", false);
            enemyAnimator.SetBool("Attack", false);
            enemyAnimator.SetBool("Detect", true);
        }
        else if (inSightRange && inAttackRange)
        {
            AttackPlayer();
            enemyAnimator.SetBool("Patrol", false);
            enemyAnimator.SetBool("Attack", true);
            enemyAnimator.SetBool("Detect", false);
        }
    }
    void Patrolling()
    {
        if (!walkPointSet)
        {
            float randomZpos = Random.Range(-walkPointRange, walkPointRange);
            float randomXpos = Random.Range(-walkPointRange, walkPointRange);
            walkPoint = new Vector3(transform.position.x + randomXpos, transform.position.y, transform.position.z + randomZpos);
            if (!Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
            {
                walkPointSet = true;
                
            }
        }
        if (walkPointSet)
        {
            enemyAgent.SetDestination(walkPoint);   //from navmesh
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }
    void DetectPlayer()
    {
        enemyAgent.SetDestination(player.position);
        transform.LookAt(player);
    }
    void AttackPlayer()
    {
        enemyAgent.SetDestination(transform.position);
        transform.LookAt(player);
        if (!isAttacking)
        {
            Rigidbody rb = Instantiate(projectile, attackPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * projectileForce, ForceMode.Impulse);

            isAttacking = true;
            Invoke("ResetAttack", attackDelay);
        }
    }
    void ResetAttack()
    {
        isAttacking = false;
    }
    public void EnemyTakeDamage(int damageAmount)
    {
        enemyHealth -= damageAmount;
        if (enemyHealth <= 0)
        {
            EnemyDeath();
        }
    }
    void EnemyDeath()
    {
        Destroy(gameObject);
        gameManager.AddKill();
        gameManager.enemyNumber--;
        gameManager.killedEnemyNumber++;
        if (gameManager.killedEnemyNumber == 1)
        {
            enemyWinText.text = gameManager.killedEnemyNumber.ToString() + " ENEMY KILLED";
        }
        else if (gameManager.killedEnemyNumber >1)
        {
            enemyWinText.text = gameManager.killedEnemyNumber.ToString() + " ENEMIES KILLED";
        }
        Instantiate(deadEffect, transform.position, Quaternion.identity);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
