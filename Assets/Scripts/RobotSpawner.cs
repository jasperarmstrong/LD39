using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSpawner : MonoBehaviour {
	static RobotSpawner rs;

	[SerializeField] GameObject robotPrefab;
	[SerializeField] Transform floor;
	
	float minSpawnTime = 4, maxSpawnTime = 10;

	public int maxRobots = 6;
	public int numRobots = 0;

	public static void HandleDeath(RobotController robot) {
		if (rs != null && rs.numRobots > 0) {
			rs.numRobots--;
		}
		Destroy(robot.gameObject);
	}

	IEnumerator SpawnRobots() {
		while (true) {
			if (GameManager.pc?.isDead ?? false || GameManager.isGameOver) {
				yield return null;
			}
			yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
			if (numRobots < maxRobots) {
				numRobots++;
				Instantiate(robotPrefab, Vector3.right * 10, Quaternion.identity);
			}
		}
	}

	void Start () {
		rs = this;
		StartCoroutine(SpawnRobots());
	}
}
