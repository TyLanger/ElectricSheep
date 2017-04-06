using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : GridObject {


	public ScoreController scoreController;

	
	// Update is called once per frame
	void Update () {
		

		transform.position = Vector3.MoveTowards (transform.position, movePos, moveSpeed);
	}

	public override void move(int xDelta, int zDelta)
	{
		if (grid.gridToVec3 (xGridPos + xDelta, zGridPos + zDelta, out tempMovePos)) {
			// position is in bounds

			if (!grid.moveOnGrid (xGridPos, zGridPos, xDelta, zDelta, true, this)) {
				//Debug.Log("Can't move on grid: "+xGridPos+", "+zGridPos+" + " + xDelta+", "+zDelta);
				// something in the way
				return;
			}
			movePos = tempMovePos;
			xGridPos += xDelta;
			zGridPos += zDelta;
			checkForGrass ();
		}


	}

	public void hopFence(int xPos, int zPos)
	{
		// positon sheep will be after fence jump
		if (grid.gridToVec3 (xPos, zPos, out tempMovePos)) {
			movePos = tempMovePos;
			scoreController.increaseScore (1);
			//TODO play a jump animation to make it look like you're not just passing right through the fence
		}
	}

	void checkForGrass()
	{
		// check one space to the right, left, above, and below for grass
		// if you find it, move to that spot and eat the grass
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
		}

	}

}
