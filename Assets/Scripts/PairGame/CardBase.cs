using UnityEngine;
using System.Collections;
//use this as base class
public class CardBase : MonoBehaviour {
	public string id;
	public bool isFlipped=false;
	public bool isPreviewed;
	public bool isSelected = false;
	public bool isPaired=false;
	public Vector3 originalPosition;
	public float noMatchTimer = 0;
	// Use this for initialization
	void Start () {
	
	}
	public void setPosition(Vector3 pos){
		originalPosition = pos;
	}

	// Update is called once per frame
	void Update () {
	}
	public virtual bool checkPair(CardBase otherCard){
		
		return false;
	}
	public virtual void flip(){
		transform.rotation = Quaternion.LookRotation (Vector3.back);
		isFlipped = true;
	}
}
