using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {
	
	void Start () {
		JBirdEngine.AI.AIHelper.GetHeuristic(Vector3.zero, Vector3.one, JBirdEngine.AI.AIHelper.HeuristicMode.hexagonal);
	}

}
