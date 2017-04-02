using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

	Transform[,] grid;
	int xGridSize;
	int zGridSize;
	public float gridSpacing;

	void Start() {
		grid = new Transform[xGridSize, zGridSize];
	}

	Vector3 gridToVec3(int xGrid, int zGrid)
	{
		return new Vector3 (xGrid * gridSpacing, 0, zGrid * gridSpacing) + transform.position;
	}

	Transform getTransformAtGrid(int xGrid, int zGrid)
	{
		return grid [xGrid, zGrid];
	}

}
