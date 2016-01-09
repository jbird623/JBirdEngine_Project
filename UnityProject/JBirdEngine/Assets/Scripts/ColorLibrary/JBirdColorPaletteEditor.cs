using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace JBirdEngine {

	namespace ColorLibrary {

		#if UNITY_EDITOR
		[CustomEditor(typeof(JBirdColorPalette))]
		public class JBirdColorPaletteEditor : Editor {

			Color baseColor = Color.black;
			JBirdColorPalette targetPalette;

			void Analagous () {
				for (int i = 0; i < 5; i++) {
					targetPalette.colors[i].rgb = baseColor.ToHSV().ShiftHue(15 * (i - 2)).ToColor();
				}
			}

			public override void OnInspectorGUI () {
				targetPalette = (JBirdColorPalette)target;
				baseColor = EditorGUILayout.ColorField("Base Color", baseColor);
				EditorGUILayout.Space();
				if (GUILayout.Button("Analagous")) {
					Analagous();
				}
				EditorGUILayout.Space();
				foreach (ColorHelper.ColorHSVRGB color in targetPalette.colors) {
					EditorGUILayout.ColorField(color.rgb);
				}
				EditorGUILayout.Space();
				base.OnInspectorGUI();
				foreach (ColorHelper.ColorHSVRGB color in targetPalette.colors) {
					color.hsv = color.rgb.ToHSV();
				}
			}

		}
		#endif

	}

}
