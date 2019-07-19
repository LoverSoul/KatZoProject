using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour, IController
{
    public GameObject EnemyPrefab;
  
    public float WaveRate = 2f;

    public int InitialEnemyCounter = 10;
    public int CurrentEnemyCounter = 10;
    public int EnemyCounterIncreaser = 5;

    public float SpawnRadius = 16f;

    IDisposable timeCoroutine;

    [Header("UI Elements")]
    public Image redFillingImage;
    public Text enemiesCounterText;
    [Header("How many enemies player already kill")]
    public float AlreadyKilled = 0;
    [Header("How many enemies need to get killed")]
    public float enemiesNeedToKill = 100;

    [Header("Borders")]
    public GameObject collidersObj;


    private void Awake ()
	{
	    CurrentEnemyCounter = InitialEnemyCounter;

	   timeCoroutine = Observable.Timer(TimeSpan.FromSeconds(WaveRate))
	        .RepeatSafe()
	        .Subscribe(x =>
	        {
	          StartCoroutine(SpawnWave(CurrentEnemyCounter));
	            CurrentEnemyCounter += EnemyCounterIncreaser;
	        });
        CheckOfKillingEnemy();
        BordersColliderCreator();


    }




   

    void BordersColliderCreator()
    {
        Vector2 screenScale = new Vector2(Screen.width, Screen.height).normalized;
        collidersObj.transform.localScale = new Vector3(screenScale.x, screenScale.y, 1);
    }

    private IEnumerator SpawnWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            var randomAngle = Random.Range(0f, 360f);
            var spawnPos = new Vector3(Mathf.Cos(randomAngle) * SpawnRadius, Mathf.Sin(randomAngle) * SpawnRadius);
            Instantiate(EnemyPrefab, spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(0f, 0.3f));
        }
    }

    public Type ControllerType => typeof(GameController);

    public void Initialize()
    {
		
    }


    public void CheckOfKillingEnemy()
    {
        enemiesCounterText.text = "Enemies Get Killed: " + Mathf.Round(AlreadyKilled);
        redFillingImage.fillAmount = AlreadyKilled / enemiesNeedToKill;
        Debug.Log(AlreadyKilled / enemiesNeedToKill);

        if (AlreadyKilled >= enemiesNeedToKill)
        {

            timeCoroutine.Dispose();

         
        }
    }

    public void OnLevelLoad()
    {
		
    }

    public void EnableController()
    {
		
    }

    public void DisableController()
    {
		
    }
}
