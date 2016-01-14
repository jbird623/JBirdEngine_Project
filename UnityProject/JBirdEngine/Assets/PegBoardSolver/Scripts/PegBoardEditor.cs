using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum SolutionType {
	imperfect,
	perfect
}

#if UNITY_EDITOR
[CustomEditor(typeof(PegBoard))]
public class PegBoardEditor : Editor {

	public int solutionIndex;
	public SolutionType solutionType;
	public int lastPeg;

	public override void OnInspectorGUI () {
		PegBoard targetBoard = (PegBoard)target;
		base.OnInspectorGUI ();
		GUILayout.Space(10f);
		if (GUILayout.Button("Start Solving!")) {
			targetBoard.StartSolving();
		}
		GUILayout.Space(10f);
		if (GUILayout.Button("Force Stop")) {
			targetBoard.ForceStop();
		}
		GUILayout.Space(10f);
		if (GUILayout.Button("Reset Board")) {
			targetBoard.ResetBoard();
		}
		GUILayout.Space(10f);
		lastPeg = EditorGUILayout.IntField(lastPeg);
		if (GUILayout.Button("Find Solution With Peg Index")) {
			solutionIndex = targetBoard.FindSolutionWithLastPeg(lastPeg);
			solutionType = SolutionType.imperfect;
		}
		GUILayout.Space(10f);
		solutionType = (SolutionType)EditorGUILayout.EnumPopup(solutionType);
		solutionIndex = EditorGUILayout.IntField(solutionIndex);
		if (GUILayout.Button("Step Through Solution")) {
			targetBoard.StepThroughSolution(solutionType, solutionIndex);
		}
		GUILayout.Space(10f);
	}

}
#endif
