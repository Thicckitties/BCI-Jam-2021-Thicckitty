using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Thicckitty
{
    public abstract class ACustomPropertyDrawer : PropertyDrawer
    {
        #region fields

        private Stack<int> _indentEdits = new Stack<int>();
        private Rect _currentRectDisplay;

        #endregion

        #region properties

        protected virtual int PropertiesHeightDifference => 20;

        #endregion

        #region methods

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int numberOfProperties = CalculateNumberOfProperties(property);
            return EditorGUIUtility.singleLineHeight * numberOfProperties +
                   EditorGUIUtility.standardVerticalSpacing * (numberOfProperties - 1);
        }

        private int CalculateNumberOfProperties(SerializedProperty property)
        {
            int calculatedPropertyHeight = GetNumberOfPropertiesInField(property);
            if (!property.hasChildren || !property.isExpanded)
            {
                return calculatedPropertyHeight;
            }

            return calculatedPropertyHeight
                   + DisplayOrGetPropertiesInField(property, false);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ResetValues(position);
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(this._currentRectDisplay, property, label);

            if (property.hasChildren && !property.isExpanded)
            {
                EditorGUI.EndProperty();
                return;
            }

            ExtendRectYPosition(PropertiesHeightDifference);
            this.Indent();
            DisplayOrGetPropertiesInField(property, true);
            this.Outdent();
            EditorGUI.EndProperty();
        }

        /// <summary>
        /// Abstract function that is used to either display or get # of properties in a field.
        /// to 
        /// </summary>
        /// <param name="property">The property that we are getting the properties of.</param>
        /// <param name="draw">Determines whether to draw properties in a field or just get number of properties.</param>
        /// <returns>Int - The number of properties in field or 0 if draw is false.</returns>
        abstract protected int DisplayOrGetPropertiesInField(SerializedProperty property, bool draw);

        private void ResetValues(Rect position)
        {
            this._indentEdits.Clear();
            this._currentRectDisplay = new Rect(position.x, position.y, position.width, PropertiesHeightDifference);
        }

        protected void Indent(int level = 1)
        {
            this._indentEdits.Push(EditorGUI.indentLevel);
            EditorGUI.indentLevel += level;
        }

        protected void Outdent()
        {
            EditorGUI.indentLevel = this._indentEdits.Pop();
        }

        public static int GetNumberOfPropertiesInField(SerializedProperty property, bool children = false)
        {
            if (property == null)
            {
                return 0;
            }

            if (!children || !property.hasChildren || !property.isExpanded)
            {
                return 1;
            }

            SerializedProperty copiedProperty = property.Copy();
            return copiedProperty.CountInProperty();
        }

        public static int GetNumberOfPropertiesInFields(params SerializedProperty[] properties)
        {
            int number = 0;
            foreach (SerializedProperty property in properties)
            {
                number += GetNumberOfPropertiesInField(property);
            }

            return number;
        }

        public static int GetNumberOfPropertiesInArrayField(SerializedProperty property,
            System.Func<SerializedProperty, int> numberOfPropertiesCallable)
        {
            SerializedProperty copiedProperty = property.Copy();
            int numberOfProperties = copiedProperty.CountInProperty();

            if (!copiedProperty.isExpanded)
            {
                return numberOfProperties;
            }

            if (property.arraySize <= 0)
            {
                return numberOfProperties + 1;
            }

            int additionalProperties = 0;
            for (int i = 0; i < property.arraySize; i++)
            {
                SerializedProperty newProperty = property.GetArrayElementAtIndex(i);
                additionalProperties += numberOfPropertiesCallable.Invoke(property);
            }

            return numberOfProperties + additionalProperties;
        }

        #region label_field

        protected void DisplayLabelField(string label)
        {
            DisplayLabelField(label, PropertiesHeightDifference);
        }

        protected void DisplayLabelField(string label, float heightDifference)
        {
            DisplayLabelField(new GUIContent(label), heightDifference);
        }

        protected void DisplayLabelField(string label, GUIStyle style)
        {
            DisplayLabelField(label, PropertiesHeightDifference, style);
        }

        protected void DisplayLabelField(string label, float heightDifference, GUIStyle style)
        {
            DisplayLabelField(new GUIContent(label), heightDifference, style);
        }

        protected void DisplayLabelField(GUIContent label)
        {
            DisplayLabelField(label, PropertiesHeightDifference);
        }

        protected void DisplayLabelField(GUIContent label, float heightDifference)
        {
            DisplayLabelField(label, heightDifference, EditorStyles.label);
        }

        protected void DisplayLabelField(GUIContent label, float heightDifference, GUIStyle style)
        {
            EditorGUI.LabelField(_currentRectDisplay, label, style);
            ExtendRectYPosition(heightDifference);
        }

        #endregion


        protected void DisplaySeparator()
        {
            DisplaySeparator(PropertiesHeightDifference);
        }

        protected void DisplaySeparator(float separationLength)
        {
            DisplayLabelField("", separationLength);
        }

        /// <summary>
        /// Used to display a property field.
        /// </summary>
        /// <param name="property">The serialized property.</param>
        /// <param name="heightDifference">The height difference.</param>
        protected void DisplayField(SerializedProperty property)
        {
            DisplayField(property, PropertiesHeightDifference);
        }

        protected void DisplayField(SerializedProperty property, float heightDifference)
        {
            DisplayField(property, property?.displayName ?? "", heightDifference);
        }

        protected void DisplayField(SerializedProperty property, GUIContent content)
        {
            DisplayField(property, content, PropertiesHeightDifference, true);
        }

        protected void DisplayField(SerializedProperty property, GUIContent content, bool displayChildren)
        {
            DisplayField(property, content, PropertiesHeightDifference, displayChildren);
        }

        protected void DisplayField(SerializedProperty property, string label)
        {
            DisplayField(property, label, PropertiesHeightDifference);
        }

        protected void DisplayField(SerializedProperty property, string label, bool displayChildren)
        {
            DisplayField(property, new GUIContent(label), PropertiesHeightDifference, displayChildren);
        }

        protected void DisplayField(SerializedProperty property, string label, float heightDifference)
        {
            DisplayField(property, new GUIContent(label), heightDifference, true);
        }

        protected void DisplayField(SerializedProperty property, string label, float heightDifference, bool displayChildren)
        {
            DisplayField(property, new GUIContent(label), heightDifference, displayChildren);
        }

        protected void DisplayField(SerializedProperty property, GUIContent content, float heightDifference,
            bool forceDisplayChildren)
        {
            if (property == null)
            {
                return;
            }

            EditorGUI.PropertyField(this._currentRectDisplay, property, content,
                forceDisplayChildren && property.hasChildren && property.isExpanded);
            ExtendRectYPosition(heightDifference);
        }

        private void ExtendRectYPosition(float height)
        {
            _currentRectDisplay.y += height;
        }

        #endregion
    }
}
