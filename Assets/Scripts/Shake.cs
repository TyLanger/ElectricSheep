using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour {

	Vector3 originalPos;
	int shakeDirection = 1;
	public float shakeStrength;
	public float shakeTime = 0.3f;

	float lastTime;
	// Use this for initialization
	void Start () {
		originalPos = transform.position;
		//InvokeRepeating ("shake", 1, shakeTime);
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > lastTime) {
			lastTime = Time.time + shakeTime;
			transform.position = new Vector3 (originalPos.x + shakeStrength * shakeDirection, originalPos.y, originalPos.z);
			shakeDirection *= -1;
		}
	}

	void shake()
	{
		
	}

}
