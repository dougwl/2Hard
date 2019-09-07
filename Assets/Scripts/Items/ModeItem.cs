using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeItem : BuyableItem {

	//Custom values for modes
	private void Awake() {
		type = "mode";
		price = 300;
		priceDiscount = 200;
		priceSuperDiscount = 150;
	}
}
