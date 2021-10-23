using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TMovementData))]
public class TMovementDataPropertyDrawer : ACustomPropertyDrawer
{
    protected override int DisplayOrGetPropertiesInField(SerializedProperty property, bool draw)
    {
        SerializedProperty movementType = property.FindPropertyRelative("movementType");
        SerializedProperty useTransformReference = property.FindPropertyRelative("useTransformReference");
        SerializedProperty transformReference = property.FindPropertyRelative("transformReference");
        SerializedProperty rigidbodyReference = property.FindPropertyRelative("rigidbodyReference");

        int numberOfPropertiesInField = GetNumberOfPropertiesInField(movementType);
        if (draw)
        {
            DisplayField(movementType);
        }
        
        switch ((TMovementData.TMovementType)movementType.enumValueIndex)
        {
            case TMovementData.TMovementType.TYPE_TRANSFORM:
            {
                if (draw)
                {
                    DisplayField(useTransformReference);
                    if (useTransformReference.boolValue)
                    {
                        DisplayField(transformReference);
                    }
                }
                else
                {
                    numberOfPropertiesInField += GetNumberOfPropertiesInField(useTransformReference);
                    if (useTransformReference.boolValue)
                    {
                        numberOfPropertiesInField += GetNumberOfPropertiesInField(transformReference);
                    }
                }
            }
            break;
            case TMovementData.TMovementType.TYPE_RIGIDBODY:
            {
                if (draw)
                {
                    DisplayField(rigidbodyReference);
                }
                else
                {
                    numberOfPropertiesInField += GetNumberOfPropertiesInField(rigidbodyReference);
                }
            }
            break;
        }
        return numberOfPropertiesInField;
    }
}