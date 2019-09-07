using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class CurveData {


    public Dictionary<string, AnimationCurve> LoadData()
    {

        Dictionary<string, AnimationCurve> Curves = new Dictionary<string, AnimationCurve>();

        TextAsset json = Resources.Load("Curves") as TextAsset;
        CurveAsset curveAsset = JsonUtility.FromJson<CurveAsset>(json.ToString());

        foreach (Curve thisCurve in curveAsset.curves)
        {
            Curves.Add(thisCurve.name,thisCurve.curve);
        }

        return Curves;

    }







}
