using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour {

	public Director director;
	// number of sheep counted
	public int score;

	public int[] scoreToAdvanceLevel;
	int currentLevel = 0;
	//int numberOfLevels = 5;


	void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
	}

	public void increaseScore(int points)
	{
		score += points;
		GetComponentInChildren<TextMesh> ().text = "Sheep counted: "+score.ToString();
		if (score >= scoreToAdvanceLevel [currentLevel]) {
			// reset score
			score = 0;

			advanceLevel ();
		}
	}

	public void advanceLevel()
	{
		director.completedLevel (currentLevel);
		currentLevel++;
		if (currentLevel == scoreToAdvanceLevel.Length) {
			// overflow
			currentLevel--;
		}
	}

}
