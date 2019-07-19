using System;
using DG.Tweening;
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour//, IController
{
    [SerializeField] private LineRenderer _directionLine;

    [Header("Character Rotation Speed")]
    public float rotationSpeed = 1;
    public GameObject navigator;

    public LayerMask colliders;
    Coroutine _rotateCoroutine;

    private void Awake ()
	{
		
	}

    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            navigator.transform.position = GetMouseWorldPos();
            

            Time.timeScale = 0.5f;
            _directionLine.enabled = true;
            
                _directionLine.SetPosition(0, transform.position);
                _directionLine.SetPosition(1, navigator.transform.position);

               Quaternion rot = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(navigator.transform.position - transform.position), rotationSpeed * Time.deltaTime);
               rot.x = transform.rotation.x;
               rot.y = transform.rotation.y;

               transform.rotation = rot;




            

          /*  if (_rotateCoroutine != null)
            {
                StopCoroutine(_rotateCoroutine);
                _rotateCoroutine = null;
            }
            var point = GetMouseWorldPos();
            point.y = transform.position.y;
            _rotateCoroutine = StartCoroutine(RotateToClick(point - transform.position, rotationSpeed/100));
            */

        }

        if (Input.GetMouseButtonUp(0))
            {
            Time.timeScale = 1f;
            _directionLine.enabled = false;

            transform.DOMove(_directionLine.GetPosition(1), 0.5f);
        }
    }

    /*  public void OnMouseDown()
      {
          Time.timeScale = 0.5f;
          _directionLine.enabled = true;
          _directionLine.SetPosition(0, transform.position);

          Debug.Log("OnMouseDown");
      }

      public void OnMouseDrag()
      {
          _directionLine.SetPosition(1, GetMouseWorldPos());
      }

      public void OnMouseUp()
      {
          Time.timeScale = 1f;
          _directionLine.enabled = false;

          transform.DOMove(_directionLine.GetPosition(1), 0.5f);
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
     */

       

    private Vector3 GetMouseWorldPos()
    {/*
        Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z);
        Vector3 result = Camera.main.ScreenToWorldPoint(pos);
        result.z = 0.5f;

       

       */


        Vector3 result = new Vector3();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100,colliders))
        {
            result = hit.collider.transform.position;
            
        }
        result.z = 0.5f;
       
        return result;

    }

    public Type ControllerType => typeof(Player);

    public void Initialize()
    {
		
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
