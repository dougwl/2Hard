using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialPack : BuyableItem {

	//Custom values for Packs
	private void Awake() {
		type = "pack";
		price = 200;
		priceDiscount = 150;
		priceSuperDiscount = 100;	
	}

	public List <BuyableItem> items;	//list of itens (not used yet)

	//get the thumbs of all items (not used yet)
	public List <Sprite> Thumbs(){
		List <Sprite> tempImage = new List<Sprite>();
		for (int i=0; i<items.Count; i++){
			tempImage.Add(items[i].thumbnail);
		}
		return tempImage;
	}
}
