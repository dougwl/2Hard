using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrailItem : BuyableItem {

	//Custom values for Trails
	private void Awake() {
		type = "trail";
		price = 100;
		priceDiscount = 75;
		priceSuperDiscount = 50;
	
	}
	
	public int nVar;	//Number of variations (not used yet)
	public List <Sprite> imgVar;	//sprites of the variations (not used yet)
	public List <string> descVar;	//descriptions of the variations (not used yet)
}
