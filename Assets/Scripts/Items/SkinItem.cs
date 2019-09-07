using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinItem : BuyableItem {

	//custom values for Ball
	private void Awake() {
		type = "skin";
		price = 100;
		priceDiscount = 75;
		priceSuperDiscount = 50;
	}

	public int nVar;	//number of variations of the skin (not used yet)
	public List <Sprite> imgVar;	//variations sprites (not used yet)
	public List <string> descVar;	//variations descriptions (not used yet)
	
	public bool isBoughtSkins{get; protected set;}	//skins buy control

	public void SetBoughtSkins(){	//set skins bought
		isBoughtSkins = true;
	}
}
