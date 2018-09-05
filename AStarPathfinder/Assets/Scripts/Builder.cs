using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Module 6: Programming End Assignment
 * Made by Paulius Gagelas and Niels van Huizen
 * With Unity 5.5.0b and the HTC Vive
 * Supervisor: Angelika Mader 
 * University of Twente, 2016
 */

// Class for building obstacles
public class Builder : MonoBehaviour {

    public GameObject obstaclePrefab; // Prefab of the obstacles 

    private bool firstPut; // Check if an obstacle has been placed 
    public GameObject tutorial; // Introductory text
	public Camera camera;

	private EnemySpawner enemySpawner;

	void Start(){
		camera = gameObject.transform.GetChild(0).GetComponent<Camera>();
		enemySpawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<EnemySpawner>();
	}

    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(0); // If touchpad is pressed then reset scene
        }

		RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);


		if (Physics.Raycast(ray, out hit)){
			Transform objectHit = hit.transform;

			if(Input.GetMouseButton(0) && objectHit.gameObject.tag == "Map"){
				Vector3 spawnPosition = hit.point;//Camera.ScreenToWorldPoint(Input.mousePosition);
				spawnPosition.y = 1.0f;
				Build(spawnPosition);
			}
			if (Input.GetMouseButton(1) && objectHit.gameObject.tag == "Obstacles"){
				Destroy(objectHit.gameObject);
			}
		}
	}

    // Method for building obstacles when trigger is pressed on the map
    public void Build(Vector3 obstaclePlace) {
            if (!firstPut) {
                tutorial.gameObject.SetActive(false); // Turn off tutorial after we build first obstacle
                firstPut = true;
            }
            GameObject obstacle = (GameObject)Instantiate(obstaclePrefab, new Vector3(obstaclePlace.x, 1, obstaclePlace.z), Quaternion.identity); // Make an obstacle at the pressed controller position
            obstacle.transform.parent = GameObject.FindWithTag("Obstacles").transform; // Change the parent of the obstacle
			enemySpawner.UpdateRoutes();
    }
}
