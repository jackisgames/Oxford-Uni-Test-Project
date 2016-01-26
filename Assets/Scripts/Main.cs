using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

	// Use this for initialization
	public static Main instance;
	public Camera cam;
	public Texture checkBox;
	public Texture redCross;
	[SerializeField]CardBase[] cardList;
	public GameState gameState;
	List<CardBase>activeCardPrefab=new List<CardBase>();
	List<CardBase>activeCard=new List<CardBase>();
	CardBase firstCard;
	CardBase secondCard;
	//bool isMatch;
	float waitTimer;
	void Awake(){
		instance = this;
	}
	public void previewCard(CardBase card){
		if (card.isPreviewed) {
			gameState = GameState.Input;
			card.isPreviewed = false;
		} else {
			foreach (CardBase c in activeCard) {
				c.isPreviewed = false;
			}
			card.isPreviewed = true;
			gameState = GameState.PreviewBig;
		}
	}
	public void selectCard(CardBase card){
		if (firstCard == null) {
			firstCard = card;
			firstCard.isSelected = true;
			previewCard (card);
		}else{
			if (firstCard.GetInstanceID () != card.GetInstanceID ()) {

				secondCard = card;
				previewCard (card);
				gameState = GameState.Result;
				Debug.Log ("result");
				//if (firstCard.checkPair (card)) {
				//	firstCard.isPaired=card.isPaired=true;
				//	secondCard = card;
				//	previewCard (card);
				//	gameState = GameState.Result;
				//	isMatch = true;
				//} else {
				//	isMatch = false;
				//	previewCard (card);
				//	firstCard.isSelected = card.isSelected = false;
				//	firstCard = null;

				//}


			}
		}
	}
	void Start () {
		updateState (GameState.Preview);
		ShuffleCard ();
	}
	void OnGUI(){
		switch(gameState){
		case GameState.Preview:
			if (GUI.Button (new Rect ((Screen.width - 200) / 2, 4, 200, 40), "Confirm")) {
				ShuffleCard ();
			}
			break;
		case GameState.Result:

			if (GUI.Button (new Rect (10, Screen.height - 50, 150, 50), "No Match")) {
				firstCard.isSelected = secondCard.isSelected = false;
				firstCard = null;
				secondCard = null;
				gameState = GameState.Input;
			}
			if (GUI.Button (new Rect (Screen.width-160, Screen.height - 50, 150, 50), "Match")) {
				if (firstCard.checkPair (secondCard)) {
					firstCard.isPaired = secondCard.isPaired = true;

					resumeGamefromNoMatch ();
				}else{
					firstCard.noMatchTimer = secondCard.noMatchTimer = 3;
					gameState = GameState.WaitConfirm;
					Invoke("resumeGamefromNoMatch",3);
				}



			}

			break;
		}
	}
	void resumeGamefromNoMatch(){
		firstCard.isSelected = secondCard.isSelected = false;
		gameState = GameState.Input;
		firstCard = null;
		secondCard = null;
	}
	//shuffle active cards
	void ShuffleCard(){
		//create random card holder
		List<CardBase> shuffledCard = new List<CardBase> ();
		for (int i = 0; i < 6; i++) {
			//fetch random card from active card
			CardBase randomCard = activeCard [Random.Range (0, activeCard.Count)];
			//add that random card to shuffled cards list
			shuffledCard.Add (randomCard);
			//remove random card from card list so it will not appear again
			activeCard.Remove (randomCard);
			//position the card on the new position
			randomCard.setPosition(new Vector3(1.5f-(i%3)*1.5f,-.75f+(int)(i/3)*1.5f,0));
		}
		activeCard = shuffledCard;
		gameState = GameState.Input;
	}
	void updateState(GameState state){
		if (state != gameState) {
			gameState = state;
			switch (gameState) {
			case GameState.Preview:
				//get 3 card randomly from cardlist
				List<CardBase> deck = new List<CardBase> ();
				//rebuild deck from card list
				deck.AddRange (cardList);
				activeCardPrefab.Clear ();

				while (activeCardPrefab.Count < 3) {
					//get random card from deck
					CardBase randomCard = deck [Random.Range (0, deck.Count)];
					//add that random card to active card
					activeCardPrefab.Add (randomCard);
					//remove that random card from deck
					deck.Remove (randomCard);
				}
				//spawn that random card to stage
				for(int i=0;i<activeCardPrefab.Count;i++){
					CardBase baseCardFront = (CardBase)Instantiate (activeCardPrefab[i], new Vector3 (-1.5f + i*1.5f, .75f, 0), Quaternion.identity);
					CardBase baseCardBack = (CardBase)Instantiate (activeCardPrefab[i], new Vector3 (-1.5f + i*1.5f, -.75f, 0), Quaternion.identity);
					baseCardFront.setPosition (baseCardFront.transform.position);
					baseCardBack.setPosition (baseCardBack.transform.position);
					baseCardBack.flip ();
					activeCard.Add (baseCardFront);
					activeCard.Add (baseCardBack);
				}
				break;
			
			}
		}
	}
	// Update is called once per frame
	void Update () {
		if (gameState == GameState.Result||gameState==GameState.WaitConfirm) {
			
				firstCard.transform.position = Vector3.MoveTowards (firstCard.transform.position, new Vector3 (-.75f, 0, -1), Time.deltaTime * 15);
				secondCard.transform.position = Vector3.MoveTowards (secondCard.transform.position, new Vector3 (.75f, 0, -1), Time.deltaTime * 15);
				firstCard.transform.localScale = Vector3.MoveTowards (firstCard.transform.localScale, Vector3.one * 1.5f, 15 * Time.deltaTime);
				secondCard.transform.localScale = Vector3.MoveTowards (secondCard.transform.localScale, Vector3.one * 1.5f, 15 * Time.deltaTime);

		}

	
	}
}
