using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : GridObject {


	public bool broken = false;

	protected override void Start()
	{
		base.Start ();
		gridType = GridType.Fence;
	}
}
