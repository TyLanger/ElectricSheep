using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextController : MonoBehaviour {


	public Transform cameraTrans;

	public TextMesh[] textMeshes;
	public string message;
	public float letterSpacing;

	// Use this for initialization
	void Start () {
		
		for (int i = 0; i < message.Length; i++) {
			var text = Instantiate (textMeshes[i], transform.position + new Vector3(i * letterSpacing, 0, 0), cameraTrans.rotation) as TextMesh;
			text.text = message.Substring (i, 1);

		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
