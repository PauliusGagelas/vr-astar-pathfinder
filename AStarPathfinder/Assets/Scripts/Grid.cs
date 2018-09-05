using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Module 6: Programming End Assignment
 * Made by Paulius Gagelas and Niels van Huizen
 * With Unity 5.5.0b and the HTC Vive
 * Supervisor: Angelika Mader 
 * University of Twente, 2016
 */

// Based on the code by Sebastian Lague
// Class for making a grid
public class Grid : MonoBehaviour {


    public LayerMask unwalkableMask; // Unwalkable area
    public Vector2 gridWorldSize; // Size of the plane
    public float nodeRadius; // Size of a node
    Node[,] grid; // Double array of grid cells

    int gridSizeX, gridSizeY;

    void Awake() {
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x /  (2 * nodeRadius)); // Calculate gridsize and make it an integer
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y / (2 * nodeRadius));
		grid = new Node[gridSizeX, gridSizeY]; // Initialize a grid with its size
        CreateGrid();
    }

    void Update() {
        CreateGrid(); // Updating the grid for new obstacles
    }

    void CreateGrid() {
		Vector3 worldBottomLeft = transform.position + Vector3.left * gridWorldSize.x / 2 + Vector3.back * gridWorldSize.y / 2; // Find the bottom left of the grid

        for (int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * (2 * nodeRadius) + nodeRadius) + Vector3.forward * (y * (2 * nodeRadius) + nodeRadius); // Position of node
				bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask)); // Check if node in the world position is walkable ( If there is a collision then make it becomes false)
                grid[x, y] = new Node(walkable, worldPoint, x, y); // Make a new Node at the position in the grid
            }
        }
    }



    // Method for finding the current nodes neighbours
    public List<Node> GetNeighbours(Node node) {
		
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if (x == 0 && y == 0) { // This iteration can be skipped, it is the node given
                    continue;
                }

                // All neigbouring nodes
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                //if the neighbouring nodes are inside the world, add them to the list
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) { // If the nodes is in the grid
                    neighbours.Add(grid[checkX, checkY]); // Add it to the list of neighbours at their position
                }
            }
        }
        return neighbours;
    }


	// Method for finding the world point of the node
    public Node NodeFromWorldPoint(Vector3 worldPosition) {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x; // Percentages of where the node is situated
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX); // Values can only be between 0 and 1
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX); // Finding where this position is on the grid
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y]; // Return the corresponding node
    }

	// Method for drawing gizmos to see the walkable nodes
    void OnDrawGizmos() {
        if (grid != null) {
            foreach (Node n in grid) {
                Gizmos.color = (n.walkable) ? Color.white : Color.red; // If node is walkable then make it white if not then make it red
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (2 * nodeRadius));
            }
        }
    }
}