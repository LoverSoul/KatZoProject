using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayPreferences : MonoBehaviour
{

    [Header("Player Configuration")]
    PlayerController player;
    public GameObject slashPrefab;
    public  float playerHealth;
    public  float playerMaxHealth;
    public  float playerRotationSpeed;
    public float playerMovementSpeed;
    public  float playerDamage;

    [Header("Enemy Configuration")]
    GameControllerFixed controller;
    public GameObject enemy;
    public GameObject enemyHitPrefab;
    public float enemyHealth;
    public float enemyDamage;
    public float enemyMinimumSpeed;
    public float enemyMaximumSpeed;
    public float enemyAttackDistance = 1.4f;

    [Header("Scene Configuration")]
    [Range(0.05f, 1)]
    public float timeSlowsDown;
    public float bonusTimer;

    [Header("UI Buttons")]
    public GameObject pauseButton;
    public GameObject resumeButton;
    public GameObject restartButton;
    public GameObject uiPauseElement;



    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        controller = GetComponent<GameControllerFixed>();
        UpdateConfiguration();
    }

    public void UpdateConfiguration()
    {
        player.maxHealth = playerMaxHealth;
        player.health = playerHealth;
        player.rotationSpeed = playerRotationSpeed;
        player.movementSpeed = playerMovementSpeed;
        player.damage = playerDamage;
        player.timeSlowsDown = timeSlowsDown;
        player.slashPrefab = slashPrefab;
        controller.enemyPrefab = enemy;
        controller.enemyHitPrefab = enemyHitPrefab;
        controller.enemyHealth = enemyHealth;
        controller.enemyMinimumSpeed = enemyMinimumSpeed;
        controller.enemyMaximumSpeed = enemyMaximumSpeed;
        controller.enemyDamage = enemyDamage;
        controller.enemyAttackDistance = enemyAttackDistance;
        controller.bonusTimer = bonusTimer;
       

    }

    public void Pause()
    {
       
        player._directionLine.enabled = false;
        player.gameplayIsActive = false;
        player.doMovement = false;
        player._directionLine.SetPosition(1, player.transform.position);
        restartButton.SetActive(false);
        pauseButton.SetActive(false);
        resumeButton.SetActive(true);
        uiPauseElement.SetActive(true);
        Time.timeScale = .00001f;



    }

    public void Resume()
    {
        Time.timeScale = 1f;
        player.gameplayIsActive = true;
        restartButton.SetActive(true);
        pauseButton.SetActive(true);
        resumeButton.SetActive(false);
        uiPauseElement.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
