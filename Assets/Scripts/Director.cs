using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour {

	// This makes things happen
	// Like when the creator says something, this handles spawning it in the world



	[System.Serializable]
	public struct Level {
		public LevelLayout[] layout;
		public int goal;
	}

	[System.Serializable]
	public struct LevelLayout {
		public GridObject gridObject;
		public Vector2 gridCoords;
	}

	public Level[] levels;

	public Sheep[] sheep;
	public Fence[] fence;
	public Grass grass;
	public GameObject bed;
	public TextMesh count;
	public PlayerController player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void setupLevel(int levelNumber)
	{
		for (int i = 0; i < levels[levelNumber].layout.Length; i++) {
			// move each gridObject to its location
			levels [levelNumber].layout [i].gridObject.moveToPos ((int)levels [levelNumber].layout [i].gridCoords.x, (int)levels [levelNumber].layout [i].gridCoords.y);
			// hide the object until the level starts
			levels [levelNumber].layout [i].gridObject.hide();
		}

	}

	public void textComponentDone(int index)
	{
		switch (index) {
		case 0:

			break;
		case 1:
			bed.SetActive (true);
			break;
		case 2:
			activateAllSheep ();
			break;
		case 3:

			//player.gameObject.SetActive (true);
			break;

		case 4:
			activateAllFence ();
			break;

		case 5:
			count.gameObject.SetActive (true);
			break;

		case 7:
			//player.gameObject.SetActive (true);
			player.unHide ();
			break;

		case 15:
			returnPlayerControl ();
			break;


		}
	}

	public void completedLevel(int index)
	{
		switch (index) {
		case 0:
			player.canMove = false;
			for (int i = 0; i < sheep.Length; i++) {
				sheep [i].gameObject.SetActive (false);

			}
			for (int i = 0; i < sheep.Length; i++) {
				sheep [i].moveToPos (i + 3, i + 2);
			}
			Invoke ("activateAllSheep", 1);
			Invoke ("returnPlayerControl", 1);
			break;
		case 1:
			player.canMove = false;
			grass.gameObject.SetActive (true);
			// eat the grass so it starts out by growing
			grass.eatGrass ();
			for (int i = 0; i < sheep.Length; i++) {
				sheep [i].moveToPos (i + 2, i + 1);
			}
			Invoke ("activateAllSheep", 1);
			Invoke ("returnPlayerControl", 1);
			break;

		}
	}

	void returnPlayerControl()
	{
		player.canMove = true;
	}
	void activateAllSheep()
	{
		for (int i = 0; i < sheep.Length; i++) {
			sheep [i].gameObject.SetActive( true);
			sheep [i].GetComponent<Actor> ().appear ();
		}
	}

	void activateAllFence()
	{
		for (int i = 0; i < fence.Length; i++) {
			fence [i].gameObject.SetActive( true);
			fence [i].GetComponent<Actor> ().appear ();
		}
	}
}
