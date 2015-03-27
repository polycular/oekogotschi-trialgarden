using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace ToolbAR.Vuforia
{

    [CustomEditor(typeof(ARDataSetBehaviour), true)]
    public class ARDataSetBehaviourEditor : Editor
    {
        void updateTrackablesFor(ARDataSetBehaviour dataset)
        {
            foreach (var go in ARUtilities.updateTrackablesFor(dataset))
            {
                EditorUtility.SetDirty(go);
                ImageTargetBehaviour it = go.GetComponent<ImageTargetBehaviour>();
                if (it != null)
                {
                    EditorUtility.SetDirty(it);
                }
            }

            UnityUtils.expandHierarchy(dataset.gameObject, true);
        }

        public override void OnInspectorGUI()
        {
            if (this.target.GetType() != typeof(ARDataSetBehaviour))
                return;
            ARDataSetBehaviour target = (ARDataSetBehaviour)this.target;

            if (target.transform.parent != ARScene.Behaviour.transform)
            {
                target.transform.parent = ARScene.GameObject.transform;
            }

            if (!target.hasName)
            {
                target.gameObject.name = "ARDataSet<Unknown>";
                //Has no DataSet Chosen, yet
                //Let choose a DataSet, excluding the ones that already have been chosen
                List<string> possible = ARScene.getFreeDataSets();
                if (possible.Count > 0)
                {
                    possible.Insert(0, "Choose a DataSet");
                    int chosenIdx = EditorGUILayout.Popup("DataSet", 0, possible.ToArray(), new GUILayoutOption[0]);
                    if (chosenIdx > 0)
                    {
                        //Something has ben selected
                        string chosen = possible[chosenIdx];
                        target.Name = chosen;
                        target.gameObject.name = "ARDataSet<" + target.Name + ">";
                        EditorUtility.SetDirty(target);
                        updateTrackablesFor(target);
                    }
                }
                else
                {
                    EditorGUILayout.LabelField("DataSet", "No DataSet left to assign");
                }

            }
            else
            {
                target.gameObject.SetActive(EditorGUILayout.Toggle("Activate DataSet", target.gameObject.activeInHierarchy));
                if (GUILayout.Button("Update Trackables"))
                {
                    updateTrackablesFor(target);
                }
            }
        }
    }
}