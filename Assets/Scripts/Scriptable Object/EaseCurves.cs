using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "EaseCurves", menuName = "Easing Curves/Curve List", order = 0)]

[ExecuteInEditMode]
public class EaseCurves : ScriptableObject {

    public List<Curve> curves;
    [HideInInspector] public List<string> curveIndex; // List with curves names.
    [HideInInspector] public AnimationCurve curveInput; 

    [HideInInspector] public bool addCurve;
    [HideInInspector] public bool serializeCurve;
    [HideInInspector] public string curveName;
    [HideInInspector] public bool removeCurve;
    [HideInInspector] public string curveToBeRemoved;
    
    private void OnEnable(){
        if (curves?.Count > 0 != true)  curves = new List<Curve>(); //check if is null (?)
        if (curveIndex?.Count > 0 != true)  curveIndex = new List<string>();
    } 

    public void CreateCurve(){
        AddTo(curveName,curveInput);
    }

    public void DeleteCurve(){
        Remove(curveToBeRemoved);
    }

    private void AddTo(string curveName, AnimationCurve curve){
        if (!curveIndex.Contains(curveName)){
            Curve tempCurve = new Curve();
            tempCurve.Register(curveName,curve); // Method defined in Curve Class
            curveIndex.Add(curveName);
            curves.Add(tempCurve);
        }
        else throw new Exception("Name already exists.");
    }

    private void Remove(string curveName){
        
        int index = 0;
        foreach (string name in curveIndex){
            if(curveName == name) {
                index = curveIndex.IndexOf(name); // Get the index position from the name typed.
                curves.RemoveAt(index);
                curveIndex.RemoveAt(index);
                break;
            }
        }
        
    }

    public void LoadData(){
       CurveAsset curveAsset = JsonUtility.FromJson<CurveAsset>(ReadDataFromFile());
       this.curves = curveAsset.curves;
       this.curveIndex = curveAsset.names;
    }

    public void SaveData(){
        CurveAsset curveAsset = new CurveAsset
        {
            curves = this.curves,
            names = this.curveIndex
        };
        WriteDataToFile(JsonUtility.ToJson(curveAsset,prettyPrint:true));
    }

    public void CreateEnum(){
        string enums = "public enum CurveName{\n";
        enums += String.Join(",\n",curveIndex);
        enums += "\n}";
        string path = Application.dataPath + "/CurveName.cs";
        Debug.Log("AssetPath:" + path);
        File.WriteAllText(path, enums);
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif

    }

    public void WriteDataToFile (string jsonString){
       
        string path = Application.dataPath + "/Curves.json";
        Debug.Log("AssetPath:" + path);
        File.WriteAllText(path, jsonString);
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    public string ReadDataFromFile(){

        string path = Application.dataPath + "/Curves.json";
        Debug.Log("AssetPath:" + path);
        return File.ReadAllText(path);
    }

    public AnimationCurve SearchCurve(string curveType){
        foreach (string name in curveIndex){
            if(curveType == name) {
                int index = curveIndex.IndexOf(name); // Get the index position from the name typed.
                return curves[index].curve;
            }
        }
        Debug.Log("Curve not found.");
        return AnimationCurve.Linear(1f,1f,1f,1f);
    }

}
