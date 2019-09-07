using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyableItem : MonoBehaviour {

	public virtual bool isBought {get; protected set;}	//buy control (not used yet)
	public virtual bool isActive {get; protected set;}	//customization control (not used yet)

	public virtual int price{get; protected set;}	//normal price
	public virtual int priceDiscount{get; protected set;}	//sale price (not used yet)
	public virtual int priceSuperDiscount{get; protected set;}	//super sale price (not used yet)

	public virtual string type{get; protected set;}	//for object type control (not used yet)
	public string description;	//object description to be displayed at the buy UI
	public string title; //object name
	public Sprite thumbnail; //object thumbnail in store
	public Sprite img; //object full image for the buy UI (not used yet)

	//set bought
	public void Bought(){ 
		isBought = true;
	}

	//set as active customization
	public void Activate(){
		isActive = true;
	}

	//set as disactive customization
	public void Desactivate(){
		isActive = false;
	}

	//set a new prices
	public void SetPrice(int nPrice){
		price = nPrice;
	}
	
	public void SetPrice(int nPrice, int nPriceD){
		price = nPrice;
		priceDiscount = nPriceD;
	}

	public void SetPrice(int nPrice, int nPriceD, int nPriceSD){
		price = nPrice;
		priceDiscount = nPriceD;
		priceSuperDiscount = nPriceSD;
	}

}
