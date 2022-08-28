using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace BMProject.UI
{
    [CustomEditor(typeof(BmUISprite))]
    public class BmUISpriteEditor : Editor
    {
        private bool debugRendereBounds = false;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var obj = target as BmUISprite;
            if(GUILayout.Button("Calc Box"))
            {
                obj.CalculateSelfBox();
            }

            debugRendereBounds = EditorGUILayout.Toggle("DebugGizmo",debugRendereBounds);
            obj.debugDrawGizmo = debugRendereBounds;


        }

    }
}