using UnityEngine;
using UnityEditor;
using System.Collections;

namespace ToolbAR
{
    public class UnityUtils : Editor
    {
        public static void expandHierarchy(GameObject go, bool expand)
        {
            // bail out immediately if the go doesn't have children
            if (go.transform.childCount == 0) return;

            // get a reference to the hierarchy window
            var hierarchy = getFocusedWindow("Hierarchy");

            // select our go
            selectObject(go);

            // create a new key event (RightArrow for collapsing, LeftArrow for folding)
            var key = new Event { keyCode = (expand) ? KeyCode.RightArrow : KeyCode.LeftArrow, type = EventType.keyDown };

            // finally, send the window the event
            hierarchy.SendEvent(key);
        }

        public static void selectObject(Object obj)
        {
            Selection.activeObject = obj;
        }

        public static EditorWindow getFocusedWindow(string window)
        {
            focusOnWindow(window);
            return EditorWindow.focusedWindow;
        }

        public static void focusOnWindow(string window)
        {
            EditorApplication.ExecuteMenuItem("Window/" + window);
        }
    }
}