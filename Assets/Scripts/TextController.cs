using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextController : MonoBehaviour {


	public Transform cameraTrans;

	public TextMesh textMesh;
	public string message;
	public float letterSpacing;
	public float heightScale;
	CharacterInfo[] charInfo;
	Dictionary<char, int> charWidthDic;

	public AnimationCurve xycurve;

	float minShakeStrength = 0.2f;
	float maxShakeStrength = 0.4f;
	float maxShakeTime = 0.3f;
	float minShakeTime = 0.1f;

	// Use this for initialization
	void Start () {
		var keys = xycurve.keys;

		// sorts the message into alphabetical order
		charInfo = textMesh.font.characterInfo;
		//Debug.Log ((70 - 'A'));

		charWidthDic = new Dictionary<char, int>();


		for (int t = 0; t < charInfo.Length; t++) {
			//Debug.Log(((char)charInfo[t].index) + " " + charInfo[t].advance + " " + charInfo[t].index);
			// fill up the dictionary
			charWidthDic.Add (((char)charInfo [t].index), charInfo [t].advance);
		}

		/* Dictionary testing
		int width;
		if(charWidthDic.TryGetValue('G', out width))
			Debug.Log ("G: " + width);
		if(charWidthDic.TryGetValue('o', out width))
			Debug.Log ("o: " + width);
		if(charWidthDic.TryGetValue('o', out width))
			Debug.Log ("o: " + width);
		if(charWidthDic.TryGetValue('d', out width))
			Debug.Log ("d: " + width);
		if(charWidthDic.TryGetValue(' ', out width))
			Debug.Log (" : " + width);
		if(charWidthDic.TryGetValue('D', out width))
			Debug.Log ("D: " + width);
		if(charWidthDic.TryGetValue('a', out width))
			Debug.Log ("a: " + width);
		if(charWidthDic.TryGetValue('y', out width))
			Debug.Log ("y: " + width);
		*/

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
			//var charInfo = text.font.characterInfo;

			text.text = message.Substring (i, 1);
			//Debug.Log (text.text + ": " + (text.text.ToCharArray()[0]+0));
			//charInfo [(char)text.text];

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

	/*
	 * Not needed anymore with the dictionary
	int getCharacterWidth(int unicode)
	{

		// char array only has some characters in it.
		// It has what I set as the message in the textMesh
		// Right now, I have ABC... abc... .,:;'!?(space)
		// the symbols are at the start of the array
		// starting at your unicode - 'A'
		// Should give an offset of 0 for A, 1 for B, etc.
		// Know there are always 52 letters, the length-52 is the number of symbols
		// add that on to get closer to the start of the letters(probably exact)
		// Adding numbers might change this
		// For symbols smaller than A, they will return a negative so the max function makes it always start at 0
		// numbers go first
		// then symbols
		// Capital
		// 7 spacees between Z a
		// lowercase
		// can't find lowercase
		for (int i = Mathf.Max(0, (unicode - 'A' + (charInfo.Length-52))); i < charInfo.Length; i++) {
			Debug.Log ("step");
			if (charInfo [i].index == unicode) {
				return charInfo [i].advance;
			}
		}
		return 0;
	}

	int getCharacterWidth(char character)
	{
		return getCharacterWidth (character + 0);
	}
	*/
}
