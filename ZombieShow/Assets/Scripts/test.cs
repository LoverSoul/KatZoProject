using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    Coroutine _rotateCoroutine;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (_rotateCoroutine != null)
                {
                    StopCoroutine(_rotateCoroutine);
                    _rotateCoroutine = null;
                }
                var point = hit.point;
                point.y = transform.position.y;
                _rotateCoroutine = StartCoroutine(RotateToClick(point - transform.position, 1f));
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
}
