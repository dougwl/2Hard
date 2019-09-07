using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Curve
{ // Define the essential information from a curve.

    public string name;
    public AnimationCurve curve;

    public void Register(string nameInput, AnimationCurve curveInput)
    {
        name = nameInput;
        curve = curveInput;
    }
}

public class CurveAsset
{
    public List<string> names;
    public List<Curve> curves;
}


