using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour {

	public int xStartGrid = 1;
	public int zStartGrid = 2;

	public int xGridPos;
	public int zGridPos;

	public Grid grid;

	public float moveSpeed;
	// position to move towards
	protected Vector3 movePos;
	protected Vector3 tempMovePos;

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

	public virtual void move(int xDelta, int zDelta)
	{
		if (grid.gridToVec3 (xGridPos + xDelta, zGridPos + zDelta, out tempMovePos)) {
			// position is in bounds

			if (!grid.moveOnGrid (xGridPos, zGridPos, xDelta, zDelta)) {
				//Debug.Log("Can't move on grid: "+xGridPos+", "+zGridPos+" + " + xDelta+", "+zDelta);
				// something in the way
				return;
			}
			movePos = tempMovePos;
			xGridPos += xDelta;
			zGridPos += zDelta;
		}
	}
}
