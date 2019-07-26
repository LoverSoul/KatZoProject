using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Pause, death, gameplay stop")]
    public bool gameplayIsActive = true;
    public GameObject slashPrefab;
    [SerializeField]
    public LineRenderer _directionLine;

    public float damage;

    public float health;

    public float maxHealth;


    public float rotationSpeed = 10;

    public float movementSpeed = 5;

    public float timeSlowsDown;
    public GameObject navigator;

    public LayerMask colliders;
    Coroutine _rotateCoroutine;
    public bool doMovement;
    public bool imAttacking;
    public bool imHitEnemy;

    [Header("Player Canvas")]
    public Canvas HealthCanvas;
    public Image playerHealthImage;
    


    // Start is called before the first frame update
    void Start()
    {
        if (_directionLine == null)
            _directionLine = gameObject.GetComponentInChildren<LineRenderer>();
        health = maxHealth;
        HealthCanvas.transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        UIHealthDemonstration();
    }


    void Update()
    {
        if (gameplayIsActive)
        {
            if (Input.GetMouseButton(0))
            {
                LookController();
                LineRendererController();
            }

            if (Input.GetMouseButtonUp(0))
            {
                MovementController();
            }

            MovementAction();
            SlashController();
        }
    }

    void LookController()
    {
        doMovement = false;
        imAttacking = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 500, Color.red);
        if (Physics.Raycast(ray, out hit, colliders))
        {
            if (_rotateCoroutine != null)
            {
                StopCoroutine(_rotateCoroutine);
                _rotateCoroutine = null;
            }
            var point = hit.point;
            point.y = transform.position.y;
            navigator.transform.position = point;
            _rotateCoroutine = StartCoroutine(RotateToClick(navigator.transform.position - transform.position, rotationSpeed / 100));
        }
    }

    void LineRendererController()
    {
        Time.timeScale = timeSlowsDown;
        _directionLine.enabled = true;

        _directionLine.SetPosition(0, transform.position);
        _directionLine.SetPosition(1, navigator.transform.position);
    }

    void MovementController()
    {
        Time.timeScale = 1f;
        _directionLine.enabled = false;
        doMovement = true;
        
    }

    void MovementAction()
    {
        if (doMovement)
        {
            if (Vector3.Distance(transform.position, _directionLine.GetPosition(1)) > 0.05f)
            {
                imAttacking = true;
                float step = movementSpeed * Time.deltaTime;
                //Здесь можно добавить ускорение
                transform.position = Vector3.MoveTowards(transform.position, _directionLine.GetPosition(1), step);
            }
            else
            {
                doMovement = false;
                imAttacking = false;
                return;
            }
            
        }
    }


    public IEnumerator RotateToClick(Vector3 newLocalTarget, float time)
    {
        float elapsedTime = 0.0f;
        Quaternion startingRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.LookRotation(newLocalTarget);
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime; // <- move elapsedTime increment here
            // Rotations
            transform.rotation = Quaternion.Slerp(startingRotation, targetRotation, (elapsedTime / time));
            yield return new WaitForEndOfFrame();
        }
        yield return 0;
    }

    public void UIHealthDemonstration()
    {
        playerHealthImage.fillAmount = health / maxHealth;
    }

    public void SlashController()
    {
        if (imHitEnemy)
        {
            if (slashPrefab != null)
            {
                GameObject slash = Instantiate(slashPrefab);
                slash.transform.rotation = transform.rotation;
                Destroy(slash, 3);
            }
        }
        imHitEnemy = false;
    }
}
