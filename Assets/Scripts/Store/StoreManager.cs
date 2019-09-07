using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour {

	[SerializeField] private Transform canvas;

	[Header("Pages")]
	[SerializeField] private Transform offersPage;
	[SerializeField] private Transform packsPage;
	[SerializeField] private Transform skinsPage;
	[SerializeField] private Transform colorsPage;
	[SerializeField] private Transform musicsPage;
	[SerializeField] private Transform backgroundsPage;
	
	[Header("Coins")]
	[SerializeField] private Text playerCoins;

	private bool lerpUp = false;
	private bool lerpDown = false;

	//Pop-Up Buy Screen
	[Header("Buy Pop-Up")]
	[SerializeField] private GameObject buyHolder;
	[SerializeField] private Text buyText;
	[SerializeField] private Image buyIcon;
	[SerializeField] private Text buyTitle;
	[SerializeField] private Text buyDescription;
	[SerializeField] private Text buyContent;
	[SerializeField] private GameObject buyFade;

	
	//Each of these are the prefabs of each kind of item
	[Header("Prefabs")]
	[SerializeField] private GameObject defaultSkin;
	[SerializeField] private GameObject defaultMode;
	[SerializeField] private GameObject defaultMusic;
	[SerializeField] private GameObject defaultSpecial;
	[SerializeField] private GameObject defaultTrail;
	[SerializeField] private GameObject defaultPack;
	[SerializeField] private GameObject defaultBackground;

	//List of each kind item
	[Header("Object Lists")]
	private List <GameObject> skins;
	private List <GameObject> modes;
	private List <GameObject> musics;
	private List <GameObject> specials;
	private List <GameObject> trails;
	private List <GameObject> packs;
	private List <GameObject> backgrounds;

	//Sprite List of each kind of item (keep them in the same order as the creation)
	[Header("Sprite Lists")]
	[SerializeField] private List <Sprite> skinsThumb;
	[SerializeField] private List <Sprite> modesThumb;
	[SerializeField] private List <Sprite> musicsThumb;
	[SerializeField] private List <Sprite> specialsThumb;
	[SerializeField] private List <Sprite> trailsThumb;
	[SerializeField] private List <Sprite> packsThumb;
	[SerializeField] private List <Sprite> backgroundsThumb;
	
	//Add here what you want to add
	void Creation(){
		
		CreateASkin("Expensive Skin","This is the 1st Skin",1,300);
		CreateASkin("Cheap Skin","This is the 2nd Skin",1,50);
		CreateASkin("Skin 3","This is the 3rd Skin",1);
		CreateASkin("Skin 4","This is the 4th Skin",1);
		CreateASkin("Skin 5","This is the 5th Skin",1);
		
		CreateAMusic("Ex Music 1","This is the 1st Music",250);
		CreateAMusic("Ch Music 2","This is the 2nd Music",20);
		CreateAMusic("Music 3","This is the 3rd Music");
		CreateAMusic("Music 4","This is the 4th Music");
		CreateAMusic("Music 5","This is the 5th Music");
		
	}

	public void Gift(int n){
		PlayerPrefs.SetInt ("Coins", PlayerPrefs.GetInt ("Coins") + n);
		playerCoins.text = PlayerPrefs.GetInt ("Coins").ToString();
	}

	//call the buy screen
	public void BuyScreen(BuyableItem item){
		if (!item.isBought){
			lerpDown = false;
			lerpUp = true;
			
			//receive the Item info and transfer to the Pop Up UI
			buyTitle.text = item.title;
			buyDescription.text = item.description;
			buyIcon.sprite = item.thumbnail;
			buyContent.text = "1 " + item.type;
			buyText.text = item.price.ToString() + " C\nConfirm";

			//buyText.GetComponent<Button>().onClick.AddListener(() => CancelBuy());
			buyText.GetComponent<Button>().onClick.AddListener(() => ConfirmBuy(item)); //add the event on Buy click with the parameter being the BuyableItem
			
		}
	}

	//cancel the Buy Screen
	public void CancelBuy(){
		lerpUp=false;
		lerpDown=true;
		buyText.GetComponent<Button>().onClick.RemoveAllListeners(); //Clean Button Action List
	
	}

	public void ConfirmBuy(BuyableItem item){
		if (PlayerPrefs.GetInt("Coins")>= item.price){
			buyText.text = "Bought";
			PlayerPrefs.SetInt("Coins",PlayerPrefs.GetInt("Coins") - item.price);
			item.Bought();
			PlayerPrefs.SetInt(item.type + item.title,1);
			playerCoins.text = PlayerPrefs.GetInt("Coins").ToString();
			ShowPriceAll();
			CancelBuy();
		}
	}

	private void Update (){
		if (lerpUp) {
			buyFade.GetComponent<CanvasGroup>().interactable = true;
			buyFade.GetComponent<CanvasGroup>().blocksRaycasts = true;
            
			float decelerate = Mathf.Min(10 * Time.deltaTime, 1f);
            	
			buyHolder.transform.localPosition = Vector2.Lerp(buyHolder.transform.localPosition, new Vector3 (0,150), decelerate);
			buyFade.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(buyFade.GetComponent<CanvasGroup>().alpha, 1f, decelerate);
			
			if (Vector2.SqrMagnitude(buyHolder.transform.localPosition - new Vector3 (0,150)) < 0.25f) {
				buyHolder.transform.localPosition = new Vector3 (0,150);
				buyFade.GetComponent<CanvasGroup>().alpha =  1f;	
				lerpUp = false;
			}
    	}

		if (lerpDown)	{
			buyFade.GetComponent<CanvasGroup>().interactable = false;
			buyFade.GetComponent<CanvasGroup>().blocksRaycasts = false; 
				
			float decelerate = Mathf.Min(10 * Time.deltaTime, 1f);
            	
			buyHolder.transform.localPosition = Vector2.Lerp(buyHolder.transform.localPosition, new Vector3 (0,-1180), decelerate);
			buyFade.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(buyFade.GetComponent<CanvasGroup>().alpha, 0f, decelerate);
			
			if (Vector2.SqrMagnitude(buyHolder.transform.localPosition - new Vector3 (0,-1180)) < 0.25f) {
				buyHolder.transform.localPosition = new Vector3 (0,-1180);
				buyFade.GetComponent<CanvasGroup>().alpha =  0f;	
				lerpDown = false;
			}
		}
	}

	void Start () {
		playerCoins.text = PlayerPrefs.GetInt("Coins").ToString();

		//Init of the lists
		skins = new List<GameObject>();
		musics = new List<GameObject>();

		//Call the creations
		Creation();

		//Call the show price function
		ShowPriceAll();

		//Call the organizer
		Organize(skins);
		Organize(musics);
	}

	void ShowPriceAll(){
		ShowPrice(skins);
		ShowPrice(musics);
	}
	
	void ShowPrice(List <GameObject> storeItem){
		for (int i=0;i<storeItem.Count;i++){ //for each element of the list...
			BuyableItem itemStatus = storeItem[i].GetComponent<BuyableItem>(); // ...get the "BuyableItem"...
			storeItem[i].GetComponentInChildren<Text>().text = itemStatus.price  + " C"; //.. and add it's price to the text
			if (itemStatus.isBought) storeItem[i].GetComponentInChildren<Text>().text = "Bought";
		}
	}
	
	void Organize(List <GameObject> objList){
		
		int topPosition = -100;
		int rowsNumber = 3;
		int spaceBetweenY = -350;
		int spaceBetweenX = 350;
		
		int x = -spaceBetweenX;

		for (int i=0;i<objList.Count;i++){
			objList[i].transform.localPosition += new Vector3 (x,topPosition + (i/rowsNumber) * spaceBetweenY);
			x += spaceBetweenX;
			if (x>spaceBetweenX) x=-spaceBetweenX;
		}
	}

	//Create a Skin Item, needs its title, description and number of variations
	void CreateASkin(string title, string description, int nVar){
		
		skins.Add(Instantiate(defaultSkin,skinsPage)); //add a prefab in the place we need
		
		int i = skins.Count-1; //get the last object position
		SkinItem thisSkin = skins[i].GetComponent<SkinItem>(); //Get the SkinItem for better implementation
		
		thisSkin.thumbnail = skinsThumb[i];	//get the thumnail
		skins[i].GetComponentInChildren<Image>().sprite = thisSkin.thumbnail; //set the thumbnail in the Image component
		
		thisSkin.title = title; //set the title (name)
		thisSkin.description = description; //set the item description
		thisSkin.nVar = nVar; //(not used yet)

		if (PlayerPrefs.GetInt(thisSkin.type+thisSkin.title) == 1) thisSkin.Bought();

		thisSkin.GetComponent<Button>().onClick.AddListener(() => BuyScreen(thisSkin)); //add the event on click with the parameter being itself
	}

	//Create a Skin Item. Same as above but can also change the price 
	void CreateASkin(string title, string description, int nVar, int price){
		CreateASkin(title,description,nVar); //create the Skin
		skins[skins.Count-1].GetComponent<BuyableItem>().SetPrice(price); //set the price
	}


	//Bellow there is the "CreateA..." functions that work the same way as the "CreateASkin" 

	void CreateATrail(string title, string description, int nVar){
		
		trails.Add(Instantiate(defaultTrail,canvas));
		
		int i = trails.Count-1;
		TrailItem thisTrail = trails[i].GetComponent<TrailItem>();
		
		thisTrail.thumbnail = trailsThumb[i];
		trails[i].GetComponentInChildren<Image>().sprite = thisTrail.thumbnail;
		
		thisTrail.title = title;
		thisTrail.description = description;
		thisTrail.nVar = nVar;

		if (PlayerPrefs.GetInt(thisTrail.type+thisTrail.title) == 1) thisTrail.Bought();

		thisTrail.GetComponent<Button>().onClick.AddListener(() => BuyScreen(thisTrail));
	}

	void CreateATrail(string title, string description, int nVar, int price){
		CreateATrail(title,description,nVar);
		trails[trails.Count-1].GetComponent<BuyableItem>().SetPrice(price);
	}

	void CrateAPack(string title, string description, int nVar){
		
		packs.Add(Instantiate(defaultPack,packsPage));
		
		int i = packs.Count-1;
		SpecialPack thisPack = packs[i].GetComponent<SpecialPack>();
		
		thisPack.thumbnail = packsThumb[i];
		packs[i].GetComponentInChildren<Image>().sprite = thisPack.thumbnail;
		
		thisPack.title = title;
		thisPack.description = description;
		
		if (PlayerPrefs.GetInt(thisPack.type+thisPack.title) == 1) thisPack.Bought();

		thisPack.GetComponent<Button>().onClick.AddListener(() => BuyScreen(thisPack));
	}

	void CrateAPack(string title, string description, int nVar, int price){
		CrateAPack(title,description,nVar);
		packs[packs.Count-1].GetComponent<BuyableItem>().SetPrice(price);
	}

	void CreateAMusic(string title, string description){
		
		musics.Add(Instantiate(defaultMusic,musicsPage));
		
		int i = musics.Count-1;
		MusicItem thisMusic = musics[i].GetComponent<MusicItem>();
		
		thisMusic.thumbnail = musicsThumb[i];
		musics[i].GetComponentInChildren<Image>().sprite = thisMusic.thumbnail;
		
		thisMusic.title = title;
		thisMusic.description = description;

		if (PlayerPrefs.GetInt(thisMusic.type+thisMusic.title) == 1) thisMusic.Bought();

		thisMusic.GetComponent<Button>().onClick.AddListener(() => BuyScreen(thisMusic));
	}

	void CreateAMusic(string title, string description, int price){
		CreateAMusic(title,description);
		musics[musics.Count-1].GetComponent<BuyableItem>().SetPrice(price);
	}

	void CreateABackground(string title, string description){
		
		backgrounds.Add(Instantiate(defaultBackground,backgroundsPage));
		
		int i = backgrounds.Count-1;
		BackgroundItem thisBackground = backgrounds[i].GetComponent<BackgroundItem>();
		
		thisBackground.thumbnail = backgroundsThumb[i];
		backgrounds[i].GetComponentInChildren<Image>().sprite = thisBackground.thumbnail;
		
		thisBackground.title = title;
		thisBackground.description = description;

		if (PlayerPrefs.GetInt(thisBackground.type+thisBackground.title) == 1) thisBackground.Bought();

		thisBackground.GetComponent<Button>().onClick.AddListener(() => BuyScreen(thisBackground));
	}

	void CreateABackground(string title, string description, int price){
		CreateABackground(title,description);
		backgrounds[backgrounds.Count-1].GetComponent<BuyableItem>().SetPrice(price);
	}

}
