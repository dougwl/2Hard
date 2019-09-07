using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EaseCurves))]

public class CustomCurvesEditor : Editor
{

   public override void OnInspectorGUI()
	{
		DrawDefaultInspector (); 

		EaseCurves easeCurveInstance = (EaseCurves) target;

        /* foreach (string name in easeCurveInstance.curveIndex)
        {
            EditorGUILayout.LabelField(name);
        }     */
        
        if (GUILayout.Button ("Save to File")){
			easeCurveInstance.SaveData();    
            Debug.Log("Data Saved.");
		}

        
		if (GUILayout.Button ("Load from File")){
			easeCurveInstance.LoadData();   
            Debug.Log("Data Loaded."); 
		}

        if (GUILayout.Button ("Create Enum")){
			easeCurveInstance.CreateEnum();   
            Debug.Log("EnumCreated."); 
		}

        if(easeCurveInstance.addCurve = EditorGUILayout.BeginToggleGroup("Add Curve",easeCurveInstance.addCurve)){

            easeCurveInstance.removeCurve = false;

            easeCurveInstance.curveInput = EditorGUILayout.CurveField("Select Curve: ",easeCurveInstance.curveInput);
            easeCurveInstance.curveName = EditorGUILayout.TextField("Curve Name",easeCurveInstance.curveName);

            this.Repaint();

		    if (GUILayout.Button ("Add"))  
		    {
			    easeCurveInstance.CreateCurve();
                easeCurveInstance.curveInput = new AnimationCurve();
                easeCurveInstance.curveName = "";  
		    }
        }

        EditorGUILayout.EndToggleGroup();
        

        if(easeCurveInstance.removeCurve = EditorGUILayout.BeginToggleGroup("Delete Curve",easeCurveInstance.removeCurve)){

            easeCurveInstance.addCurve = false;

            easeCurveInstance.curveToBeRemoved = EditorGUILayout.TextField("Curve Name",easeCurveInstance.curveToBeRemoved);

		    if (GUILayout.Button ("Delete"))    
		    {
			    easeCurveInstance.DeleteCurve();    
		    }
        }

        EditorGUILayout.EndToggleGroup();
        


	}
}
