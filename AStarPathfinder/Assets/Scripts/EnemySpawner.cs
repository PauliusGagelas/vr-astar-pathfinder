using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using System.Collections.Generic;

/* Module 6: Programming End Assignment
 * Made by Paulius Gagelas and Niels van Huizen
 * With Unity 5.5.0b and the HTC Vive
 * Supervisor: Angelika Mader 
 * University of Twente, 2016
 */

// Class for the enemy spawner
public class EnemySpawner : MonoBehaviour {

    public GameObject enemyPrefab; // Prefab for the enemy
	private List<Enemy> enemies = new List<Enemy>(); // Enemies spawned in the scene

    // Method for spawning a new enemy
    public void SpawnEnemy() {
        GameObject enemy = (GameObject)Instantiate(enemyPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation); // Make an enemy gameobject
		Transform target = GameObject.FindGameObjectWithTag("Target").transform; // Make a new target that corresponds to the real target
		enemy.GetComponent<Enemy>().target = target; // Update the target of the enemy
		enemy.transform.parent = GameObject.FindWithTag("Spawner").transform; // Change the parent of each enemy
		enemies.Add(enemy.GetComponent<Enemy>());
    }

	public void UpdateRoutes(){
		foreach(Enemy enemy in enemies){
			if (enemy != null){
				enemy.UpdatePath();
			}
		}
	}


}
