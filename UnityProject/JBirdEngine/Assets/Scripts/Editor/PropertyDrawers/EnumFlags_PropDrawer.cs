using UnityEngine;
using UnityEditor;

namespace JBirdEngine {

    [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    public class EnumFlags_PropDrawer : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            //base.OnGUI(position, property, label);

            property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
        }

    }
    
}
