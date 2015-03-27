using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ToolbAR.CSharpExtensions;

namespace ToolbAR.SceneManagement
{
    [CustomPropertyDrawer(typeof(SceneRef))]
    public class SceneRefDrawer : PropertyDrawer 
    {
        override public void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            float clearButtonWidth = 20f;

            Rect buttonRect = new Rect(position.x, position.y, position.width - clearButtonWidth, position.height);
            Rect clearButtonRect = new Rect(position.x + position.width - clearButtonWidth, position.y, clearButtonWidth, position.height);

            SerializedProperty pathProperty = property.FindPropertyRelative("mPath");
            string currentPath = pathProperty.stringValue;
            
            if (GUI.Button(buttonRect,
                new GUIContent(((currentPath.Length != 0) ? currentPath.Substring(0, currentPath.Length - ".unity".Length) : "<None>"), "Change the Referenced Scene")
                ))
            {
                //Selecta new scene
                string newpath = EditorUtility.OpenFilePanel(
                    "Select a Scene",
                    ((currentPath.Length != 0) ? Application.dataPath+"/"+currentPath : Application.dataPath),
                    "unity");
                if (newpath.Length != 0 && newpath.EndsWith(".unity"))
                {
                    newpath = PathExtensions.getRelativePathTo(newpath, Application.dataPath + "/");
                    if (newpath != currentPath)
                    {
                        currentPath = newpath;
                        pathProperty.stringValue = currentPath;
                    }
                }
            }
            Color guicolor = GUI.color;
            if (currentPath.Length == 0)
                GUI.enabled = false;
            else
                GUI.backgroundColor = new Color(0.9f, 0.4f, 0.4f);
            if (GUI.Button(clearButtonRect, new GUIContent("X", "Nullify Scene Reference")))
            {
                currentPath = "";
                pathProperty.stringValue = "";
            }
            GUI.backgroundColor = guicolor;
            GUI.enabled = true;

            EditorGUI.EndProperty();
        }
    }
}