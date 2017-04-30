using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {


	float appearTime = 1f;
	float growthTime = 0f;
	public float growthRate = 0.15f;
	bool appearing = false;
	bool growing = true;
	public float appearScale = 1.1f;
	Vector3 originalScale;

	// Use this for initialization
	void Start () {
		originalScale = gameObject.transform.localScale;	
	}
	
	// Update is called once per frame
	void Update () {
		if (appearing) {
			
			gameObject.transform.localScale = originalScale * Mathf.Lerp (1, appearScale, growthTime);

			if (growing) {
				growthTime += growthRate;
				if (growthTime > appearTime) {
					growing = false;
				}
			} else {
				growthTime -= growthRate;
				if (growthTime < 0) {
					growing = true;
					appearing = false;
				}
			}
		}
	}

	public void appear()
	{
		gameObject.SetActive (true);
		appearing = true;
	}
}
