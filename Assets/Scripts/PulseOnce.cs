using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseOnce : MonoBehaviour {

	//void Start () {
	//	StartCoroutine(PulseMe());
	//}

	//private IEnumerator PulseMe(){
	//	Vector3 transfOrig;
	//	float pp = 1; 
	//	float intensity = 0.15f;
	//	transfOrig = transform.localScale;


	//	while (pp < 1 + intensity*0.99){
	//		pp += Time.deltaTime/2;
	//		transform.localScale = new Vector3 (transfOrig.x * pp, transfOrig.y * pp, transfOrig.z);
	//		yield return null;
	//	}

	//	while (pp > 1*1.01){
	//		pp -= Time.deltaTime/2;
	//		transform.localScale = new Vector3 (transfOrig.x * pp, transfOrig.y * pp, transfOrig.z);
	//		yield return null;
	//	}
	//}

    public float max = 1.15f;

    private void Start()
    {
        transform.InterpolateScale(1f, transform.localScale, transform.localScale * max, CurveName.Linear);
    }
}
