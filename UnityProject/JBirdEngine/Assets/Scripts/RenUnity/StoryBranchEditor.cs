using UnityEngine;
using System.Collections;
using UnityEditor;

namespace JBirdEngine {

	namespace RenUnity {

		/// <summary>
		/// Story branch custom inspector class.
		/// </summary>
		[CustomEditor(typeof(StoryBranch))]
		public class StoryBranchEditor : Editor {

			public TextAsset readFile;
			public string writeFileName = "Assets/RenUnity/Json/Untitled.txt";

			public override void OnInspectorGUI () {

				DrawDefaultInspector();

				StoryBranch editorTarget = (StoryBranch)target;

				GUILayout.Space(16);
				if (GUILayout.Button("Read From File")) {
					editorTarget.storyBranch = StoryBranchJsonSerializer.Read(readFile);
				}
				readFile = EditorGUILayout.ObjectField("File to read from:", readFile, typeof(TextAsset), true) as TextAsset;

				GUILayout.Space(16);
				if (GUILayout.Button("Write To File")) {
					StoryBranchJsonSerializer.Write(writeFileName, editorTarget.storyBranch);
				}
				writeFileName = EditorGUILayout.TextField("File to write to:", writeFileName);

			}

		}

	}

}
