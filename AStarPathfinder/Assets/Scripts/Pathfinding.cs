using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* Module 6: Programming End Assignment
 * Made by Paulius Gagelas and Niels van Huizen
 * With Unity 5.5.0b and the HTC Vive
 * Supervisor: Angelika Mader
 * University of Twente, 2016
 */

// Based on the code by Sebastian Lague
// Class for the pathfinding of an enemy
public class Pathfinding : MonoBehaviour {

    PathRequestManager pathRequestManager;
    Grid grid;

    void Awake() {
        pathRequestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>();
    }

	// Method for starting find path
    public void StartFindPath(Vector3 startPos, Vector3 targetPos) {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos) {
        Vector3[] wayPoints = new Vector3[0];
        bool pathSuccess = false; // A bool to check if path is possible
        Node startNode = grid.NodeFromWorldPoint(startPos);     // The algorithm starts at this point
        Node targetNode = grid.NodeFromWorldPoint(targetPos);   // The algorithm ends at this point

        if (startNode.walkable && targetNode.walkable) { // If start and end are walkable
            List<Node> openSet = new List<Node>(); // The set of node to be evaluated
            HashSet<Node> closedSet = new HashSet<Node>(); // The set of nodes already evaluated
            openSet.Add(startNode); // Add the first node to the openSet

            while (openSet.Count > 0) {
                Node currentNode = openSet[0]; // The currentNode is the startNode
                for (int i = 1; i < openSet.Count; i++) {

                    // If a node in the openSet has a fCost smaller than the fCost from the startNode, than this is the new currentNode
                    // On top of this, if the fCost of a node is equal to the fCost of the currentNode, than we look at the hCost
                    // If the hCost is smaller, than this is the new CurrentNode
					if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost) {
                        currentNode = openSet[i];
                    }
                }

                // We need to remove the newest currentNode from the openSet and put it into closedSet
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                // If the currentNode is the targetNode
                if (currentNode == targetNode) {
                    pathSuccess = true;
                    break; // Get out of the while
                }

                // We need to check for each neighbour if it is walkable and not already evaluated
                foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
                    if (!neighbour.walkable || closedSet.Contains(neighbour)) {
                        continue;
                    }

                    // If the new path to a neighbour is shorter or when the neighbour is not in the openSet, we:
                    // Set an fCost to the neighbour
                    // Set the parent of this neighbour to current, which is important for backtracking
                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour); // Cost = gcost + hcost
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                        neighbour.gCost = newMovementCostToNeighbour; // Set G cost
						neighbour.hCost = GetDistance(neighbour, targetNode); // Distance to end (H cost)
                        neighbour.parent = currentNode; // Set the parent of the neighbour

                        if (!openSet.Contains(neighbour)) {
                            openSet.Add(neighbour); // Add neighbour if it is not in the open set
                        }
                    }
                }
            }
            yield return null; // Wait for one frame and then return

            if (pathSuccess) {
                wayPoints = RetracePath(startNode, targetNode);
            }
            pathRequestManager.FinishedProcessingPath(wayPoints, pathSuccess); // Send the manager that we finished the path and its waypoints
        }
    }

    // We backtrack to get the shortest costing path
    Vector3[] RetracePath(Node startNode, Node endNode) {
        List<Node> path = new List<Node>();
        Node currentNode = endNode; // Make current the end node

        while (currentNode != startNode) {
            path.Add(currentNode);
            currentNode = currentNode.parent; // Make current the parent of the node
        }

		List<Vector3> wayPoints = new List<Vector3>();
		for (int i = 1; i < path.Count; i++) {
			wayPoints.Add (path [i].worldPosition); // Each waypoint gets a position
		}
		wayPoints.Reverse();
		return wayPoints.ToArray();
    }

    // We calculate the distance between two nodes, taking the absolute value to solely get positive values
    int GetDistance(Node nodeA, Node nodeB) {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX); // Get absolute distance
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        // If the x-distance is bigger, we calculate the distance with 14 being the cost for making a diagonal move and 10 for making a vertical or horizotal move
        if (dstX > dstY) return 14 * dstY + 10 * (dstX - dstY);

        // If the y-distance is bigger, we calculate the distance with 14 being the cost for making a diagonal move and 10 for making a vertical or horizotal move
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
