using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GridSpace {

	public GridObject[] gridObjects;
	public int index;
	int maxObjects;

	public GridSpace(int _maxObjects)
	{
		maxObjects = _maxObjects;
		gridObjects = new GridObject[maxObjects];
		index = 0;
		gridObjects [index] = null;
	}

	public void add(GridObject g)
	{
		gridObjects [index] = g;
		index++;
	}

	public bool remove(GridObject g)
	{
		for (int i = 0; i < index; i++) {
			if (gridObjects [i] == g) {
				// found it, remove it
				// move the thing in the last position to overwrite this
				gridObjects[i] = gridObjects[index];
				index--;
				return true;
			}
		}
		return false;
	}

	public bool find(GridObject g, out int _index)
	{
		for (int i = 0; i < index; i++) {
			if (gridObjects [i] != null) {
				if (gridObjects [i].GetComponent<GridObject> () != null) {
					if (gridObjects [i].GetComponent<GridObject> () == g) {
						_index = i;
						return true;
					}
				}
			}
		}
		_index = -1;
		return false;
	}

}

public class Grid : MonoBehaviour {


	GridSpace[,] grid;

	// can't use the new keyword for Grass
	public Grass grass;

	[SerializeField]
	public GridSpace gSpace;

	int xGridSize = 6;
	int zGridSize = 7;
	public float gridSpacing;

	void Awake() {
		grid = new GridSpace[xGridSize, zGridSize];
		for (int x = 0; x < xGridSize; x++) {
			for (int z = 0; z < zGridSize; z++) {
				grid [x, z] = new GridSpace (3);
			}
		}
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

	public bool getTransformAtGrid(int xGrid, int zGrid, out Transform[] outTransforms)
	{
		GridObject[] GOs;
		if (getGridObjectsAtGrid (xGrid, zGrid, out GOs)) {
			outTransforms = new Transform[GOs.Length];
			for (int i = 0; i < GOs.Length; i++) {
				outTransforms [i] = GOs [i].transform;
			}
			return true;
		}
		outTransforms = null;
		return false;
	}

	public bool getGridObjectsAtGrid(int xGrid, int zGrid, out GridObject[] outGO)
	{
		if ((xGrid >= xGridSize || xGrid < 0) || (zGrid >= zGridSize || zGrid < 0)) {
			outGO = null;
			return false;
		}
		// even if it's not out of bounds, it could still return null
		if(grid [xGrid, zGrid].index > 0)
		{
			// something there
			GridObject[] objects = new GridObject[grid [xGrid, zGrid].index];
			for (int i = 0; i < grid[xGrid, zGrid].index; i++) {
				objects [i] = grid [xGrid, zGrid].gridObjects[i];
			}
			outGO = objects;
			return true;

		}
		// no objects there
		outGO = null;
		return false;
	}

	public bool canMoveToGrid(int xGrid, int zGrid, bool isSheep)
	{
		if (outOfBounds (xGrid, zGrid)) {
			return false;
		}
		if (grid [xGrid, zGrid].index > 0) {
			// something is there
			GridObject[] gos;
			if (getGridObjectsAtGrid (xGrid, zGrid, out gos)) {
				for (int i = 0; i < gos.Length; i++) {
					if (gos [i].GetComponent<Grass> () != null) {
						// just grass, can walk there
						return true;
					} else if (isSheep && (gos [i].GetComponent<Fence> () != null)) {
						if (gos [i].GetComponent<Fence> ().broken) {
							return true;
						}
					}
				}
			}

			return false;
		}
		// nothing is there
		return true;
	}

	public bool moveOnGrid(int xCurrent, int zCurrent, int xChange, int zChange, GridObject gridObject)
	{
		// default is non-sheep
		return moveOnGrid (xCurrent, zCurrent, xChange, zChange, false, gridObject);
	}

	public bool moveOnGrid(int xCurrent, int zCurrent, int xChange, int zChange, bool isSheep, GridObject gridObject)
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

		if (canMoveToGrid (xCurrent+xChange, zCurrent+zChange, isSheep)) {
			// at most, only grass in the way

			if (isSheep) {
				// check for broken fence
				int index;
				if (grid [xCurrent + xChange, zCurrent + zChange].find (grass, out index)) {
					// move one extra up to jump over the fence
					grid [xCurrent + xChange, zCurrent + zChange + 1].add(gridObject);
					grid [xCurrent, zCurrent].remove(gridObject);
					return true;
				}
			}

			// add this to next spot
			grid [xCurrent + xChange, zCurrent + zChange].add(gridObject);
			// remove from previous spot
			grid [xCurrent, zCurrent].remove(gridObject);

			return true;
		}
		Debug.Log ("Last return");
		return false;
		/*
		 * Old jumping over fence
		if(grid [xCurrent + xChange, zCurrent + zChange] != null)
		{
			// check if the transform is grass
			if (grid [xCurrent + xChange, zCurrent + zChange].GetComponent<Grass> () == null) {
				// if it does NOT have a grass component, you can't move onto its space
				// something in the way
				if (isSheep) {
				
					Transform tempTrans = grid [xCurrent + xChange, zCurrent + zChange];
					Debug.Log (tempTrans);
					// is the thing ran into a fence?
					if (tempTrans.GetComponent<Fence> () != null) {
						if (tempTrans.GetComponent<Fence> ().broken) {
							// sheep can jump over broken fence
							// sheep moves one more up
							grid [xCurrent + xChange, zCurrent + zChange + 1] = grid [xCurrent, zCurrent];
							grid [xCurrent, zCurrent] = null;
							// assume fence is only above the sheep
							sheep.hopFence (xCurrent + xChange, zCurrent + zChange + 1);
							return true;
						}
					}
				}
				Debug.Log (grid [xCurrent + xChange, zCurrent + zChange] + " in the way.");
				return false;
			}


			// the transform was grass. Things can move onto grass
			/*
			grid [xCurrent + xChange, zCurrent + zChange] = grid[xCurrent, zCurrent];
			grid [xCurrent, zCurrent] = null;
			return true;

		}
	*/

	}

	public bool addToGrid(int xPos, int zPos, GridObject gridObject)
	{
		if (outOfBounds(xPos, zPos)) {
			return false;
		}
		grid [xPos, zPos].add(gridObject);
		return true;
	}

	public bool findAtGrid(int xPos, int zPos, GridObject toFind, GridObject found)
	{
		int index;
		if (grid [xPos, zPos].find (toFind, out index)) {
			// found it
			found = grid[xPos, zPos].gridObjects[index];
			Debug.Log(index + " found: "+grid[xPos, zPos].gridObjects[index]);
			return true;
		}
		found = null;
		return false;
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
				
				Debug.Log (x + ", " + z + ": " + grid [x, z].gridObjects[0] + " " + grid [x, z].gridObjects[1] + " " + grid [x, z].gridObjects[2]);
			}
		}
	}
}
