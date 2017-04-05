using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour {

	public int xStartGrid = 1;
	public int zStartGrid = 2;

	public int xGridPos;
	public int zGridPos;

	public Grid grid;
	public ScoreController scoreController;

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
		Debug.Log ("Checking for grass");
		Debug.Log ("Current pos: " + xGridPos + ", " + zGridPos);
		Transform outTrans;
		if (grid.getTransformAtGrid (xGridPos + 1, zGridPos, out outTrans)) {
			if (outTrans != null) {
				Debug.Log (outTrans);
				if (outTrans.GetComponent<Grass> () != null) {
					// grass to the right
					Debug.Log("Found it right");
					move (1, 0);
					outTrans.GetComponent<Grass> ().eatGrass ();
				}
			}
		} if (grid.getTransformAtGrid (xGridPos - 1, zGridPos, out outTrans)) {
			if (outTrans != null) {
				Debug.Log (outTrans);
				if (outTrans.GetComponent<Grass> () != null) {
					// grass to the left
					Debug.Log("Found it left");
					move (-1, 0);
					outTrans.GetComponent<Grass> ().eatGrass ();
				}
			}
		} if (grid.getTransformAtGrid (xGridPos, zGridPos + 1, out outTrans)) {
			if (outTrans != null) {
				Debug.Log (outTrans);
				if (outTrans.GetComponent<Grass> () != null) {
					// grass above
					Debug.Log("Found it above");
					move (0, 1);
					outTrans.GetComponent<Grass> ().eatGrass ();
				}
			}
		} if (grid.getTransformAtGrid (xGridPos, zGridPos - 1, out outTrans)) {
			if (outTrans != null) {
				Debug.Log (outTrans);
				if (outTrans.GetComponent<Grass> () != null) {
					// grass below
					Debug.Log("Found it below");
					move (0, -1);
					outTrans.GetComponent<Grass> ().eatGrass ();
				}
			}
		}

	}

}
