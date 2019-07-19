using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float Speed = 2f;

    private Transform _followTarget;


	private void Awake ()
	{
	    _followTarget = GameObject.FindGameObjectWithTag(Constants.Tags.Player).transform;

	    Speed = Random.Range(2f, 4f);

	    this.UpdateAsObservable()
	        .Subscribe(x =>
	        {
                transform.Translate((_followTarget.position - transform.position).normalized * Speed * Time.smoothDeltaTime);
	        });

	    this.OnTriggerEnterAsObservable()
	        .Subscribe(x =>
	        {
	            transform.DOScale(Vector3.one * 1.5f, 0.25f);
	            GetComponent<MeshRenderer>().material.DOFade(0f, 0.25f)
	                .OnComplete(() =>
	                {
                        if (GameObject.FindGameObjectWithTag("GameController") != null)
                        {
                            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().AlreadyKilled++;
                            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().CheckOfKillingEnemy();
                        }
                        Destroy(gameObject);
	                });
	        });
	}
	
	private void Start ()
	{
		
	}
	
	private void Update ()
	{
		
	}
}
