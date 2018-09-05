using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/* Module 6: Programming End Assignment
 * Made by Paulius Gagelas and Niels van Huizen
 * With Unity 5.5.0b and the HTC Vive
 * Supervisor: Angelika Mader 
 * University of Twente, 2016
 */

// Based on the code by Sebastian Lague
// Class that handles the path requests done by enemies
public class PathRequestManager : MonoBehaviour {

    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>(); // Queue of path Requests
    PathRequest currentPathRequest; // Current request

    static PathRequestManager instance; // Instance of the class

    Pathfinding pathfinding;
    bool isProcessingPath; // Boolean to check if we are processing a path

    void Awake() {
        instance = this;
        pathfinding = GetComponent<Pathfinding>(); // Get the gameObjects pathfinding script
    }

	// Info we need to make a new path request
    struct PathRequest { 
        public Vector3 pathStart, pathEnd;
        public Action<Vector3[], bool> callBack;
        public PathRequest(Vector3 pathStart_, Vector3 pathEnd_, Action<Vector3[], bool> callBack_) {
            pathStart = pathStart_;
            pathEnd = pathEnd_;
            callBack = callBack_;
        }
    }

	// Method for requesting a path
    public static void RequestPath(Vector3 pathStart,Vector3 pathEnd, Action<Vector3[],bool> callBack) {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callBack);
        instance.pathRequestQueue.Enqueue(newRequest); // Add the new request inside of the queue
        instance.TryProcessNext(); // To see if we are processing a path
    }

    // Method for checking if we can process a new path
    void TryProcessNext() {
        if(!isProcessingPath && pathRequestQueue.Count > 0) { // If not processing a path and the queue is not empty
            currentPathRequest = pathRequestQueue.Dequeue(); // Take off the top item of the queue
            isProcessingPath = true;
			pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd); // Pathfinder finds path
        }
    }

    // Method for when we finished processing a path
    public void FinishedProcessingPath(Vector3[] path, bool success) {
        currentPathRequest.callBack(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }
}
