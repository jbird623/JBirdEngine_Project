using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JBirdEngine.Hexagonal;

public class PegBoard : MonoBehaviour {

	[System.Serializable]
	public class PegMove {

		PegSlot peg;
		HexGrid.ConnectionIndex direction;
		public bool done;

		public PegMove () {
			peg = null;
			direction = (HexGrid.ConnectionIndex)0;
			done = false;
		}

		public PegMove (PegSlot p, HexGrid.ConnectionIndex d) {
			peg = p;
			direction = d;
			done = false;
		}

		public PegMove (PegMove m) {
			peg = m.peg;
			direction = m.direction;
			done = false;
		}

		public bool isValid {
			get {
				if (peg == null) {
					return false;
				}
				PegSlot jumped = peg.connections[(int)direction];
				if (jumped == null) {
					return false;
				}
				PegSlot newSlot = jumped.connections[(int)direction];
				if (newSlot == null) {
					return false;
				}
				if (!done) {
					return (peg.hasPeg && jumped.hasPeg && !newSlot.hasPeg);
				}
				else {
					return (!peg.hasPeg && !jumped.hasPeg && newSlot.hasPeg);
				}
			}
		}

		public bool DoMove () {
			if (isValid) {
				peg.hasPeg = false;
				peg.connections[(int)direction].hasPeg = false;
				peg.connections[(int)direction].connections[(int)direction].hasPeg = true;
				done = true;
				return true;
			}
			return false;
		}

		public bool UndoMove () {
			if (isValid) {
				peg.hasPeg = true;
				peg.connections[(int)direction].hasPeg = true;
				peg.connections[(int)direction].connections[(int)direction].hasPeg = false;
				done = false;
				return true;
			}
			return false;
		}

	}

	[System.Serializable]
	public class Solution {

		public List<PegMove> moves;
		public int lastPegIndex;

		public Solution () {
			moves = new List<PegMove>();
			lastPegIndex = -1;
		}

		public Solution (List<PegMove> m, int i) {
			moves = new List<PegMove>();
			foreach (PegMove move in m) {
				moves.Add(new PegMove(move));
			}
			lastPegIndex = i;
		}

	}

	public PegSlot slotPrefab;

	public List<PegSlot> slots;

	public List<int> emptyPositions;

	public int baseSlots;
	public float slotSeparation;

	public float yieldWaitTime;

	public List<Solution> solutions;
	public List<Solution> perfectSolutions;

	private float _side = -1f;
	private float _apothem = -1f;

	private List<PegMove> currentMoves;

	private Solution currentSolution;
	private int solutionMoveIndex;

	void Awake () {
		slots = new List<PegSlot>();
		if (slotPrefab == null) {
			Debug.LogError("PegBoard: No slot prefab selected!");
		}
		solutions = new List<Solution>();
		currentSolution = null;
	}

	void Start () {
		CreateSlotTriangle(baseSlots);
	}

	public float side {
		get {
			if (_side == -1f) {
				_side = (float)(baseSlots - 1) * slotSeparation;
			}
			return _side;
		}
	}

	public float apothem {
		get {
			if (_apothem == -1f) {
				_apothem = (side / 2f) * Mathf.Sqrt(3f);
			}
			return _apothem;
		}
	}

