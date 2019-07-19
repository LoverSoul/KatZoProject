using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerFixed : MonoBehaviour
{
    public GameObject EnemyPrefab;
    [Header("DONT FIX HERE, LOOK ON GAMEPLAY PREFERENCES")]
    public float enemyMinimumSpeed;
    public float enemyMaximumSpeed;
    public float enemyDamage;
    public float enemyHealth;
    public float enemyMaxHealth;
    public float enemyAttackDistance;
    public float bonusTimer;
    float _bonus;

    [Header("Spawn Enemies Counter")]
    public int InitialEnemyCounter = 10;
    public int CurrentEnemyCounter = 10;
    public int EnemyCounterIncreaser = 5;

    float cameraHeight;

    [Header("Spawn Enemy Radius")]
    public float minSpawnRadius = 16f;
    public float maxSpawnRadius = 32f;
    Coroutine spawn;

    [Header("Time before spawn a new wave")]
    public float timeBeforeSpawn = 3;
    float tbs;
   

    [Header("UI Elements")]
    public Image redFillingImage;
    public Text enemiesCounterText;
    public GameObject restart;
    [Header("How many enemies player already kill")]
    public float AlreadyKilled = 0;
    [Header("How many enemies need to get killed")]
    public float enemiesNeedToKill = 100;

    [Header("Borders")]
    public GameObject collidersObj;

    [Header("Stop Game")]
    public bool stopSpawnEnemies;

    [Header("UI Bonus Element")]
    public Image bonusFillImage;
    public Text bonusText;
    public bool giveBonusToPlayer = true;

    private void Start()
    {
        
       
        CheckOfKillingEnemy();
        BordersColliderCreator();
        tbs = timeBeforeSpawn;
        _bonus = bonusTimer;
       
    }

    void Update()
    {
        if (!stopSpawnEnemies)
        {
            tbs -= Time.deltaTime;
            if (tbs <= 0)
            {
                spawn = StartCoroutine(SpawnWave(CurrentEnemyCounter));
                tbs = timeBeforeSpawn;
            }
        }
        BonusTimerConfigure();
    }




    void BordersColliderCreator()
    {
        cameraHeight = Camera.main.transform.position.y / 15;

        Vector2 screenScale = new Vector2(Screen.width, Screen.height).normalized;
        float x = screenScale.x * cameraHeight;
        float y = screenScale.y * cameraHeight;
        collidersObj.transform.localScale = new Vector3(x,y, 1);
    }

    private IEnumerator SpawnWave(int enemiesToSpawn)
    {
        CurrentEnemyCounter += EnemyCounterIncreaser;


        for (int i = 0; i < enemiesToSpawn; i++)
        {
            float randomRadius = Random.Range(minSpawnRadius, maxSpawnRadius);

            Vector3 spawnPos = RandomCircle(transform.position, randomRadius);
            if (!stopSpawnEnemies)
            {
                GameObject _e = Instantiate(EnemyPrefab, spawnPos, Quaternion.identity);
                EnemyController ec = _e.GetComponent<EnemyController>();
                ec.maxHealth = enemyMaxHealth;
                ec.health = enemyHealth;
                ec.minimumSpeed = enemyMinimumSpeed;
                ec.maximumSpeed = enemyMaximumSpeed;
                ec.damage = enemyDamage;
                ec.attackDistance = enemyAttackDistance;
            }
            yield return new WaitForSeconds(Random.Range(0f, 0.3f));
        }
    }

    Vector3 RandomCircle(Vector3 center, float radius)
    {
        float ang = Random.Range(0f, 360f);
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }


    public void CheckOfKillingEnemy()
    {
        enemiesCounterText.text = "Enemies Get Killed: " + Mathf.Round(AlreadyKilled);
        redFillingImage.fillAmount = AlreadyKilled / enemiesNeedToKill;
   

        if (AlreadyKilled >= enemiesNeedToKill)
        {
            if (tbs != 1000)
            {
                stopSpawnEnemies = true;
                StopCoroutine(spawn);
                FindAndKillAllEnemies();
                tbs = 1000;
            }
        }
    }

    public void FindAndKillAllEnemies()
    {
        GameObject[] enemies;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (int x = 0; x < enemies.Length; x++)
        {
            enemies[x].GetComponent<EnemyController>().givePointsToPlayer = false;
            enemies[x].GetComponent<EnemyController>().activateDestructionProtocol = true;
            
        }
    }

    void BonusTimerConfigure()
    {
        if (_bonus > 0)
        {
            _bonus -= Time.deltaTime;
            bonusText.text = "Bonus: " + _bonus.ToString("0");
            bonusFillImage.fillAmount = _bonus/bonusTimer;
        }
        else
        {
            _bonus = 0;
            giveBonusToPlayer = false;
            bonusText.text = "Bonus: " + _bonus.ToString("0");
            bonusFillImage.fillAmount = 0;
            return;
        }
        
    }
}
