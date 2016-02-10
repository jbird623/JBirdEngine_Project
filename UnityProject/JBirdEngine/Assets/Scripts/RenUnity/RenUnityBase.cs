using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Text;

// TODO:
// Add library of story branches
// Add library of characters
// Add button to check for parsing errors
//    Verify Characters (speaking/entrance/exit)
//    Verify Moods
//    Verify Jumps
//    Verify Stats (set/check)
// Add actual decoding of story branches into UI
//    GoToNextLine
//    ChooseOption
// Add "none" keyword for not jumping to a new branch
// Add ability to add multiple commands in story branches
// Add name of character speaking (#[name]:)
// Add character moods/portraits (#mood [name] [mood]:)
// Add character entrances/exits (#enter [name] (location):/#exit [name] (location):) - default to center
// Add character stats changing (#stat [name] [stat] [n]:) - also for option consequence
// Add backgrounds (#bg [image]:)
// Add fade (#fade [seconds]:) - also for option consequences
// Add if statements to options (#if [stat] [n]:) - only for option consequence
// Add if statements to branching (#if [stat] [n] [jumpTo]: #jump [defaultJumpTo]) - also for option consequence, otherwise happens immediately

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

		}

		/// <summary>
		/// Dialogue object class.
		/// </summary>
		[System.Serializable]
		public class DialogueObject {
		
			[XmlAttribute("Text")]
			public string text;

            public DialogueObject () {
                text = "";
            }

			public DialogueObject (string str) {
                text = str;
			}

		}

		/// <summary>
		/// Used to serialize Story Branches as XML files.
		/// </summary>
		public static class StoryBranchXmlSerializer {

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

        public static class StoryBranchJsonSerializer {

            public static Branch Read (TextAsset file) {
                Branch newBranch = new Branch();
                using (StringReader reader = new StringReader(file.text)) {
                    string json = reader.ReadToEnd();
                    newBranch = JsonUtility.FromJson<Branch>(json);
                }
                return newBranch;
            }

            public static void Write (string fileName, Branch branch) {
                string json = JsonUtility.ToJson(branch, true);
                using (FileStream writer = new FileStream(fileName, FileMode.OpenOrCreate)) {
                    writer.Write(Encoding.UTF8.GetBytes(json), 0, Encoding.UTF8.GetByteCount(json));
                }
            }

        }

	}

}
