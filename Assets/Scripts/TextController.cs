using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextController : MonoBehaviour {


	public Transform cameraTrans;

	public TextMesh textMesh;
	public string message;
	public float letterSpacing;
	public float heightScale;

	public AnimationCurve xycurve;

	float minShakeStrength = 0.2f;
	float maxShakeStrength = 0.4f;
	float maxShakeTime = 0.3f;
	float minShakeTime = 0.1f;

	// Use this for initialization
	void Start () {
		var keys = xycurve.keys;
		for (int i = 0; i < message.Length-1; i++) {
			

			var text = Instantiate (textMesh, transform.position + new Vector3(i * letterSpacing, xycurve.Evaluate((float)i/message.Length)*heightScale, 0), cameraTrans.rotation) as TextMesh;

			/*
			Vector2 v1 = new Vector2((float)i/message.Length, xycurve.Evaluate((float)i/message.Length));
			Vector2 v2 = new Vector2((float)i/message.Length + 0.01f, xycurve.Evaluate((float)i/message.Length + 0.01f));
			var rot = Vector2.Angle (Vector2.right, (v2 - v1).normalized);

			
			Debug.Log (v1.ToString("F4"));
			Debug.Log (v2.ToString("F4"));
			Debug.Log ("Theta: " + rot);
			*/

			
			//text.transform.Rotate (new Vector3 (0,0,rot * ((v2.y - v1.y)/Mathf.Abs(v2.y - v1.y))));

			text.text = message.Substring (i, 1);
			/* This makes the letters shake kinda.
			 * Shelving this for now
			Shake textShake = text.GetComponent<Shake> ();
			textShake.shakeStrength = Mathf.Lerp (minShakeStrength, maxShakeStrength, (float)i/message.Length);
			//textShake.shakeTime = Mathf.Lerp (maxShakeTime, minShakeTime, (float)i/message.Length);
			*/
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
