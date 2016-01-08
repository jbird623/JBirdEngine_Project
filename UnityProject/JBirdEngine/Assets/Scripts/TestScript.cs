using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {

	public JBirdEngine.ColorLibrary.ColorHelper.ColorHSVRGB hsvrgb;
	public JBirdEngine.ColorLibrary.ColorHelper.ColorHSV hsv;

	void Start () {
		JBirdEngine.AI.AIHelper.GetHeuristic(Vector3.zero, Vector3.one, JBirdEngine.AI.AIHelper.HeuristicMode.hexagonal);
	}

}
