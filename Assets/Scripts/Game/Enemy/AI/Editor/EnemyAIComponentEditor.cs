using System;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

namespace Thicckitty
{
    [CustomEditor(typeof(EnemyAIComponent))]
    public class EnemyAIComponentEditor : Editor
    {

        private SerializedProperty _aiMovementSpeed;
        private SerializedProperty _controllerType;

        private SerializedProperty _sprite3DUpdaterData;
        
        private SerializedProperty _backAndForthData;
        private SerializedProperty _mimicMovementData;

        private SerializedProperty _groundDetectionData;

        private SerializedProperty _positionColor;

        private void OnEnable()
        {
            _mimicMovementData = serializedObject.FindProperty("mimicMovementData");
            _aiMovementSpeed = serializedObject.FindProperty("aiMovementSpeed");
            _controllerType = serializedObject.FindProperty("controllerType");
            _backAndForthData = serializedObject.FindProperty("backAndForthData");
            _positionColor = serializedObject.FindProperty("positionColor");
            _groundDetectionData = serializedObject.FindProperty("groundDetectionData");
            _sprite3DUpdaterData = serializedObject.FindProperty("sprite3DUpdaterData");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.LabelField("Movement", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_aiMovementSpeed, 
                new GUIContent("Movement Speed"));
            EditorGUILayout.PropertyField(_groundDetectionData,
                new GUIContent("Ground Detection Data"));
            
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Visuals", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_sprite3DUpdaterData,
                new GUIContent("Sprite 3D Updater Data"));
            
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("AI", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_controllerType,
                new GUIContent("Controller Type"));

            switch ((AIControllerType)_controllerType.enumValueIndex)
            {
                case AIControllerType.CONTROLLER_TYPE_BACK_AND_FORTH:
                {
                    EditorGUILayout.PropertyField(_backAndForthData,
                        new GUIContent("AI Data"));
                }
                break;
                case AIControllerType.CONTROLLER_TYPE_MIMIC_MOVEMENT:
                {
                    EditorGUILayout.PropertyField(_mimicMovementData,
                        new GUIContent("AI Data"));
                }
                break;
            }
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Gizmos", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_positionColor,
                new GUIContent("Position Color"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}