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
public class BuilderVR : MonoBehaviour {

    private SteamVR_TrackedObject trackedObject; // HTC Vive Controller
    private SteamVR_Controller.Device device; // HTC Vive
    public GameObject obstaclePrefab; // Prefab of the obstacles 

    private bool firstPut; // Check if an obstacle has been placed 
    public GameObject tutorial; // Introductory text

    void Start() {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
    }

    void Update() {
        device = SteamVR_Controller.Input((int)trackedObject.index);
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad)) {
            SceneManager.LoadScene(0); // If touchpad is pressed then reset scene
        }
    }

    // Method for building obstacles when trigger is pressed on the map
    public void Build(Transform obstaclePlace) {
        if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger)) {
            if (!firstPut) {
                tutorial.gameObject.SetActive(false); // Turn off tutorial after we build first obstacle
                firstPut = true;
            }
            GameObject obstacle = (GameObject)Instantiate(obstaclePrefab, new Vector3(obstaclePlace.transform.position.x, 1, obstaclePlace.transform.position.z), Quaternion.identity); // Make an obstacle at the pressed controller position
            obstacle.transform.parent = GameObject.FindWithTag("Obstacles").transform; // Change the parent of the obstacle
        }
    }
}
