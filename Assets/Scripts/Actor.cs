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

	bool fadingOutro = false;
	float maxHorSpread = 7f;
	int horDir = 1;
	float maxVertDrift = 10f;
	float outroPercent = 0f;
	float outroRate = 0.001f;
	float moveSpeed = 0.2f;
	Vector3 goalPos;
	int numTurns = 5;
	int currentTurn = 1;
	TextMesh textMesh;

	// Use this for initialization
	void Start () {
		originalScale = gameObject.transform.localScale;
		currentTurn = 1;
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
		if (fadingOutro && !movingIntro) {
			// don't try to do the fading outro until the movingIntro is over

			// move speed gets slower as you approach the corners
			// moveSpeed is the base speed of the text moving
			// (numTurns - currentTurn)/numTurns makes the speed slow down the higher the number of turns
			// (1.0f - Mathf.Abs (transform.position.x - startingPos.x)/maxHorSpread) makes the text speed up in the middle
			// it slows down as it approaches the maxHorSpread
			// abs(trans.x - start.x) is from 0 to maxHorSpread
			// dividing by maxHor spread makes it from 0 to 1
			// subtracting from 1.0f swaps it to be from 1 to 0
			// this gives a greater weight to when trans.x and start.x are close (the center)
			// and a smaller weight near the corners
			// plus a small amout so movespeed isn't 0
			// i.e when numTurns == currentTurn
			float moveWeight = ((float)(numTurns - currentTurn)/numTurns * (1.0f - Mathf.Abs (transform.position.x - startingPos.x)/(float)maxHorSpread))+0.5f;
			//Debug.Log (moveWeight);
			transform.position = Vector3.MoveTowards (transform.position, goalPos, moveSpeed * moveWeight);


			// make it shift left and right
			// when it gets to one side, it switches to the other side
			// left hand is the distance displaced from the start
			// when going right, trans.x should be bigger than start.x
			// horDir is 1 for going right
			// so this number should approach maxHorSpread which is how far left and right it goes
			// when going left, trans.x will be smaller than start.x
			// the subtraction will be negative
			// horDir is -1 for going left
			// this makes the number positive so it can be compared to the right side
			// right side is how far the max is for this turn
			// as turns goes up, this number gets smaller
			if((transform.position.x - startingPos.x)*horDir > (maxHorSpread* (numTurns+1 - currentTurn) / numTurns) - 0.1f) {
				if (textMesh != null) {
					// fade the a channel out
					// 1 / numTurns at a time
					textMesh.color += new Color(0, 0, 0, -1.0f / (float)numTurns);
					Debug.Log (textMesh.color.a);
				}

				horDir *= -1;
				currentTurn++;
				goalPos = startingPos + new Vector3 (horDir * maxHorSpread* (numTurns+1 - currentTurn) / numTurns, maxVertDrift * currentTurn / numTurns, 0);

			}

			if (currentTurn > numTurns) {
				// end when it gets to the proper height
				fadingOutro = false;
			}
				



			//transform.position = Vector3.Lerp (transform.position, startingPos + new Vector3((float)maxHorSpread/currentTurn * horDir, (float)currentTurn/numTurns*maxVertDrift, 0), outroPercent);
			//transform.position += new Vector3((float)maxHorSpread/currentTurn * horDir * (1 - outroPercent), (float)currentTurn/numTurns*maxVertDrift * (1-outroPercent), 0);

			//

			/*
			outroPercent += outroRate;

			if (outroPercent > 1.0f) {
				outroPercent = 0;
				currentTurn++;
				horDir *= -1;
			}*/
			/*
			*/
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

	public void startFadingOutro()
	{
		// words appear, then drift upwards and fade out
		startingPos = transform.position;
		fadingOutro = true;
		//goalPos = transform.position + new Vector3 (0, maxVertDrift, 0);
		goalPos = startingPos + new Vector3 (horDir * maxHorSpread* (numTurns+1 - currentTurn) / numTurns, maxVertDrift * currentTurn / numTurns, 0);
		//Debug.Log (startingPos);
		//Debug.Log (goalPos);
		textMesh = GetComponent<TextMesh> ();

	}
}
