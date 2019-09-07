using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownMove : MonoBehaviour {

    //private Vector3 transfOrig;
    //private float pp; 
    //public float intensity = 20f;

    //private void Start() {
    //	transfOrig = transform.localPosition;
    //}

    //void Update () {
    //	pp = Mathf.PingPong(Time.time * 2 * intensity , intensity);
    //	transform.localPosition = new Vector3 (transfOrig.x, transfOrig.y + pp, transfOrig.z);
    //}

    public float intensity = 20f;

    private void Start()
    {
        transform.InterpolatePosition(0.5f, transform.position, transform.position + new Vector3(0, intensity), CurveName.Linear, pingPong:true);
    }

}
