﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour {

	int xStartGrid = 1;
	int zStartGrid = 2;

	int xGridPos;
	int zGridPos;

	public Grid grid;

	public float moveSpeed;
	// position to move towards
	Vector3 movePos;
	Vector3 tempMovePos;

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


		transform.position = Vector3.MoveTowards (transform.position, movePos, moveSpeed);
	}

	public void move(int xDelta, int zDelta)
	{
		if (grid.gridToVec3 (xGridPos + xDelta, zGridPos + zDelta, out tempMovePos)) {
			// success
			movePos = tempMovePos;
			grid.moveOnGrid (xGridPos, zGridPos, xDelta, zDelta);
			xGridPos += xDelta;
			zGridPos += zDelta;
		}
	}

}
