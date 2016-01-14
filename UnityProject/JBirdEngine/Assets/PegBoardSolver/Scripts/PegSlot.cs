using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JBirdEngine;
using JBirdEngine.Hexagonal;
using JBirdEngine.ColorLibrary;

public class PegSlot : MonoBehaviour {

	public bool hasPeg;

	public List<PegSlot> connections;

	private Renderer renderer;

	void Awake () {
		connections = new List<PegSlot>();
		for (int i = 0; i < 6; i++) {
			connections.Add(null);
		}
		renderer = GetComponent<Renderer>();
		if (renderer == null) {
			Debug.LogError("PegSlot: Peg does not have renderer!");
		}
	}

	void Update () {
		DrawDebugConnections();
		DrawPeg();
	}

	void DrawDebugConnections () {
		foreach (PegSlot slot in connections) {
			if (slot == null) {
				continue;
			}
			Debug.DrawLine(transform.position, slot.transform.position, MoreColors.mintIceCream);
		}
	}

	void DrawPeg () {
		renderer.enabled = hasPeg;
	}

	public void AddConnection (PegSlot other, HexGrid.ConnectionIndex direction) {
		connections[(int)direction] = other;
		other.connections[HexGrid.ReverseConnectionIndex(direction)] = this;
	}

}
