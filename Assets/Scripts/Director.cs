using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour {

	// This makes things happen
	// Like when the creator says something, this handles spawning it in the world


	public Sheep[] sheep;
	public Fence[] fence;
	public TextMesh count;
	public PlayerController player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void textComponentDone(int index)
	{
		switch (index) {
		case 0:

			break;
		case 1:
			activateAllSheep ();
			break;
		case 2:

			//player.gameObject.SetActive (true);
			break;

		case 3:
			activateAllFence ();
			break;

		case 4:
			count.gameObject.SetActive (true);
			break;

		case 5:
			player.gameObject.SetActive (true);
			break;




		}
	}

	public void completedLevel(int index)
	{
		switch (index) {
		case 0:
			Debug.Log ("Beat level 1");
			player.canMove = false;
			for (int i = 0; i < sheep.Length; i++) {
				sheep [i].gameObject.SetActive (false);
			}
			for (int i = 0; i < sheep.Length; i++) {
				sheep [i].moveToPos (i + 1, i + 2);
			}
			Invoke ("activateAllSheep", 1);
			Invoke ("activatePlayer", 1);
			break;
		case 1:


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

		}
	}

	void activateAllFence()
	{
		for (int i = 0; i < fence.Length; i++) {
			fence [i].gameObject.SetActive( true);

		}
	}
}
