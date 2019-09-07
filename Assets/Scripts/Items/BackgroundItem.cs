using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundItem : BuyableItem {

	//Custom values for Background
	private void Awake() {
		type = "background";
		price = 100;
		priceDiscount = 75;
		priceSuperDiscount = 50;
	
	}

	public Sprite bg;

}
