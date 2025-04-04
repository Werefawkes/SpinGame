using CustomInspector.Extensions;
using UnityEditor;
using UnityEngine;

namespace CustomInspector.Editor
{
    [CustomPropertyDrawer(typeof(UnfoldAttribute))]
    public class UnfoldAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = PropertyValues.ValidateLabel(label, property);

            if (property.propertyType != SerializedPropertyType.Generic)
            {
                DrawProperties.DrawPropertyWithMessage(position, label, property, $"{nameof(UnfoldAttribute)} only valid on generics (a serialized class)", MessageType.Error);
                return;
            }
            //draw line in front
            Rect lineRect = new()
            {
                x = EditorGUI.IndentedRect(position).x - 7,
                y = position.y + 12,
                width = 1,
                height = position.height - 15,
            };
            EditorGUI.DrawRect(lineRect, new Color(.4f, .4f, .4f));

            //draw prop
            property.isExpanded = true;
            EditorGUI.BeginChangeCheck();
            DrawProperties.PropertyField(position, label, property);
            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Generic)
            {
                return DrawProperties.GetPropertyWithMessageHeight(label, property);
            }
            return DrawProperties.GetPropertyHeight(label, property);
        }
    }
}
