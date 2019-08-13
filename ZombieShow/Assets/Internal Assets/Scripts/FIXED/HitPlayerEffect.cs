using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitPlayerEffect : MonoBehaviour
{
    public bool playerWasHit;
    [Header("Camera Position Control")]
    public float timerOfShake = 0.5f;
    public float timerBetweenShakes = 0.05f;
    public Image redScreenCamera;
    public float shakeDifference = .2f;
    Vector3 cameraTransform;
    Camera cam;

   public bool startShake;
    float ts;
    float tbs;

   public byte controlOfHit;
   public bool startColorChange;
    byte colorStep = 0;

    void Start()
    {
        if (Camera.main != null)
        {
            cam = Camera.main;
            cameraTransform = cam.transform.position;
        }
    }

    void Update()
    {
        if (controlOfHit == 1)
        {
            RedScreenAction();
            ShakeAction();

            if (!startColorChange && !startShake)
                controlOfHit = 0;
        }
    }

    public void PlayerWasHit()
    {
        playerWasHit = true;
        if (playerWasHit && controlOfHit == 0)
        {
            startColorChange = true;
            startShake = true;
            controlOfHit = 1;
        }
    }

    void RedScreenAction()
    {
        if (startColorChange)
        switch (colorStep)
        {
            case 0:
                if (redScreenCamera.color.a < .5f)
                {
                    redScreenCamera.color += new Color(0, 0, 0, 0.05f);
                }
                else
                    colorStep++;
                break;
            case 1:
                if (redScreenCamera.color.a > 0)
                {
                    redScreenCamera.color -= new Color(0, 0, 0, 0.05f);
                }
                else
                    colorStep++;
                break;
            case 2:
                    colorStep = 0;
                    startColorChange = false;
                    return;
               
        }
    }


    void ShakeAction()
    {
        if (startShake)
        {
            ts -= Time.deltaTime;
            tbs -= Time.deltaTime;
            if (ts > 0)
            {
                if (tbs <= 0)
                {
                    cam.transform.position =
                        new Vector3(Random.Range(cameraTransform.x + shakeDifference, cameraTransform.x - shakeDifference),
                        cameraTransform.y,
                        Random.Range(cameraTransform.z + shakeDifference, cameraTransform.z - shakeDifference));
                    tbs = timerBetweenShakes;
                }
            }
            else
            {
                tbs = timerBetweenShakes;
                ts = timerOfShake;
                cam.transform.position = cameraTransform;
                startShake = false;
            }
           
            
        }
    }

    
}
