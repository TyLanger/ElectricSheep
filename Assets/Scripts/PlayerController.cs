using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Facing{up, down, left, right};

public class PlayerController : GridObject {



	Facing facing;

	// used for finding sheep
	// can't use new keyword for them
	public Sheep sheep;
	public GridObject foundSheep;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		facing = Facing.right;

	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown ("f")) {
			grid.debugGrid ();
		}

		// not a huge fan of hard-coding the buttons like this, but this way you only move one space per button press
		// with the hor and vert axis, you move multiple at a time and it's hard to control how many
		if(Input.GetKeyDown("d"))
		{
			// right
			if (facing != Facing.right) {
				facing = Facing.right;
			}
			if (grid.canMoveToGrid (xGridPos + 1, zGridPos, false)) {
				// if nothing occupying that space
				// move right
				// if this isn't out of bounds, it will set movePos to the vec3 position of that grid space
				if (grid.gridToVec3 (xGridPos + 1, zGridPos, out tempMovePos)) {
					// success
					xGridPos += 1;
					movePos = tempMovePos;
					// -1 in first place because already updated gridPos
					grid.moveOnGrid (xGridPos - 1, zGridPos, 1, 0, this);
				}
			}
		}
		else if(Input.GetKeyDown("a"))
		{
			// left
			if (facing != Facing.left) {
				facing = Facing.left;
			}
			if (grid.canMoveToGrid (xGridPos - 1, zGridPos, false)) {
				// if nothing occupying that space
				// move left
				if (grid.gridToVec3 (xGridPos - 1, zGridPos, out tempMovePos)) {
					xGridPos -= 1;
					movePos = tempMovePos;
					// -1 in first place because already updated gridPos
					grid.moveOnGrid (xGridPos + 1, zGridPos, -1, 0, this);
				}
			}
		}
		if(Input.GetKeyDown("w"))
		{
			// up
			if (facing != Facing.up) {
				facing = Facing.up;
			}
			if (grid.canMoveToGrid (xGridPos, zGridPos + 1, false)) {
				// if nothing occupying that space
				// move up
				if (grid.gridToVec3 (xGridPos, zGridPos + 1, out tempMovePos)) {
					zGridPos += 1;
					movePos = tempMovePos;
					// -1 in first place because already updated gridPos
					grid.moveOnGrid (xGridPos, zGridPos - 1, 0, 1, this);
				}
			}
		}
		else if(Input.GetKeyDown("s"))
		{
			// down
			if (facing != Facing.down) {
				facing = Facing.down;
			} 
			if (grid.canMoveToGrid (xGridPos, zGridPos - 1, false)) {
				// if nothing occupying that space
				// move left
				if (grid.gridToVec3 (xGridPos, zGridPos - 1, out tempMovePos)) {
					zGridPos -= 1;
					movePos = tempMovePos;
					// -1 in first place because already updated gridPos
					grid.moveOnGrid (xGridPos, zGridPos + 1, 0, -1, this);
				}
			}
		}

		if (Input.GetKeyDown ("space")) {
			// swing your crook
			//Transform tempTrans;
			//Sheep sheep;
			//GridObject foundSheep;
			switch (facing) 
			{
				case Facing.right:
				{
					//right
					if (grid.findAtGrid (xGridPos + 1, zGridPos, sheep, out foundSheep)) {
						// found a sheep at that grid spot
						foundSheep.GetComponent<Sheep>().move (1, 0);
					}
					/*
					if (grid.getTransformAtGrid (xGridPos + 1, zGridPos, out tempTrans)) {
						// tempTrans exists
						// even if it's not out of bounds, it could still return null
						// is it a sheep?
						if (tempTrans != null) {
							Sheep sheep = tempTrans.GetComponent<Sheep> ();
							if (sheep != null) {
								// move 1 on x-axis, 0 on z-axis
								sheep.move (1, 0);
							}
						}
					}*/
					break;
				}
				case Facing.left:
				{
					// left
					if (grid.findAtGrid (xGridPos + 1, zGridPos, sheep, out foundSheep)) {
						// found a sheep at that grid spot
						foundSheep.GetComponent<Sheep>().move (-1, 0);
					}
					/*
					if (grid.getTransformAtGrid (xGridPos - 1, zGridPos, out tempTrans)) {
						// tempTrans exists
						// is it a sheep?
						if (tempTrans != null) {
							Sheep sheep = tempTrans.GetComponent<Sheep> ();
							if (sheep != null) {
								// move 1 on x-axis, 0 on z-axis
								sheep.move (-1, 0);
							}
						}
					}*/
					break;
				}
				case Facing.up:
				{
					// up
					if (grid.findAtGrid (xGridPos + 1, zGridPos, sheep, out foundSheep)) {
						// found a sheep at that grid spot
						foundSheep.GetComponent<Sheep>().move (0, 1);
					}
					/*
					if (grid.getTransformAtGrid (xGridPos, zGridPos + 1, out tempTrans)) {
						// tempTrans exists
						// is it a sheep?
						if (tempTrans != null) {
							Sheep sheep = tempTrans.GetComponent<Sheep> ();
							if (sheep != null) {
								// move 1 on x-axis, 0 on z-axis
								sheep.move (0, 1);
							}
						}
					}*/
					break;
				}
				case Facing.down:
				{
					// down
					if (grid.findAtGrid (xGridPos + 1, zGridPos, sheep, out foundSheep)) {
						// found a sheep at that grid spot
						foundSheep.GetComponent<Sheep>().move (0, -1);
					}
					/*
					if (grid.getTransformAtGrid (xGridPos, zGridPos - 1, out tempTrans)) {
						// tempTrans exists
						// is it a sheep?
						if (tempTrans != null) {
							Sheep sheep = tempTrans.GetComponent<Sheep> ();
							if (sheep != null) {
								// move 1 on x-axis, 0 on z-axis
								sheep.move (0, -1);
							}
						}
					}*/
					break;
				}
			}


		}

		transform.position = Vector3.MoveTowards (transform.position, movePos, moveSpeed);
	}
}
