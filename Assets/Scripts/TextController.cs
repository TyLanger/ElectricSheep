using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextController : MonoBehaviour {

	// struct to handle different parts of text doing different things
	// i.e. moving, changing colour, etc.
	[System.Serializable]
	public struct textComponent {
		public string message;
		public float typeSpeed;
		public bool continuous;
		public Color colour;
	}

	public textComponent[] messages;

	public Transform cameraTrans;

	public TextMesh textMesh;
	public string message;
	public float letterSpacing;
	public float heightScale;
	CharacterInfo[] charInfo;
	static Dictionary<char, int> charWidthDic;

	// max width words can go before they get wrapped to a new line
	public float maxWidth;
	public float lineSpacing = 1;
	string[] words;
	int currentWordIndex = 0;
	TextMesh currentTextMesh;
	int currentLetterIndex = 1;
	int currentLineIndex = 1;
	Vector3 lastWordPos;
	public float wordSpacing = 1;

	//Transform movingWord;

	public bool curved = false;
	public AnimationCurve xycurve;

	Vector3 lastLetterPos;
	int spacing;
	char[] letter;

	public bool typeOverTime = false;
	public bool wordMode = false;
	public float typeSpeed = 1;
	float timeSinceLastLetter = 0;
	int letterIndex;
	public bool useTextComponent = false;
	int textComponentIndex = 0;

	/*
	float minShakeStrength = 0.2f;
	float maxShakeStrength = 0.4f;
	float maxShakeTime = 0.3f;
	float minShakeTime = 0.1f;
	*/

	// Use this for initialization
	void Start () {
		letterIndex = 0;
		var keys = xycurve.keys;
		lastLetterPos = Vector3.zero;
		lastWordPos = Vector3.zero;

		words = message.Split (new char[]{ ' ' }, 30); 


		// the characters in the message in alphabetical order
		charInfo = textMesh.font.characterInfo;
		// this is somehow always filled with the letters I want....
		// Even if I create a new 3D text and put that in textMesh
		// It is filled with all the letters I have used so far


		// TODO
		// dictionary should probably be somewhere more central so it doesn't get created for every 3D text object
		// something like a billboard that doesn't get destroyed on load
		charWidthDic = new Dictionary<char, int>();
		charWidthDic.Clear ();

		for (int t = 0; t < charInfo.Length; t++) {
			// fill up the dictionary
			if (!charWidthDic.ContainsKey ((char)charInfo [t].index)) {
				// only add if key doesnt exist
				charWidthDic.Add (((char)charInfo [t].index), charInfo [t].advance);
			}
		}

		if (curved) {

			for (int i = 0; i < message.Length - 1; i++) {
			
				letter = message.Substring (i, 1).ToCharArray();
				// needs to be char array to use the method


				// use 0, because you know there is only 1 char in the array
				// from substring(i, 1). the 1 means 1 char

				if (charWidthDic.TryGetValue (letter[0], out spacing)) {

					Vector3 letterPos = new Vector3 (lastLetterPos.x + spacing * letterSpacing, xycurve.Evaluate ((float)i / message.Length) * heightScale, 0);
					instantiateLetter (letterPos, letter[0]);
					lastLetterPos = letterPos;
				}
			}
		} else if(!typeOverTime){
			// creates the text at the start
			// skip this step if you're going to have the text appear over time
			var text = Instantiate (textMesh, transform.position, transform.rotation) as TextMesh;
			text.transform.parent = transform;
			text.text = message;
		}
	}
	
	// Update is called once per frame
	void Update () {
		// typeOverTime means letters show up one at a time
		if (typeOverTime) {
			if (Time.time > timeSinceLastLetter) {
				timeSinceLastLetter = Time.time + typeSpeed;

				if (useTextComponent) {
					// use the special properties of the textComponents
					// i.e. different colour for different words
					// each text component is its own object

					if (currentTextMesh == null) {
						Vector3 wordPos;
						if (textComponentIndex == 0) {
							wordPos = Vector3.zero;
						} else {
							wordPos = lastWordPos + new Vector3 ((wordSpacing + letterSpacing * wordWidth (words [Mathf.Max (0, currentWordIndex - 1)])), 0, 0);
						}

						if (wordPos.x > maxWidth) {
							// wrap words
							// relative to parent so 0
							wordPos = new Vector3 (0, -lineSpacing * currentLineIndex, 0);
							currentLineIndex++;
						}
						typeSpeed = messages [textComponentIndex].typeSpeed;
						instantiateWord (wordPos, messages [textComponentIndex].message);
						currentTextMesh.color = messages [textComponentIndex].colour;
						lastWordPos = wordPos;
					}					
					// draw the next letter
					// letterIndex < numLetters
					if (currentLetterIndex <= messages [textComponentIndex].message.Length) {
						currentTextMesh.text = messages [textComponentIndex].message.Substring (0, currentLetterIndex);
						currentLetterIndex++;
					} else {
						// new section 
						// move on to next textComponent in the array


						//currentTextMesh.color = messages [textComponentIndex].colour;
						currentTextMesh = null;
						textComponentIndex++;
						currentLetterIndex = 1;
						if (textComponentIndex == messages.Length) {
								
							typeOverTime = false;
						}
					}


				}

				// Create each letter as its own object
				// each letter appears one at a time after a delay of typeSpeed seconds
				else if (letterIndex < message.Length && !wordMode) {
				
					letter = message.Substring (letterIndex, 1).ToCharArray ();
					letterIndex++;
					if (charWidthDic.TryGetValue (letter [0], out spacing)) {

						Vector3 letterPos = new Vector3 ((lastLetterPos.x + spacing * letterSpacing), 0, 0);
						instantiateLetter (letterPos, letter [0]);
						lastLetterPos = letterPos;
					}
				
				} else if (wordMode) {
					// wordMode means each word is a different object instead of each letter being a different object

					// if the current letter to draw this frame
					// is less than the length of the word at currentWordIndex
					// continue
					// otherwise, move on to next word
					if (currentLetterIndex <= words [currentWordIndex].Length) {
						// go 1 past the length, because of Substring
						// it takes a length

						if (currentTextMesh == null) {
							// check this first so that the text is created,
							// then the correct message is put in
							// if this is after, the whole word will flash for a frame

							// use the width of the word before you to get your position
							// this is because of left-middle anchoring
							// right-middle anchoring uses your own width, but text shifts left as new letters are added.
							Vector3 initWordPos;
							if (currentWordIndex == 0) {
								initWordPos = Vector3.zero;
							} else {
								initWordPos = lastWordPos + new Vector3 ((wordSpacing + letterSpacing * wordWidth (words [Mathf.Max (0, currentWordIndex - 1)])), 0, 0);
							}

							if (initWordPos.x > maxWidth) {
								// wrap words
								// relative to parent so 0
								initWordPos = new Vector3 (0, -lineSpacing * currentLineIndex, 0);
								currentLineIndex++;
							}

							instantiateWord (initWordPos, words [0]);
							lastWordPos = initWordPos;
						}
						// add another letter to be displayed

						currentTextMesh.text = words [currentWordIndex].Substring (0, currentLetterIndex);
						currentLetterIndex++;

					} else {
						// make a new word
						currentWordIndex++;
						currentLetterIndex = 1;
						// set this to null to utilize the if statement above
						// it handles the initialization
						currentTextMesh = null;

						if (currentWordIndex >= words.Length) {
							// reached the end of the words
							//TODO make better end condition
							// temporary failsafe
							typeOverTime = false;

							// rotate after all the words are in place to get the rotation I want
							// doing this because it is easier than accounting for it when I initialy
							// initialize the words
							// it makes the words shift after each line is already out so it looks silly
							//transform.Rotate (new Vector3 (0, -10f, 0));

						}
					}

				}
			}
		}
	}

	int wordWidth(string word)
	{
		// calculate the width of the word using the size of each letter accoring to the font
		int sum = 0;
		int width = 0;
		for (int i = 0; i < word.Length; i++) {
			var letter = word.Substring (i, 1);
			if (charWidthDic.TryGetValue (letter[0], out width)) {
				sum += width;
			}
		}
		return sum;
	}

	void instantiateWord(Vector3 position, string word)
	{
		// default to camera rotation
		instantiateWord (position, cameraTrans.rotation, word);
	}

	void instantiateWord(Vector3 position, Quaternion rotation, string word)
	{
		// create the word as an object
		var text = Instantiate (textMesh, transform.position + position, rotation) as TextMesh;
		text.transform.parent = transform;
		text.text = word;
		// change the anchor so that when the text gets typed out,
		// it appears to the right end of the word
		// otherwise, it shifts the word left and the new letter always appears in the same spot
		text.anchor = TextAnchor.MiddleLeft;
		// This is used to figure out which word was created last
		// for spacing purposes
		currentTextMesh = text;
	}

	void instantiateLetter(Vector3 position, char letter)
	{
		/* Make one function do the work
		 * default to the camera's rotation so it faces towards the camera
		*/
		instantiateLetter (position, cameraTrans.rotation, letter);
	}

	void instantiateLetter(Vector3 position, Quaternion rotation, char letter)
	{
		var text = Instantiate (textMesh, transform.position + position, rotation) as TextMesh;


		text.transform.parent = transform;
		// letter is a char. Use toString to convert to string
		text.text = letter.ToString();
	}
		
}
