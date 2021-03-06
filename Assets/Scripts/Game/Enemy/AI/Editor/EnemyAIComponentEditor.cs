using System;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

namespace Thicckitty
{
    [CustomEditor(typeof(EnemyAIComponent))]
    public class EnemyAIComponentEditor : Editor
    {

        private SerializedProperty _attackType;
        private SerializedProperty _targetPosition;
        private SerializedProperty _rangedAttackData;

        private SerializedProperty _aiMovementSpeed;
        private SerializedProperty _controllerType;

        private SerializedProperty _sprite3DUpdaterData;
        
        private SerializedProperty _backAndForthData;
        private SerializedProperty _mimicMovementData;
        private SerializedProperty _zigZagAIData;

        private SerializedProperty _controlledByAnimations;
        private SerializedProperty _animator;
        private SerializedProperty _walkAnimation;
        private SerializedProperty _stunBeginAnimation;
        private SerializedProperty _stunEndAnimation;
        
        private SerializedProperty _groundDetectionData;

        private SerializedProperty _positionColor;
        
        private SerializedProperty _stunTime;
        private SerializedProperty _ballKnockbackMultiplier;
        private SerializedProperty _ballHitDirectionThreshold;
        private SerializedProperty _minBallSpeed;
        
        private void OnEnable()
        {
            _mimicMovementData = serializedObject.FindProperty("mimicMovementData");
            _aiMovementSpeed = serializedObject.FindProperty("aiMovementSpeed");
            _controllerType = serializedObject.FindProperty("controllerType");
            _backAndForthData = serializedObject.FindProperty("backAndForthData");
            _positionColor = serializedObject.FindProperty("positionColor");
            _zigZagAIData = serializedObject.FindProperty("zigZagAIData");
            _groundDetectionData = serializedObject.FindProperty("groundDetectionData");
            _sprite3DUpdaterData = serializedObject.FindProperty("sprite3DUpdaterData");
            _animator = serializedObject.FindProperty("animator");
            _attackType = serializedObject.FindProperty("attackType");
            _rangedAttackData = serializedObject.FindProperty("rangedAttackData");
            _targetPosition = serializedObject.FindProperty("targetPosition");
            _controlledByAnimations = serializedObject.FindProperty("controlledByAnimations");
            _walkAnimation = serializedObject.FindProperty("walkAnimation");
            _stunBeginAnimation = serializedObject.FindProperty("stunBeginAnimation");
            _stunEndAnimation = serializedObject.FindProperty("stunEndAnimation");
            _stunTime = serializedObject.FindProperty("stunTime");
            _ballKnockbackMultiplier = serializedObject.FindProperty("ballKnockbackMultiplier");
            _ballHitDirectionThreshold = serializedObject.FindProperty("ballHitDirectionThreshold");
            _minBallSpeed = serializedObject.FindProperty("minBallSpeed");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            EditorGUILayout.LabelField("Movement + Collision", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_aiMovementSpeed, 
                new GUIContent("Movement Speed"));
            EditorGUILayout.PropertyField(_groundDetectionData,
                new GUIContent("Ground Detection Data"));
            
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Visuals", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_sprite3DUpdaterData,
                new GUIContent("Sprite 3D Updater Data"));
            
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Animation", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_controlledByAnimations,
                new GUIContent("Behavior Controlled By Animations"));
            EditorGUILayout.PropertyField(_animator);
            EditorGUILayout.PropertyField(_walkAnimation);
            EditorGUILayout.PropertyField(_stunBeginAnimation);
            EditorGUILayout.PropertyField(_stunEndAnimation);

            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Stun/Knockback", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_stunTime);
            EditorGUILayout.PropertyField(_ballKnockbackMultiplier);
            EditorGUILayout.PropertyField(_ballHitDirectionThreshold);
            EditorGUILayout.PropertyField(_minBallSpeed);
            
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Movement", EditorStyles.boldLabel);
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
                case AIControllerType.ZIG_ZAG_MOVEMENT:
                {
                    EditorGUILayout.PropertyField(_zigZagAIData,
                        new GUIContent("AI Data"));
                }
                break;
            }
            EditorGUILayout.Separator();
            
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("Attack", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_attackType,
                new GUIContent("Attack Type"));
            if ((EnemyAttackType)_attackType.enumValueIndex != EnemyAttackType.TYPE_NONE)
            {
                EditorGUILayout.PropertyField(_targetPosition, 
                    new GUIContent("Target Position"));
            }
            switch ((EnemyAttackType) _attackType.enumValueIndex)
            {
                case EnemyAttackType.TYPE_RANGED_ENEMY:
                {
                    EditorGUILayout.PropertyField(_rangedAttackData,
                        new GUIContent("Ranged Attack Data"));
                    break;
                }
            }
            EditorGUILayout.Separator();
            
            EditorGUILayout.LabelField("Gizmos", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_positionColor,
                new GUIContent("Position Color"));
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}