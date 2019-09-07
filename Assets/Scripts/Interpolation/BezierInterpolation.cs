using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierInterpolation : MonoBehaviour
{

    public static BezierInterpolation instance = null;
    public Dictionary<string, AnimationCurve> curves;

     public static BezierInterpolation Execute{
        get{
            if(!instance){ 
                instance = new GameObject().AddComponent<BezierInterpolation>();
                instance.curves = new CurveData().LoadData();
                DontDestroyOnLoad(instance);
            }
            return instance;
        }
    } 

    public IEnumerator Blerp (Func<InterpolationData,float,bool> interpolate, InterpolationData data, float dur, bool pingpong = false, int loop = 0){
        AnimationCurve curve = data.curve;
        bool normal = true;
        int count = 0;
        float value = 0f, executionTime = 0f;
        while (pingpong || count <= loop ){
            bool isAlive = true;
            while(executionTime < dur && isAlive && normal || executionTime > 0 && isAlive && !normal)
            {
			    if (normal) executionTime += Time.deltaTime;
                else executionTime -= Time.deltaTime;
                value = executionTime/dur;
			    float perc = curve.Evaluate(value);
                isAlive = interpolate(data,perc);
			    yield return false;
		    }
           
            if (normal)
            {
                normal = false;
                value = 1;
                executionTime = dur;
            }
            else
            {
                normal = true;
                value = 0;
                executionTime = 0;
            }

            count++;
        }
        yield return true;
	}

    public AnimationCurve GetCurve(string name){
        if (instance.curves?.Count != null) return instance.curves[name];
        else instance.curves = new CurveData().LoadData();
        return instance.curves[name];
    }

    public bool InterpolateTransform(InterpolationData data, float perc){
        Transform trans = data.tr;
        if (trans == null) return false;
        if (data.setScale) trans.localScale +=  Vector3.LerpUnclamped(data.startVector,data.endVector,perc) - trans.localScale;
        else if (data.setPosition) trans.localPosition += Vector3.LerpUnclamped(data.startVector,data.endVector,perc) - trans.localPosition;
        return true;  
    }

    public bool InterpolateRectTransform(InterpolationData data, float perc){
        RectTransform rect = data.rt;
        if (rect == null) return false;
        rect.sizeDelta += Vector2.LerpUnclamped(data.startVector,data.endVector,perc) - rect.sizeDelta;
        return true;
    }

    public bool InterpolateBoxCollider(InterpolationData data, float perc){
        BoxCollider2D collider = data.bc;
        if (collider == null) return false;
        collider.size += Vector2.LerpUnclamped(data.startVector,data.endVector,perc) - collider.size;
        return true;
    }

    public bool InterpolateCircleCollider(InterpolationData data, float perc){
        CircleCollider2D collider = data.cd;
        if (collider == null) return false;
        collider.radius += Mathf.LerpUnclamped(data.startValue,data.endValue,perc) - collider.radius;
        return true; 
    }

    public bool InterpolateCanvasGroup(InterpolationData data, float perc)
    {
        CanvasGroup canvasGroup = data.cv;
        if (canvasGroup == null) return false;
        canvasGroup.alpha += Mathf.LerpUnclamped(data.startValue, data.endValue, perc) - canvasGroup.alpha;
        return true;
    }
}

public class InterpolationData
{
    public Vector3 startVector;
    public Vector3 endVector;
    public float startValue;
    public float endValue; 
    public float duration;
    public AnimationCurve curve;
    public bool setPosition;
    public bool setScale;

    public Transform tr = null;
    public RectTransform rt = null;
    public CircleCollider2D cd = null;
    public BoxCollider2D bc = null;
    public CanvasGroup cv = null;

}

public static class ExtensionMethods{

