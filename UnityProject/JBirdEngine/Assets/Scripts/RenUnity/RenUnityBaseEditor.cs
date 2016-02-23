using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JBirdEngine {
	
	namespace RenUnity {

		#if UNITY_EDITOR
		[CustomEditor(typeof(RenUnityBase))]
		public class RenUnityBaseEditor : Editor {

			RenUnityBase targetBase;

			public override void OnInspectorGUI (){
				targetBase = (RenUnityBase)target;

				EditorGUILayout.Space();
				EditorGUILayout.LabelField("Conditional Flags (most recent first):", EditorStyles.boldLabel);
				EditorGUILayout.BeginVertical();
				GUI.enabled = false;
				for (int i = targetBase.conditionalFlags.Count - 1; i >= 0; i--) {
					EditorGUILayout.TextField(targetBase.conditionalFlags[i]);
				}
				GUI.enabled = true;
				EditorGUILayout.EndVertical();

				EditorGUILayout.Space();
				if (GUILayout.Button("Update Character List")) {
					targetBase.GetCharacters();
				}

				EditorGUILayout.Space();
				EditorGUILayout.LabelField("Characters:", EditorStyles.boldLabel);
				EditorGUILayout.BeginVertical();
				for (int i =0; i < targetBase.characterData.Count; i++) {
					EditorGUILayout.Space();
					GUI.enabled = false;
					EditorGUILayout.EnumPopup("Name:", targetBase.characterData[i].name);
					GUI.enabled = true;
					EditorGUILayout.BeginVertical();
					for (int j = 0; j < targetBase.characterData[i].stats.Count; j++) {
						GUI.enabled = false;
						EditorGUILayout.EnumPopup("Stat:", targetBase.characterData[i].stats[j].stat);
						GUI.enabled = true;
						targetBase.characterData[i].stats[j].value = EditorGUILayout.FloatField("Value:", targetBase.characterData[i].stats[j].value);
					}
					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.EndVertical();
			}

		}
		#endif

	}
}
