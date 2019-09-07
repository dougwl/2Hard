using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicItem : BuyableItem {

	//Custom values for music
	private void Awake() {
		type = "music";
		price = 100;
		priceDiscount = 75;
		priceSuperDiscount = 50;
	
	}
}
