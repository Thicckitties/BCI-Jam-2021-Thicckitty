using UnityEngine;
using UnityEditor;

namespace Thicckitty
{
    [CustomPropertyDrawer(typeof(EnemyBackAndForthAIData))]
    public class EnemyBackAndForthAIDataPropertyDrawer : ACustomPropertyDrawer
    {
        protected override int DisplayOrGetPropertiesInField(SerializedProperty property, bool draw)
        {
            SerializedProperty minDistance = property.FindPropertyRelative("minDistance");
            SerializedProperty referenceType = property.FindPropertyRelative("referenceType");
            SerializedProperty transformRefA = property.FindPropertyRelative("transformRefA");
            SerializedProperty transformRefB = property.FindPropertyRelative("transformRefB");
            SerializedProperty offsetA = property.FindPropertyRelative("offsetA");
            SerializedProperty offsetB = property.FindPropertyRelative("offsetB");

            int numberOfPropertiesInField = GetNumberOfPropertiesInFields(
                minDistance, referenceType);

            if (draw)
            {
                DisplayField(minDistance, "Minimum Distance Threshold");
                DisplayField(referenceType, "Position Reference Type");
            }

            switch ((EnemyBackAndForthAIData.BackAndForthAIReferenceType) referenceType.enumValueIndex)
            {
                case EnemyBackAndForthAIData.BackAndForthAIReferenceType.TYPE_TRANSFORM_REFERENCES:
                {
                    if (draw)
                    {
                        DisplayField(transformRefA, "Transform A");
                        DisplayField(transformRefB, "Transform B");
                    }

                    numberOfPropertiesInField += GetNumberOfPropertiesInFields(transformRefA, transformRefB);
                }
                break;
                case EnemyBackAndForthAIData.BackAndForthAIReferenceType.TYPE_START_POSITION_OFFSETS:
                {
                    if (draw)
                    {
                        DisplayField(offsetA, "Offset A");
                        DisplayField(offsetB, "Offset B");
                    }

                    numberOfPropertiesInField += GetNumberOfPropertiesInFields(offsetA, offsetB);
                }
                break;
            }
            return numberOfPropertiesInField;
        }
    }
}