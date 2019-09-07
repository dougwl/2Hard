using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opacity : MonoBehaviour {

	[SerializeField] private float op; 
	// Use this for initialization
	void Start () {
		SpriteRenderer sprite = GetComponent<SpriteRenderer>();
		sprite.color = new Color (sprite.color.r, sprite.color.g, sprite.color.b, op);
	}
}
