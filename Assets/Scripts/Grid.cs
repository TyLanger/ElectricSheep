using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

	[SerializeField]
	public Transform[,] grid;

	int xGridSize = 5;
	int zGridSize = 5;
	public float gridSpacing;

	void Awake() {
		grid = new Transform[xGridSize, zGridSize];
	}

	void OnDrawGizmosSelected()
	{
		Vector3 pos;
		for (int x = 0; x < xGridSize; x++) {
			for (int z = 0; z < zGridSize; z++) {
				gridToVec3 (x, z, out pos);
				Gizmos.DrawSphere (pos, 0.1f);
			}
		}

	}

	/*
	 * Hard to make these check for out of bounds and return the right thing.
	 * Using out in later functions seems better
	 * 
	public Vector3 gridToVec3(int xGrid, int zGrid)
	{
		if ((xGrid >= xGridSize || xGrid < 0) || (zGrid >= zGridSize || zGrid < 0)) {
			// out of bounds
			// return something so they know they are out of bounds
			return Vector3.one;
		}
			
		return new Vector3 (xGrid * gridSpacing, 0, zGrid * gridSpacing) + transform.position;
	}

	public Transform getTransformAtGrid(int xGrid, int zGrid)
	{
		if ((xGrid >= xGridSize || xGrid < 0) || (zGrid >= zGridSize || zGrid < 0)) {
			return null;
		}
		return grid [xGrid, zGrid];
	}
	*/

	public bool gridToVec3(int xGrid, int zGrid, out Vector3 outVector)
	{
		if ((xGrid >= xGridSize || xGrid < 0) || (zGrid >= zGridSize || zGrid < 0)) {
			outVector = Vector3.one;
			return false;
		}
		outVector = new Vector3 (xGrid * gridSpacing, 0, zGrid * gridSpacing) + transform.position;
		return true;
	}

	public bool getTransformAtGrid(int xGrid, int zGrid, out Transform outTrans)
	{
		if ((xGrid >= xGridSize || xGrid < 0) || (zGrid >= zGridSize || zGrid < 0)) {
			outTrans = null;
			return false;
		}
		// even if it's not out of bounds, it could still return null
		outTrans = grid [xGrid, zGrid];
		return true;
	}

	public bool canMoveToGrid(int xGrid, int zGrid)
	{
		/* getTransformAtGrid checks if out of bound already
		if ((xGrid >= xGridSize || xGrid < 0) || (zGrid >= zGridSize || zGrid < 0)) {
			// check out of bounds
			return false;
		}
		*/

		Transform trans;
		if (getTransformAtGrid (xGrid, zGrid, out trans)) {
			// if it's empty, then you can move there
			if (trans == null) {
				return true;
			}
		}
		return false;
	}

	public bool moveOnGrid(int xCurrent, int zCurrent, int xChange, int zChange)
	{
		if (outOfBounds (xCurrent, zCurrent)) {
			// current is out of bounds
			Debug.Log("Out of bounds: "+xCurrent+", "+zCurrent+" + "+xChange+", "+zChange);
			return false;
		}
		if(outOfBounds(xCurrent + xChange, zCurrent + zChange))
		{
			// place moving to is out of bounds
			Debug.Log("Moving to out of bounds: "+xCurrent+", "+zCurrent+" + "+xChange+", "+zChange);
			return false;
		}
		grid [xCurrent + xChange, zCurrent + zChange] = grid [xCurrent, zCurrent];

		grid [xCurrent, zCurrent] = null;
		return true;
	}

	public bool addToGrid(int xPos, int zPos, Transform addTrans)
	{
		if (outOfBounds(xPos, zPos)) {
			return false;
		}
		grid [xPos, zPos] = addTrans;
		return true;
	}

	/// <summary>
	/// Returns true if out of bounds
	/// </summary>
	/// <returns><c>true</c>, if of bounds was outed, <c>false</c> otherwise.</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="z">The z coordinate.</param>
	bool outOfBounds(int x, int z)
	{
		return ((x < 0 || x >= xGridSize) || (z < 0 || z >= zGridSize));
	}

	public void debugGrid()
	{
		for (int x = 0; x < xGridSize; x++) {
			for (int z = 0; z < zGridSize; z++) {
				Debug.Log (x + ", " + z + ": " + grid [x, z]);
			}
		}
	}
}
