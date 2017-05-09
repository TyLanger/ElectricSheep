using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : GridObject {


	public ScoreController scoreController;

	//public Grass grass;
	public GridObject foundGrass;
	public GridObject foundFence;

	// this sheep has hopped the fence
	public bool finished = false;

	protected override void Start()
	{
		base.Start ();
		gridType = GridType.Sheep;
	}
	
	// Update is called once per frame
	void Update () {
		

		transform.position = Vector3.MoveTowards (transform.position, movePos, moveSpeed);
	}

	public override void moveToPos(int xPos, int zPos)
	{
		base.moveToPos (xPos, zPos);
		// to move the sheep back into the game
		finished = false;
	}

	public override void move(int xDelta, int zDelta)
	{
		
		if (grid.gridToVec3 (xGridPos + xDelta, zGridPos + zDelta, out tempMovePos)) {
			// position is in bounds

			if (!grid.moveOnGrid (xGridPos, zGridPos, xDelta, zDelta, true, this)) {
				Debug.Log ("Can't move on grid: " + xGridPos + ", " + zGridPos + " + " + xDelta + ", " + zDelta);
				// something in the way
				return;
			}

			movePos = tempMovePos;
			xGridPos += xDelta;
			zGridPos += zDelta;
			if (checkForFence ()) {
				//Debug.Log ("Found broken fence");
				hopFence (xGridPos, zGridPos+1);
			}
			checkForGrass ();
		}
	}
		
	public void hopFence(int xPos, int zPos)
	{
		// hop over the fence by moving 1 up
		move (0, 1);
		finished = true;
		// increment your score
		// give some time so the sheep can reach its destination
		Invoke("increaseScore", 1);
	}

	void increaseScore()
	{
		scoreController.increaseScore (1);
	}

	bool checkForFence()
	{
		// check for fence on your current spot
		if(grid.findAtGrid(xGridPos, zGridPos, GridType.Fence, ref foundFence))
		{
			if (foundFence != null) {
				if (foundFence.GetComponent<Fence> () != null) {
					return foundFence.GetComponent<Fence> ().broken;
				}
			}
		}
		return false;
	}

	void checkForGrass()
	{
		// check one space to the right, left, above, and below for grass
		// if you find it, move to that spot and eat the grass
		//Debug.Log("Checking");

		if (grid.findAtGrid (xGridPos + 1, zGridPos, GridType.Grass, ref foundGrass)) {
			// found grass right
			if (foundGrass.GetComponent<Grass> ().eaten) {
				// if the grass is currently eaten, dont eat it again
				return;
			}
			move(1,0);
			foundGrass.GetComponent<Grass>().eatGrass();
		}
		if (grid.findAtGrid (xGridPos - 1, zGridPos, GridType.Grass, ref foundGrass)) {
			// found grass left
			if (foundGrass.GetComponent<Grass> ().eaten) {
				// if the grass is currently eaten, dont eat it again
				return;
			}
			move(-1, 0);
			foundGrass.GetComponent<Grass>().eatGrass();
		}
		if (grid.findAtGrid (xGridPos, zGridPos + 1, GridType.Grass, ref foundGrass)) {
			// found grass up
			if (foundGrass.GetComponent<Grass> ().eaten) {
				// if the grass is currently eaten, dont eat it again
				return;
			}
			move(0, 1);
			foundGrass.GetComponent<Grass>().eatGrass();
		}
		if (grid.findAtGrid (xGridPos, zGridPos - 1, GridType.Grass, ref foundGrass)) {
			// found grass down
			if (foundGrass.GetComponent<Grass> ().eaten) {
				// if the grass is currently eaten, dont eat it again
				return;
			}
			move(0, -1);
			//Debug.Log (foundGrass);
			foundGrass.GetComponent<Grass>().eatGrass();
		}

		/*
		Transform outTrans;
		if (grid.getTransformAtGrid (xGridPos + 1, zGridPos, out outTrans)) {
			if (outTrans != null) {
				if (outTrans.GetComponent<Grass> () != null) {
					// grass to the right
					move (1, 0);
					outTrans.GetComponent<Grass> ().eatGrass ();
				}
			}
		} if (grid.getTransformAtGrid (xGridPos - 1, zGridPos, out outTrans)) {
			if (outTrans != null) {
				if (outTrans.GetComponent<Grass> () != null) {
					// grass to the left
					move (-1, 0);
					outTrans.GetComponent<Grass> ().eatGrass ();
				}
			}
		} if (grid.getTransformAtGrid (xGridPos, zGridPos + 1, out outTrans)) {
			if (outTrans != null) {
				if (outTrans.GetComponent<Grass> () != null) {
					// grass above
					move (0, 1);
					outTrans.GetComponent<Grass> ().eatGrass ();
				}
			}
		} if (grid.getTransformAtGrid (xGridPos, zGridPos - 1, out outTrans)) {
			if (outTrans != null) {
				if (outTrans.GetComponent<Grass> () != null) {
					// grass below
					move (0, -1);
					outTrans.GetComponent<Grass> ().eatGrass ();
				}
			}
		}*/

	}

}
