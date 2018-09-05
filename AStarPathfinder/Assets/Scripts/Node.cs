using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Module 6: Programming End Assignment
 * Made by Paulius Gagelas and Niels van Huizen
 * With Unity 5.5.0b and the HTC Vive
 * Supervisor: Angelika Mader 
 * University of Twente, 2016
 */

// Based on the code by Sebastian Lague
// Class for making a node
public class Node {
	public bool walkable; // We want to know if the node is walkable 
	public Vector3 worldPosition; // What point in the world the node represents
	public int gridX; // Position of the node
	public int gridY;

	public int gCost; // Cost to reach the node (start to node)
	public int hCost; // Cost to get from the node to the goal (node to end)
	public Node parent; // Parent of this specific node (Used for backtracking)


	public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY){ // Assign the values when creating a node
		walkable = _walkable;
		worldPosition = _worldPos;       
		gridX = _gridX;
		gridY = _gridY;
	}


	// Method for getting the fCost
	public int fCost{
		get{
			return gCost + hCost; // This is the estimated cost of the cheapest path from a node to the goal
		}
	}
}
