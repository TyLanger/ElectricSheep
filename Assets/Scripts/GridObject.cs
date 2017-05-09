using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GridType{Sheep, Grass, Player, Fence};

public class GridObject : MonoBehaviour {

	public int xStartGrid = 1;
	public int zStartGrid = 2;

	public int xGridPos;
	public int zGridPos;

	public Grid grid;
	public GridType gridType;

	public float moveSpeed;
	// position to move towards
	protected Vector3 movePos;
	protected Vector3 tempMovePos;

	// Use this for initialization
	protected virtual void Start () {
		xGridPos = xStartGrid;
		zGridPos = zStartGrid;

		if(grid.gridToVec3(xStartGrid, zStartGrid, out movePos))
		{
			transform.position = movePos;
			grid.addToGrid (xGridPos, zGridPos, this);
		}
	}

	public virtual void moveToPos(int xPos, int zPos)
	{
		if (grid.gridToVec3 (xPos, zPos, out movePos)) {
			transform.position = movePos;
			grid.moveOnGrid (xGridPos, zGridPos, xPos - xGridPos, zPos - zGridPos, this);
			xGridPos = xPos;
			zGridPos = zPos;
		}
	}

	public virtual void move(int xDelta, int zDelta)
	{
		Debug.Log ("Base move");
		if (grid.gridToVec3 (xGridPos + xDelta, zGridPos + zDelta, out tempMovePos)) {
			// position is in bounds

			if (!grid.moveOnGrid (xGridPos, zGridPos, xDelta, zDelta, this)) {
				Debug.Log("Base: Can't move on grid: "+xGridPos+", "+zGridPos+" + " + xDelta+", "+zDelta);
				// something in the way
				return;
			}
			movePos = tempMovePos;
			xGridPos += xDelta;
			zGridPos += zDelta;
		}
	}

	public virtual void hide()
	{
		// base hide just sets its gameobject to not active
		// other vversions could change some functionality
		// like the player no longer takes input so the player can't move
		gameObject.SetActive(false);
	}

	public virtual void unHide()
	{
		gameObject.SetActive (true);
	}
}
