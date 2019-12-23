using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Prefs")]
    public float health;
    public float maxHealth;
    public float damage;
    public bool getPushedBack;

   [Header("Speed Setting")]
    public float minimumSpeed = 2f;
    public float maximumSpeed = 2f;
    public float attackDistance = 1;
    public float distanceOfPushing = 2;

     GameObject player;
    [Header("Death and Score")]
    public bool activateDestructionProtocol;
    public bool givePointsToPlayer = false;
    public GameObject enemyHitPrefab;
    float destrTimer = 1;
    byte point = 0;
    float speed;
    byte pushInit = 0;
    Vector3 push;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        speed = Random.Range(minimumSpeed, maximumSpeed);
        health = maxHealth;
    }
 

    // Update is called once per frame
    void Update()
    {
        if (!activateDestructionProtocol)
        {
            FollowTarget();
            ControlDistance();
        }
        else
            SelfDestrucionProtocol();
    }

    void FollowTarget()
    {
        if (!getPushedBack)
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        else
            PushBackward();
    }

    void SelfDestrucionProtocol()
    {
        destrTimer -= Time.deltaTime;
        transform.localScale += new Vector3(.05f, .05f, .05f);
        GetComponent<Renderer>().material.color -= new Color(0, 0, 0, .05f);
        if (givePointsToPlayer)
            GivePointsToPlayer();

        if (destrTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    void AttackPlayerSystem()
    {
        if (pushInit == 0)
        {
            givePointsToPlayer = false;
            player.GetComponent<PlayerController>().health -= damage;
            player.GetComponent<PlayerController>().UIHealthDemonstration();
            player.GetComponent<PlayerController>().hitEffect.PlayerWasHit();
            activateDestructionProtocol = true;
        }
    }

    void GivePointsToPlayer()
    {
        if (point == 0)
        {
            if (GameObject.FindGameObjectWithTag("GameController") != null)
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerFixed>().AlreadyKilled++;
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControllerFixed>().CheckOfKillingEnemy();

            }
            point++;
        }
    }

    void ControlDistance()
    {

        if (Vector3.Distance(transform.position, player.transform.position) < attackDistance)
        {
            if (!GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().imAttacking)
                AttackPlayerSystem();
            else
                TakeDamageSystem();

        }
    }

    void TakeDamageSystem()
    {
        
        if (pushInit==0)
            health -= player.GetComponent<PlayerController>().damage;

        if (health <= 0)
        {
            player.GetComponent<PlayerController>().imHitEnemy = true;
            activateDestructionProtocol = true;

            givePointsToPlayer = true;
            if (enemyHitPrefab != null)
            {
                GameObject deathParticles = Instantiate(enemyHitPrefab);
                Destroy(deathParticles, 2);
            }
        }
        
        else
            getPushedBack = true;
        
    }

    void PushBackward()
    {
        if (pushInit == 0)
        {
            push = transform.position + player.transform.position;
            pushInit++;
        }

        transform.position = Vector3.MoveTowards(transform.position, push, speed * 10 * Time.deltaTime);
        if (Vector3.Distance(transform.position, push) < distanceOfPushing)
        {
            pushInit = 0;
            getPushedBack = false;
        }
    }
}
