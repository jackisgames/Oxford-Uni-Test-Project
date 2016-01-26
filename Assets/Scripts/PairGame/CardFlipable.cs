using UnityEngine;
using System.Collections;

public class CardFlipable : CardBase {

	// Use this for initialization

	[SerializeField]Material frontSide;
	[SerializeField]Material backSide;
	[SerializeField]Renderer frontRenderer;
	[SerializeField]Renderer backRenderer;
	void Start () {
		frontRenderer.material = frontSide;
		backRenderer.material = backSide;
	}
	public override bool checkPair (CardBase otherCard)
	{
		return id == otherCard.id;
	}
	void OnGUI(){
		Vector3 screenSpace = Main.instance.cam.WorldToScreenPoint (transform.position);
		if (Main.instance.gameState == GameState.PreviewBig) {
			if (isPreviewed) {
				//bottom left button
				if (GUI.Button (new Rect (10, Screen.height - 110, 100, 100), "back")) {
					Main.instance.previewCard (this);
				}
				//bottom right
				if (!isSelected && !isPaired) {
					if (GUI.Button (new Rect (Screen.width - 110, Screen.height - 110, 100, 100), "Select")) {
						Main.instance.selectCard (this);
					}
				}

			}
		}else if(Main.instance.gameState==GameState.WaitConfirm){
			if (noMatchTimer>0) {
				noMatchTimer -= Time.deltaTime;
				GUI.DrawTexture(new Rect(screenSpace.x,Screen.height-screenSpace.y,100,100),Main.instance.redCross);
			}
		} else {
			if (Main.instance.gameState == GameState.Input) {
				if (isPaired) {
					GUI.DrawTexture(new Rect(screenSpace.x,Screen.height-screenSpace.y,25,25),Main.instance.checkBox);
				}


				if (!isSelected && !isPaired) {
					

					if (GUI.Button (new Rect (screenSpace.x - 50, Screen.height - screenSpace.y - 50, 100, 100), "")) {
						if (Main.instance.gameState == GameState.Input) {
							ImageClicked ();
						} else {
					
						}
					}
				}
			}
		}

		//GUI.Button(new Rect(
	}

	void ImageClicked(){
		Debug.Log ("image clicked " + id + " is flipped: " + isFlipped+" "+name);
		Main.instance.previewCard (this);
	}
	// Update is called once per frame
	void Update () {
		if (Main.instance.gameState != GameState.Result&&Main.instance.gameState != GameState.WaitConfirm) {
			if (isPreviewed) {
				transform.localScale = Vector3.MoveTowards (transform.localScale, Vector3.one * 3, Time.deltaTime * 15);
				transform.position = Vector3.MoveTowards (transform.position, new Vector3 (0, 0, -1), Time.deltaTime * 15);
			} else {
				transform.localScale = Vector3.MoveTowards (transform.localScale, Vector3.one, Time.deltaTime * 24);
				transform.position = Vector3.MoveTowards (transform.position, originalPosition, Time.deltaTime * 15);
			}
		}
	}
}
