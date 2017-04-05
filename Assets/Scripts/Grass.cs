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
		
	}
}
