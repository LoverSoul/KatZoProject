using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerFixed : MonoBehaviour
{
    [HideInInspector]
    public GameObject enemyPrefab, enemyHitPrefab;
    
   // [Header("DONT FIX DOWN HERE, LOOK ON GAMEPLAY PREFERENCES")]
    [HideInInspector]
    public float enemyMinimumSpeed, enemyMaximumSpeed, enemyDamage, enemyHealth, enemyMaxHealth, enemyAttackDistance, bonusTimer;
    float _bonus;

    [Header("Time before spawn a new wave")]
    [Range(0, 2f)]
    public float timeBeforeSpawn = 1;
    float tbs;

    public AnimationCurve EnemiesSpawnCurve;
    public int InitialEnemyCounter = 10, CurrentEnemyCounter = 10;// EnemyCounterIncreaser = 5;
    float spawnTimer;
    Keyframe lastCurve;
    float curve;
    float cameraHeight;

    [Header("Spawn Enemy Radius")]
    public float minSpawnRadius = 16f;
    public float maxSpawnRadius = 32f;
    Coroutine spawn;

  

    [Header("UI Elements")]
    public Image redFillingImage;
    public Text enemiesCounterText;

    [Header("How many enemies player already kill")]
    public float AlreadyKilled = 0;
    [Header("How many enemies need to get killed")]
    public float enemiesNeedToKill = 100;

    [Header("Borders")]
    public GameObject collidersObj;
    public GameObject moveBorder;
    public float forwardBorderDebug = 2f;

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
        lastCurve = EnemiesSpawnCurve[EnemiesSpawnCurve.length-1];
        curve = lastCurve.time;
        
    }

    void Update()
    {

         if (!stopSpawnEnemies)
        {
            tbs -= Time.deltaTime;

            if (spawnTimer < curve)
                spawnTimer += Time.deltaTime;
            else
                spawnTimer = 0;

            float curveEva = EnemiesSpawnCurve.Evaluate(spawnTimer);
            int ce = (int)curveEva;
            CurrentEnemyCounter = ce;
            Debug.Log(tbs);
            

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
        if (moveBorder != null)
        {
            moveBorder.transform.localPosition = new Vector3(moveBorder.transform.localPosition.x, moveBorder.transform.localPosition.y - forwardBorderDebug, moveBorder.transform.localPosition.z);
        }
    }

    private IEnumerator SpawnWave(int enemiesToSpawn)
    {
       

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            float randomRadius = Random.Range(minSpawnRadius, maxSpawnRadius);

            Vector3 spawnPos = RandomCircle(transform.position, randomRadius);
            if (!stopSpawnEnemies)
            {
                GameObject _e = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                EnemyController ec = _e.GetComponent<EnemyController>();
                ec.enemyHitPrefab = enemyHitPrefab;
                ec.maxHealth = enemyMaxHealth;
                ec.health = enemyHealth;
                ec.minimumSpeed = enemyMinimumSpeed;
                ec.maximumSpeed = enemyMaximumSpeed;
                ec.damage = enemyDamage;
                ec.attackDistance = enemyAttackDistance;
            }
            yield return new WaitForSeconds(1);
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
