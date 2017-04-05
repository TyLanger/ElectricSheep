using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour {


	public int xStartGrid = 3;
	public int zStartGrid = 1;

	public int xGridPos;
	public int zGridPos;

	public Grid grid;

	Vector3 movePos;

	public float regrowTime = 5;
	float timeEatenAt;
	bool eaten = false;

	float originalHeight = 1;
	float eatenHeight = 0.2f;

	// Use this for initialization
	void Start () {
		xGridPos = xStartGrid;
		zGridPos = zStartGrid;

		if(grid.gridToVec3(xStartGrid, zStartGrid, out movePos))
		{
			transform.position = movePos;
			grid.addToGrid (xGridPos, zGridPos, transform);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (eaten) {
			if (Time.time > timeEatenAt + regrowTime) {
				regrowGrass ();

			}
		}
	}

	public void eatGrass()
	{
		transform.localScale = new Vector3(eatenHeight, eatenHeight, eatenHeight);
		timeEatenAt = Time.time;
		eaten = true;
	}

	void regrowGrass()
	{
		eaten = false;
		transform.localScale = new Vector3 (1, originalHeight, 1);
		grid.addToGrid (xGridPos, zGridPos, transform);
	}
}
