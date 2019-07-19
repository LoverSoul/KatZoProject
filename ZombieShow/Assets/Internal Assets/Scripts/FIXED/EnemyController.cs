using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Prefs")]
    public float health;
    public float maxHealth;
    public float damage;

   [Header("Speed Setting")]
    public float minimumSpeed = 2f;
    public float maximumSpeed = 2f;
    public float attackDistance = 1;

     GameObject player;
    [Header("Death and Score")]
    public bool activateDestructionProtocol;
    public bool givePointsToPlayer = false;
    float destrTimer = 1;
    byte point = 0;
    float speed;

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
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
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
        givePointsToPlayer = false;
        player.GetComponent<PlayerController>().health -= damage;
        player.GetComponent<PlayerController>().UIHealthDemonstration();
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

            
            activateDestructionProtocol = true;
        }
    }

    void TakeDamageSystem()
    {
        health -= player.GetComponent<PlayerController>().damage;
        if (health <= 0)
            givePointsToPlayer = true;
    }
}
