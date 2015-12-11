using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace JBirdEngine {

	namespace RenUnity {

		/// <summary>
		/// Branch class. Contains dialogue objects.
		/// </summary>
		[XmlRoot("Branch")]
		[System.Serializable]
		public class Branch {

			[XmlAttribute("BranchName")]
			public string branchName;

			[XmlArray("Script")]
			[XmlArrayItem("Line")]
			public List<DialogueObject> script;

			public Branch () {
				script = new List<DialogueObject>();
			}
			
			public void AddDialogueObject () {
				script.Add(new DialogueObject());
			}
			
			public void AddOptionObject () {
				script.Add(new DialogueObject(2));
			}

		}

		/// <summary>
		/// Dialogue object class.
		/// </summary>
		[System.Serializable]
		public class DialogueObject {
		
			[XmlAttribute("Text")]
			public string text;

			[SerializeField]
			[XmlArray("Options")]
			[XmlArrayItem("Option")]
			public List<JumpOption> options;

			public DialogueObject () {
				options = new List<JumpOption>();
			}

			public DialogueObject (int opt) {
				options = new List<JumpOption>();
				for (int i = 0; i < opt; ++i) {
					options.Add(new JumpOption());
				}
			}

		}

		/// <summary>
		/// Jump option class.
		/// </summary>
		[System.Serializable]
		public class JumpOption {

			[XmlAttribute("Text")]
			public string text;
			[XmlAttribute("JumpTo")]
			public string jumpTo;

		}

		/// <summary>
		/// Used to serialize Story Branches as XML files.
		/// </summary>
		public static class StoryBranchSerializer {

			/// <summary>
			/// Read the specified XML file and create a Story Branch from the data.
			/// </summary>
			/// <param name="file">File to parse.</param>
			public static Branch Read (TextAsset file) {
				XmlSerializer serializer = new XmlSerializer(typeof(Branch));
				using (StringReader reader = new StringReader(file.text)) {
					return serializer.Deserialize(reader) as Branch;
				}
			}

			/// <summary>
			/// Write the specified Story Branch to the specified File Name.
			/// </summary>
			/// <param name="fileName">File Name to write to.</param>
			/// <param name="branch">Story Branch to write.</param>
			public static void Write (string fileName, Branch branch) {
				Debug.Log (fileName);
				XmlSerializer serializer = new XmlSerializer(typeof(Branch));
				using (FileStream writer = new FileStream(fileName, FileMode.CreateNew)) {
					serializer.Serialize(writer, branch);
					Debug.LogFormat("Script written to {0}.", fileName);
				}
			}

		}

	}

}
