using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour {

	// number of sheep counted
	public int score;

	public int[] scoreToAdvanceLevel;
	int currentLevel = 0;
	int numberOfLevels = 5;

	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	public void increaseScore(int points)
	{
		score += points;
		if (score >= scoreToAdvanceLevel [currentLevel]) {
			// reset score
			score = 0;
			advanceLevel ();
		}
	}

	public void advanceLevel()
	{
		currentLevel++;
	}

}