    //Transform\Rect Scale 
    public static Task InterpolateScale(this Transform trans, float duration, Vector2 start, Vector2 end, CurveName? curveName = null, AnimationCurve curve = null, bool pingPong = false, int loop = 0) {
        var _blerp = BezierInterpolation.Execute;
        InterpolationData data = new InterpolationData {
            startVector = start,
            endVector = end,
            tr = trans,
            setScale = true
        };
        if(curve != null) data.curve = curve;
        else if (curveName != null) data.curve = _blerp.GetCurve(curveName.ToString());
        else throw new Exception("You need to inform a name or an Animation Curve");
        return  new Task(_blerp.Blerp(_blerp.InterpolateTransform,data,duration,pingPong,loop));
    }

    //Transform\Rect Position
    public static Task InterpolatePosition(this Transform trans, float duration, Vector2 start, Vector2 end, CurveName? curveName = null, AnimationCurve curve = null, bool pingPong = false, int loop = 0) {
        var _blerp = BezierInterpolation.Execute;
        InterpolationData data = new InterpolationData {
            startVector = start,
            endVector = end,
            tr = trans,
            setPosition = true
        };
        if(curve != null) data.curve = curve;
        else if (curveName != null) data.curve = _blerp.GetCurve(curveName.ToString());
        else throw new Exception("You need to inform a name or an Animation Curve");
        return new Task(_blerp.Blerp(_blerp.InterpolateTransform,data,duration,pingPong,loop));
    }

    //RectTransform
    public static Task InterpolateSizeDelta(this RectTransform rect, float duration, Vector2 start, Vector2 end, CurveName? curveName = null, AnimationCurve curve = null, bool pingPong = false, int loop = 0) {
        var _blerp = BezierInterpolation.Execute;
        InterpolationData data = new InterpolationData {
            startVector = start,
            endVector = end,
            rt = rect
        };
        if(curve != null) data.curve = curve;
        else if (curveName != null) data.curve = _blerp.GetCurve(curveName.ToString());
        else throw new Exception("You need to inform a name or an Animation Curve");
        return new Task(_blerp.Blerp(_blerp.InterpolateRectTransform,data,duration,pingPong,loop));
    }

    //BoxCollider2D
    public static Task InterpolateSize(this BoxCollider2D box, float duration, Vector2 start, Vector2 end, CurveName? curveName = null, AnimationCurve curve = null, bool pingPong = false, int loop = 0) {
        var _blerp = BezierInterpolation.Execute;
        InterpolationData data = new InterpolationData {
            startVector = start,
            endVector = end,
            bc = box
        };
        if(curve != null) data.curve = curve;
        else if (curveName != null) data.curve = _blerp.GetCurve(curveName.ToString());
        else throw new Exception("You need to inform a name or an Animation Curve");
        return new Task(_blerp.Blerp(_blerp.InterpolateBoxCollider,data,duration,pingPong,loop));
    }

    //CircleCollider2D
    public static Task InterpolateRadius(this CircleCollider2D circle, float duration, float start, float end, CurveName? curveName = null, AnimationCurve curve = null, bool pingPong = false, int loop = 0) {
        var _blerp = BezierInterpolation.Execute;
        InterpolationData data = new InterpolationData
        { 
            startValue = start,
            endValue = end,
            cd = circle
        };       
        if(curve != null) data.curve = curve;
        else if (curveName != null) data.curve = _blerp.GetCurve(curveName.ToString());
        else throw new Exception("You need to inform a name or an Animation Curve");
        return new Task(_blerp.Blerp(_blerp.InterpolateCircleCollider,data,duration,pingPong,loop));
    }

    //CanvasGroup Alpha
    public static Task InterpolateCanvasAlpha(this CanvasGroup canvasGroup, float duration, float start, float end, CurveName? curveName = null, AnimationCurve curve = null, bool pingPong = false, int loop = 0)
    {
        var _blerp = BezierInterpolation.Execute;
        InterpolationData data = new InterpolationData
        {
            startValue = start,
            endValue = end,
            cv = canvasGroup
        };
        if (curve != null) data.curve = curve;
        else if (curveName != null) data.curve = _blerp.GetCurve(curveName.ToString());
        else throw new Exception("You need to inform a name or an Animation Curve");
        return new Task(_blerp.Blerp(_blerp.InterpolateCanvasGroup, data, duration, pingPong, loop));
    }

}
