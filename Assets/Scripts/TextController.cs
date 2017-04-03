using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextController : MonoBehaviour {


	public Transform cameraTrans;

	public TextMesh textMesh;
	public string message;
	public float letterSpacing;

	public AnimationCurve xycurve;

	// Use this for initialization
	void Start () {
		
		for (int i = 0; i < message.Length; i++) {
			var text = Instantiate (textMesh, transform.position + new Vector3(i * letterSpacing, xycurve.Evaluate((float)i/message.Length), 0), cameraTrans.rotation) as TextMesh;
			text.text = message.Substring (i, 1);

		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
