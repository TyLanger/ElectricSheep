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
		// for if I want the first letter of a word to be a different colour
		// this way, there is no space between the single letter and rest of the word
		public bool spaceAtEnd;
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
	bool tooBig = false;
	string[] overFlowWords;
	public float lineSpacing = 1;
	string[] words;
	int currentWordIndex = 0;
	TextMesh currentTextMesh;
	TextMesh lastTextMesh;
	int currentLetterIndex = 1;
	int currentLineIndex = 1;
	Vector3 lastWordPos;
	public float wordSpacing = 1;
	float lastXshift = 0;

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

				/**
				 * textComponent mode
				 * 
				 * 
				 */
				if (useTextComponent) {
					// use the special properties of the textComponents
					// i.e. different colour for different words
					// each text component is its own object
					if (currentTextMesh == null) {
						
						Vector3 wordPos;
						if (textComponentIndex == 0) {
							wordPos = Vector3.zero;
						} else {
							wordPos = lastWordPos + new Vector3 ((letterSpacing * wordWidth (lastTextMesh.text)), 0, 0);
						}

						if (wordPos.x > maxWidth) {
							// wrap words
							// relative to parent so 0
							//Debug.Log(wordPos.x);
							wordPos = new Vector3 (0, -lineSpacing * currentLineIndex, 0);
							currentLineIndex++;
						}


						Debug.Log (wordPos.x + " " + wordWidth (messages [textComponentIndex].message) * letterSpacing + " "+messages [textComponentIndex].message);
						if (((wordPos.x + wordWidth (messages [textComponentIndex].message) * letterSpacing) > maxWidth) || ((lastXshift + wordWidth (messages [textComponentIndex].message) * letterSpacing) > maxWidth)) {
							//Debug.Log ("Width: " + wordWidth (messages [textComponentIndex].message) * letterSpacing + " " + messages [textComponentIndex].message);
							tooBig = true;
						
							//Debug.Log ("wordWidth exceeeded maxWidth");
							// the message is too big to be on one line.
							// split it up and create more textComponents
							if (overFlowWords == null) {
								//Debug.Log ("First time init");
								overFlowWords = messages [textComponentIndex].message.Split (new char[]{ ' ' }, 30);
							}

							messages [textComponentIndex].message = wordsTilWrap (overFlowWords, wordPos.x);
							Debug.Log (messages [textComponentIndex].message + ".");
								
						}
						if ((wordWidth (messages [textComponentIndex].message) * letterSpacing) < maxWidth && (wordPos.x + wordWidth (messages [textComponentIndex].message) * letterSpacing) > maxWidth) {
							// only long enough with the wordPos.x
							Debug.Log("Only big enough with the x shift");
							lastXshift = wordPos.x;
						}

						typeSpeed = messages [textComponentIndex].typeSpeed;

						instantiateWord (wordPos, messages [textComponentIndex].message);
						currentTextMesh.color = messages [textComponentIndex].colour;
						//currentTextMesh.color = messages [textComponentIndex].colour;
						lastWordPos = wordPos;
					}			
					// draw the next letter
					// letterIndex < numLetters
					if (currentLetterIndex <= messages [textComponentIndex].message.Length && (currentTextMesh!= null)) {
						//Debug.Log (currentLetterIndex + ": " + messages [textComponentIndex].message.Length);
						currentTextMesh.text = messages [textComponentIndex].message.Substring (0, currentLetterIndex);
						currentLetterIndex++;
					} else {
						// new section 
						// move on to next textComponent in the array
						if (currentTextMesh != null) {
							lastTextMesh = currentTextMesh;
							currentTextMesh = null;
						}

						if (tooBig) {
							// if it was too big to fit into one textMesh
							// dont move on to the next textComponent yet
							Debug.Log("Too big: " + lastTextMesh.text);
							tooBig = false;

						} else {
							textComponentIndex++;
						}
						currentLetterIndex = 1;
						if (textComponentIndex == messages.Length) {
								
							typeOverTime = false;
						}
					}


				}
				/**
				 * Letter by letter
				 * 
				 * 
				 */
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

				/**
				 * word by word
				 * 
				 * 
				 */
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
		
	/// <summary>
	/// What do I even name this?
	/// It takes words and figures out how many will fit in a line before it should wrap
	/// It then puts those words back into the message for the current textComponent
	/// Extra words get put into overflow
	/// </summary>
	/// <param name="words">Words.</param>
	string wordsTilWrap(string[] words, float currentXoffset)
	{
		// figure out how many of the words to put into each textComponent
		float sumWidth = 0;

		string tempMessage = "";

		for (int i = 0; i < words.Length; i++) {

			if ((currentXoffset + sumWidth) >= maxWidth) {
				// don't add this word to the current textComponent
				sumWidth = 0;

				//messages[textComponentIndex].message = tempMessage;
				// add the remaining words to the overflow words
				overFlowWords = new string[words.Length - i];
				for (int j = 0; j < (words.Length-i); j++) {
					overFlowWords [j] = words [i + j];
				}

				return tempMessage; //.Remove(tempMessage.Length-1);
			} else {
				tempMessage += words [i] + " ";
			}
			// that is a weird edge case
			// this function gets called if the size of the message is greater than the line width
			// but sometime it gets to the end of this function
			// which should mean that it ISN'T bigger than maxWidth
			// but I didn't account for spaces before
			// so, if the message is too big because of some spaces, but not too big without the spaces,
			// it messes up.
			// fixed by adding a space with every word
			sumWidth += letterSpacing*wordWidth ((words [i] + " "));
		}
		// if it makes it here, that means it needed the last word to have enough width to be too big.
		overFlowWords = null;

		tooBig = false;
		if (messages [textComponentIndex].spaceAtEnd) {
			return tempMessage; //.Remove(tempMessage.Length-1);
		} else {
			return tempMessage.Remove(tempMessage.Length-1);
		}
	}

	/*
	void splitTextComponent ()
	{
		string[] tempWords = messages [textComponentIndex].message.Split(new char[]{ ' ' }, 30);

		// figure out how many of the words to put into each textComponent
		int sumWidth = 0;
		// make a new textComponent that is the same as the current one
		// being split right now
		textComponent tempText;
		tempText = messages [textComponentIndex];
		for (int i = 0; i < tempWords.Length; i++) {
			sumWidth += wordWidth (tempWords [i]);
			if (sumWidth >= maxWidth) {
				// don't add this word to the current textComponent
				sumWidth = 0;
				// do this word again as the start of a new one
				i--;
				// add the textComponent that is now as long as it can be to all the messages
				// them size of the array is made in the inspector to not have any extra room.....
				// might be hard/unnescessary to make a new array to fit it....
				// could just make a text component with the first few words,
				// then make a new text component with the remaining words at the same textComponentIndex
				// so I don't have to add extra space to the array
				messages[textComponentIndex] = tempText;
				tempText.message = "";
			} else {
				tempText.message += tempWords [i] + " ";
			}
		}
	}*/

	int wordsWidth(string[] words)
	{
		int sum = 0;
		int width = 0;
		// space deliminated
		for (int i = 0; i < words.Length-1; i++) {
			sum += wordWidth (words [i] + " ");
		}
		// dont add the space for the last word
		sum += wordWidth (words[words.Length - 1]);
		return sum;
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
