using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Facing{up, down, left, right};

public class PlayerController : MonoBehaviour {

	int xStartGrid = 0;
	int zStartGrid = 0;
	Vector3 startPos;

	Facing facing;

	int xGridPos;
	int zGridPos;

	public float moveSpeed;
	public Grid grid;

	// this is the position the player is currently moving towards
	Vector3 movePos;
	// used as temporary storage before the vector is verified
	// If not out of bound, this will get set to movePos and the player can move.
	Vector3 tempMovePos;


	// Use this for initialization
	void Start () {
		facing = Facing.right;

		xGridPos = xStartGrid;
		zGridPos = zStartGrid;
		if(grid.gridToVec3(xStartGrid, zStartGrid, out startPos))
		{
			transform.position = startPos;
			grid.addToGrid (xGridPos, zGridPos, transform);
		}
	}
	
	// Update is called once per frame
	void Update () {

		// not a huge fan of hard-coding the buttons like this, but this way you only move one space per button press
		// with the hor and vert axis, you move multiple at a time and it's hard to control how many
		if(Input.GetKeyDown("d"))
		{
			// right
			if (facing != Facing.right) {
				facing = Facing.right;
			} else {
				if (grid.canMoveToGrid (xGridPos + 1, zGridPos)) {
					// if nothing occupying that space
					// move right
					// if this isn't out of bounds, it will set movePos to the vec3 position of that grid space
					if (grid.gridToVec3 (xGridPos + 1, zGridPos, out tempMovePos)) {
						// success
						xGridPos += 1;
						movePos = tempMovePos;
						// -1 in first place because already updated gridPos
						grid.moveOnGrid (xGridPos - 1, zGridPos, xGridPos, zGridPos);
					}
				}
			}
		}
		else if(Input.GetKeyDown("a"))
		{
			// left
			if (facing != Facing.left) {
				facing = Facing.left;
			} else {
				if (grid.canMoveToGrid (xGridPos - 1, zGridPos)) {
					// if nothing occupying that space
					// move left
					if (grid.gridToVec3 (xGridPos - 1, zGridPos, out tempMovePos)) {
						xGridPos -= 1;
						movePos = tempMovePos;
						// -1 in first place because already updated gridPos
						grid.moveOnGrid (xGridPos + 1, zGridPos, xGridPos, zGridPos);
					}
				}
			}
		}
		if(Input.GetKeyDown("w"))
		{
			// up
			if (facing != Facing.up) {
				facing = Facing.up;
			} else {
				if (grid.canMoveToGrid (xGridPos, zGridPos + 1)) {
					// if nothing occupying that space
					// move left
					if (grid.gridToVec3 (xGridPos, zGridPos + 1, out tempMovePos)) {
						zGridPos += 1;
						movePos = tempMovePos;
						// -1 in first place because already updated gridPos
						grid.moveOnGrid (xGridPos, zGridPos - 1, xGridPos, zGridPos);
					}
				}
			}
		}
		else if(Input.GetKeyDown("s"))
		{
			// down
			if (facing != Facing.down) {
				facing = Facing.down;
			} else {
				if (grid.canMoveToGrid (xGridPos, zGridPos - 1)) {
					// if nothing occupying that space
					// move left
					if (grid.gridToVec3 (xGridPos, zGridPos - 1, out tempMovePos)) {
						zGridPos -= 1;
						movePos = tempMovePos;
						// -1 in first place because already updated gridPos
						grid.moveOnGrid (xGridPos, zGridPos + 1, xGridPos, zGridPos);
					}
				}
			}
		}

		if (Input.GetKeyDown ("space")) {
			// swing your crook
			Transform tempTrans;
			switch (facing) 
			{
				case Facing.right:
				{
					//right
					if (grid.getTransformAtGrid (xGridPos + 1, zGridPos, out tempTrans)) {
						// tempTrans exists
						// is it a sheep?
						Sheep sheep = tempTrans.GetComponent<Sheep>();
						if (sheep != null) {
							// move 1 on x-axis, 0 on z-axis
							sheep.move (1, 0);
						}
					}
					break;
				}
				case Facing.left:
				{
					// left
					if (grid.getTransformAtGrid (xGridPos - 1, zGridPos, out tempTrans)) {
						// tempTrans exists
						// is it a sheep?
						Sheep sheep = tempTrans.GetComponent<Sheep>();
						if (sheep != null) {
							// move 1 on x-axis, 0 on z-axis
							sheep.move (-1, 0);
						}
					}
					break;
				}
				case Facing.up:
				{
					// up
					if (grid.getTransformAtGrid (xGridPos, zGridPos + 1, out tempTrans)) {
						// tempTrans exists
						// is it a sheep?
						Sheep sheep = tempTrans.GetComponent<Sheep>();
						if (sheep != null) {
							// move 1 on x-axis, 0 on z-axis
							sheep.move (0, 1);
						}
					}
					break;
				}
				case Facing.down:
				{
					// down
					if (grid.getTransformAtGrid (xGridPos, zGridPos - 1, out tempTrans)) {
						// tempTrans exists
						// is it a sheep?
						Sheep sheep = tempTrans.GetComponent<Sheep>();
						if (sheep != null) {
							// move 1 on x-axis, 0 on z-axis
							sheep.move (0, -1);
						}
					}
					break;
				}
			}


		}

		transform.position = Vector3.MoveTowards (transform.position, movePos, moveSpeed);
	}
}
