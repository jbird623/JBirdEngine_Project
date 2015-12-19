using UnityEngine;
using System.Collections;


public class testEnumStuff : MonoBehaviour {

    [System.Flags]//NEEDS TO HAVE SYSTEM.FLAGS ATTRIBUTE
    public enum Teststuff {
        setting1 = 0x1,
        setting2 = 0x2,
        setting3 = 0x3,
    }

    [JBirdEngine.EnumFlags]//USE THIS ATTRIBUTE TO SHOW AS FLAGS
    public Teststuff thing;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
