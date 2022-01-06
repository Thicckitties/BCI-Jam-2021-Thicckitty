using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

namespace Thicckitty
{

    [CustomPropertyDrawer(typeof(GroundDetectionData))]
    public class GroundDetectionDataPropertyDrawer : ACustomPropertyDrawer
    {
        protected override int DisplayOrGetPropertiesInField(SerializedProperty property, bool draw)
        {
            SerializedProperty groundDetectionType = property.FindPropertyRelative("groundDetectionType");
            SerializedProperty raycastLayerMask = property.FindPropertyRelative("raycastLayerMask");
            int numberOfProperties = GetNumberOfPropertiesInFields(groundDetectionType,
                raycastLayerMask);

            if (draw)
            {
                DisplayField(groundDetectionType, "Ground Detection Type");
                DisplayField(raycastLayerMask, "Raycast Layer Mask");
            }

            switch ((GroundDetectionType) groundDetectionType.enumValueIndex)
            {
                case GroundDetectionType.TYPE_USE_BOX_COLLIDER:
                {
                    SerializedProperty boxCollider = property.FindPropertyRelative("boxCollider");
                    SerializedProperty boxRaycastDistance = property.FindPropertyRelative("boxRaycastDistance");
                    SerializedProperty raycastYOffset = property.FindPropertyRelative("raycastYOffset");
                    numberOfProperties += GetNumberOfPropertiesInFields(boxCollider, boxRaycastDistance, raycastYOffset);

                    if (draw)
                    {
                        DisplayField(boxCollider, "Box Collider");
                        DisplayField(boxRaycastDistance, "Box Raycast Distance");
                        DisplayField(raycastYOffset, "Raycast Y Offset");
                    }
                    break;
                }
                case GroundDetectionType.TYPE_USE_RAYCAST_OFFSETS:
                {
                    SerializedProperty defaultRaycastData = property.FindPropertyRelative("defaultRaycastData");
                    SerializedProperty raycastOffsets = property.FindPropertyRelative("raycastOffsets");

                    int defaultRaycastDataPropertyCount = GetNumberOfPropertiesInField(defaultRaycastData, true);
                    numberOfProperties += defaultRaycastDataPropertyCount
                                          + GetNumberOfPropertiesInArrayField(raycastOffsets,
                                              (serializedProperty) => GetNumberOfPropertiesInField(serializedProperty)) + 1;

                    if (draw)
                    {
                        DisplayField(defaultRaycastData, 
                            defaultRaycastDataPropertyCount * PropertiesHeightDifference);
                        DisplayField(raycastOffsets);
                    }
                    break;
                }
            }

            // TODO: Get properties in the field.
            return numberOfProperties;
        }
    }
}
