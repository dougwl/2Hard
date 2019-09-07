using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour {

    //private Vector3 transfOrig;
    //private float pp; 
    //public float intensity = 0.15f;

    //private void Start() {
    //	transfOrig = transform.localScale;
    //}

    //void Update () {
    //	pp = 1 + Mathf.PingPong(Time.time * intensity, intensity);
    //	transform.localScale = new Vector3 (transfOrig.x * pp, transfOrig.y * pp, transfOrig.z);
    //}

    public float MaxSize = 0;
    public float Duration = 1;
    public CurveName CurveName;
    public bool PingPong = false;
    public int Loop = 0;

    private void Start()
    {
        MaxSize += 1;
        transform.InterpolateScale(Duration, transform.localScale, transform.localScale * MaxSize, curveName: CurveName , pingPong: PingPong, loop: Loop);
    }
}
