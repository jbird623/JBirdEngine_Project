using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class Character : ScriptableObject {

	[Header("The name that will be used in xml/inspector:")]
	public string codeName;
	[Header("The name that will appear in-game:")]
	public string displayName;

}