	void CreateSlotTriangle (int size) {
		//dimensions
		float heightChange = apothem / (float)(size - 1);
		float widthChange = side / (float)(size - 1);
		float xPos = -(side / 2f) + transform.position.x;
		float zPos = -(apothem / 2f) + transform.position.z;
		float yPos = transform.position.y;
		//lists
		List<PegSlot> current = new List<PegSlot>();
		List<PegSlot> previous = new List<PegSlot>();
		//here we go
		for (int i = size; i > 0; i--) {
			//create straight line
			for (int j = 0; j < i; j++) {
				//create new slot
				PegSlot newSlot = Instantiate(slotPrefab, new Vector3(xPos, yPos, zPos), Quaternion.identity) as PegSlot;
				current.Add(newSlot);
				slots.Add(newSlot);
				//connect backwards
				if (j != 0) {
					newSlot.AddConnection(current[j - 1], HexGrid.ConnectionIndex.LeftDown);
				}
				//ready next position
				if (j != i - 1) {
					xPos += widthChange;
				}
			}
			//create teeth
			if (previous.Count != 0) {
				if (previous.Count != current.Count + 1) {
					Debug.LogError("PegBoard: Something about the logic is wrong!");
				}
				for (int j = 0; j < current.Count; j++) {
					//connect left
					current[j].AddConnection(previous[j], HexGrid.ConnectionIndex.Down);
					//connect right
					current[j].AddConnection(previous[j + 1], HexGrid.ConnectionIndex.RightDown);
				}
			}
			//ready next cycle
			previous.Clear();
			previous = new List<PegSlot>(current);
			current.Clear();
			xPos = ((((xPos - transform.position.x) * 2f) - widthChange) / -2f) + transform.position.x;
			zPos += heightChange;
		}
		foreach (int pos in emptyPositions) {
			if (0 <= pos && pos < slots.Count) {
				slots[pos].hasPeg = false;
			}
			else {
				Debug.LogWarning("PegBoard: Invalid empty position.");
			}
		}
	}

	public void StartSolving () {
		solutions = new List<Solution>();
		perfectSolutions = new List<Solution>();
		currentMoves = new List<PegMove>();
		StartCoroutine(FindSolutions());
	}

	IEnumerator FindSolutions () {
		int remainingSlots = 0;
		foreach (PegSlot slot in slots) {
			if (!slot.hasPeg) {
				continue;
			}
			else {
				remainingSlots++;
			}
			for (int i = 0; i < 6; i++) {
				PegMove move = new PegMove(slot, (HexGrid.ConnectionIndex)i);
				if (move.DoMove()) {
					//yield return new WaitForSeconds(yieldWaitTime);
					currentMoves.Add(move);
					yield return StartCoroutine(FindSolutions());
					move.UndoMove();
					currentMoves.Remove(move);
				}
			}
		}
		if (remainingSlots == 1) {
			int lastPeg = -1;
			for (int i = 0; i < slots.Count; i++) {
				if (slots[i].hasPeg) {
					lastPeg = i;
					break;
				}
			}
			if (emptyPositions.Contains(lastPeg)) {
				//Debug.Log("Perfect solution found!");
				perfectSolutions.Add(new Solution(currentMoves, lastPeg));
			}
			else {
				//Debug.LogFormat("Solution found for peg {0}!", lastPeg);
				solutions.Add(new Solution(currentMoves, lastPeg));
			}
		}
		yield break;
	}

	public void ForceStop () {
		StopAllCoroutines();
	}

	public void ResetBoard () {
		for (int i = 0; i < slots.Count; i++) {
			if (emptyPositions.Contains(i)) {
				slots[i].hasPeg = false;
			}
			else {
				slots[i].hasPeg = true;
			}
		}
	}

	public void StepThroughSolution (SolutionType type, int id) {
		Solution newSolution = null;
		if (type == SolutionType.imperfect) {
			if (0 > id || id >= solutions.Count) {
				Debug.LogWarning("PegBoard: Invalid solution index.");
			}
			else {
				newSolution = solutions[id];
			}
		}
		else {
			if (0 > id || id >= perfectSolutions.Count) {
				Debug.LogWarning("PegBoard: Invalid solution index.");
			}
			else {
				newSolution = perfectSolutions[id];
			}
		}
		if (newSolution != currentSolution) {
			ResetBoard();
			currentSolution = newSolution;
			foreach (PegMove move in currentSolution.moves) {
				move.done = false;
			}
			solutionMoveIndex = 0;
		}
		else {
			if (solutionMoveIndex < currentSolution.moves.Count) {
				currentSolution.moves[solutionMoveIndex].DoMove();
				solutionMoveIndex++;
				if (solutionMoveIndex == currentSolution.moves.Count) {
					Debug.Log("PegBoard: End of solution has been reached.");
				}
			}
			else {
				Debug.Log("PegBoard: No more moves can be made.");
			}
		}
	}

	public int FindSolutionWithLastPeg (int lastPeg) {
		for (int i = 0; i < solutions.Count; i++) {
			if (solutions[i].lastPegIndex == lastPeg) {
				return i;
			}
		}
		return -1;
	}

}
