using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;

namespace Thicckitty.SODA
{
    
    [CustomEditor(typeof(Vector3Variable))]
    public class Vector3VariableEditor : Editor
    {

        private SerializedProperty _originalValue;
        private SerializedProperty _value;

        private SerializedProperty _minimumDistanceAmount;
        
        private void OnEnable()
        {
            _originalValue = serializedObject.FindProperty("originalValue");
            _value = serializedObject.FindProperty("value");
            _minimumDistanceAmount = serializedObject.FindProperty("minimumDistanceAmount");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.LabelField("Extra Values", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_minimumDistanceAmount, 
                new GUIContent("Changed Minimum Distance"));
            
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("SODA Values", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_originalValue,
                new GUIContent("Original Value"));
            EditorGUILayout.PropertyField(_value,
                new GUIContent("Value"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}