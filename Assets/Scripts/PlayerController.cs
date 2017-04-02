using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	int xStartGrid = 0;
	int zStartGrid = 0;
	Vector3 startPos;

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
		if(grid.gridToVec3(xStartGrid, zStartGrid, out startPos))
		{
			transform.position = startPos;
		}
	}
	
	// Update is called once per frame
	void Update () {

		// not a huge fan of hard-coding the buttons like this, but this way you only move one space per button press
		// with the hor and vert axis, you move multiple at a time and it's hard to control how many
		if(Input.GetKeyDown("d"))
		{
			// right
			if (grid.canMoveToGrid (xGridPos + 1, zGridPos)) {
				// if nothing occupying that space
				// move right
				// if this isn't out of bounds, it will set movePos to the vec3 position of that grid space
				if (grid.gridToVec3 (xGridPos + 1, zGridPos, out tempMovePos)) {
					// success
					xGridPos += 1;
					movePos = tempMovePos;
				}
			}
		}
		else if(Input.GetKeyDown("a"))
		{
			// left
			if (grid.canMoveToGrid (xGridPos - 1, zGridPos)) {
				// if nothing occupying that space
				// move left
				if (grid.gridToVec3 (xGridPos - 1, zGridPos, out tempMovePos)) {
					xGridPos -= 1;
					movePos = tempMovePos;
				}
			}
		}
		if(Input.GetKeyDown("w"))
		{
			// up
			if (grid.canMoveToGrid (xGridPos, zGridPos + 1)) {
				// if nothing occupying that space
				// move left
				if (grid.gridToVec3 (xGridPos, zGridPos + 1, out tempMovePos)) {
					zGridPos += 1;
					movePos = tempMovePos;
				}
			}
		}
		else if(Input.GetKeyDown("s"))
		{
			// down
			if (grid.canMoveToGrid (xGridPos, zGridPos - 1)) {
				// if nothing occupying that space
				// move left
				if (grid.gridToVec3 (xGridPos, zGridPos - 1, out tempMovePos)) {
					zGridPos -= 1;
					movePos = tempMovePos;
				}
			}
		}

		transform.position = Vector3.MoveTowards (transform.position, movePos, moveSpeed);
	}
}
