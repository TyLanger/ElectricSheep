using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour {


	public bool broken = false;
	public Grid grid;

	public int xStartGrid;
	public int zStartGrid;

	Vector3 movePos;

	// Use this for initialization
	void Start () {

		if(grid.gridToVec3(xStartGrid, zStartGrid, out movePos))
		{
			transform.position = movePos;
			grid.addToGrid (xStartGrid, zStartGrid, transform);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
