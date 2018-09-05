using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Module 6: Programming End Assignment
 * Made by Paulius Gagelas and Niels van Huizen
 * With Unity 5.5.0b and the HTC Vive
 * Supervisor: Angelika Mader
 * University of Twente, 2016
 */

// Class for creating an enemy
public class Enemy : MonoBehaviour {

    public float speed = 10;
	public Transform target; // Target it is trying to reach
	private GameObject noPathText;
	private int targetIndex; // Index of path points
    private Vector3[] path; // The path the enemy will follow
	private Pathfinding pathFinding;
	private IEnumerator followPathCoroutine;

    void Start() {
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound); // Request a new path
		noPathText = GameObject.FindGameObjectWithTag("Agent Stuck");
		noPathText.GetComponent<Text>().enabled = false;
    }

	public void UpdatePath(){
		StopCoroutine(followPathCoroutine);
		PathRequestManager.RequestPath(transform.position, target.position, OnPathFound); // Request a new path
	}

    // Method for checking if path is successful and saving it
    public void OnPathFound(Vector3[] newPath, bool pathSuccessful) {
		if (!pathSuccessful){
			noPathText.GetComponent<Text>().enabled = true;;
		}
        if (this != null && pathSuccessful) {
            path = newPath;
			if (followPathCoroutine != null){
				StopCoroutine(followPathCoroutine);
			} 
			followPathCoroutine = FollowPath();
            StartCoroutine(followPathCoroutine); // Start following the path
        }
    }

    // Method for following the path, given the waypoints
    IEnumerator FollowPath() {
        Vector3 currentWaypoint = path[0]; // Start at the first path node
        while (true) {
            if (transform.position == currentWaypoint) { // Reached the next waypoint
                targetIndex++; // Look for next waypoint
                if (targetIndex >= path.Length) { // If end is reached stop the method
                    Destroy(gameObject);
                    yield break;
                }
                currentWaypoint = path[targetIndex]; // Go to new waypoint
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime); // Making the move
            yield return null;
        }
    }

    // Method to show where waypoints are and the best path for the enemy
    public void OnDrawGizmos() {
        if(path != null) {
            for(int i = targetIndex; i < path.Length; i++) {
                Gizmos.color = Color.yellow;
                Gizmos.DrawCube(path[i],Vector3.one); // Draw a cube in the waypoint

                if(i == targetIndex) {
                    Gizmos.DrawLine(transform.position, path[i]); // Draw a line between enemy and next waypoint
                } else {
                    Gizmos.DrawLine(path[i-1], path[i]); // Draw the lines for all the next waypoints
                }
            }
        }
    }
}
