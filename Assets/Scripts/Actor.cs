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

	bool movingIntro = false;
	Vector3 startingPos;
	float introPercent = 0f;
	float introRate = 0.005f;
	float maxHeight = 5f;
	bool movingUp = true;

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
		if (movingIntro) {

			// lerp between the current position and the goal
			transform.position = Vector3.Lerp (transform.position, startingPos, introPercent);
			// this causes the letters to rise as they travel left towards the target
			// it gives a decent approximation of the curve I wanted without using an animation curve
			if (transform.position.y < maxHeight && movingUp) {
				// moves the postiion up to a max height
				// numbers are just what look nice
				transform.position += new Vector3 (0, 0.26f, 0);
			} else {
				// once it gets to the height, turn the rising off
				// and just let it lerp to position
				movingUp = false;
			}

			introPercent += introRate;
			if (introPercent > 1.0f) {
				movingIntro = false;
			}
		}
	}

	public void appear()
	{
		gameObject.SetActive (true);
		appearing = true;
	}

	public void startMovingIntro(Vector3 currentPos)
	{
		startingPos = transform.position;
		// these are numbers that look right
		// this is where the letter will spawn in.
		transform.localPosition = transform.localPosition + new Vector3 (3.4f, 3.7f, 0);
		movingIntro = true;
	}
}
