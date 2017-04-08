using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : GridObject {


	public float regrowTime = 5;
	float timeEatenAt;
	public bool eaten = false;

	float originalHeight = 2;
	float eatenHeight = 0.2f;

	protected override void Start() {
		base.Start();
		gridType = GridType.Grass;
	}

	
	// Update is called once per frame
	void Update () {
		if (eaten) {
			// make the grass get bigger as is grows back
			transform.localScale = new Vector3 ((Time.time - timeEatenAt) / regrowTime * originalHeight, (Time.time - timeEatenAt) / regrowTime * originalHeight, (Time.time - timeEatenAt) / regrowTime * originalHeight);
			if (Time.time > timeEatenAt + regrowTime) {
				regrowGrass ();

			}
		}
	}

	public void eatGrass()
	{
		transform.localScale = new Vector3(eatenHeight, eatenHeight, eatenHeight);
		timeEatenAt = Time.time;
		eaten = true;
	}

	void regrowGrass()
	{
		eaten = false;
		transform.localScale = new Vector3 (originalHeight, originalHeight, 1);
		//grid.addToGrid (xGridPos, zGridPos, this);
	}
}
