using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSpawner : MonoBehaviour {
	class TooManyRobotSpawnFailures : System.Exception {
		public TooManyRobotSpawnFailures( string message ) : base( message ) { }
	}
	
	static RobotSpawner rs;

	[SerializeField] GameObject robotPrefab;
	[SerializeField] Transform floor;

	[SerializeField] LayerMask cantSpawnLayerMask;

	float minX, maxX, minY, maxY;
	
	float minSpawnTime = 2, maxSpawnTime = 8;

	public int maxRobots = 8;

	public static void HandleDeath(RobotController robot) {
		Destroy(robot.gameObject);
	}

	Vector3 PotentialRandomSpawnLocation() {
		return new Vector3(
			Random.Range(minX, maxX),
			Random.Range(minY, maxY)
		);
	}

	bool SpawnLocationCollides(Vector3 location) {
		return Physics2D.OverlapCircle(location, 0.5f, cantSpawnLayerMask);
	}

	Vector3 RandomSpawnLocation(int numTries = 5) {
		Vector3 result = PotentialRandomSpawnLocation();
		
		while (SpawnLocationCollides(result)) {
			if (numTries < 1) {
				throw new TooManyRobotSpawnFailures("Too many tries to spawn a robot, not going to try anymore this frame");
			}
			result = PotentialRandomSpawnLocation();
			numTries--;
		}

		return result;
	}

	IEnumerator SpawnRobots() {
		while (true) {
			if (GameManager.pc?.isDead ?? false || GameManager.isGameOver) {
				yield return null;
			}
			yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
			if (GameObject.FindGameObjectsWithTag("Robot").Length < maxRobots) {
				try {
					Vector3 spawnLocation = RandomSpawnLocation();
					Instantiate(robotPrefab, RandomSpawnLocation(), Quaternion.identity);
				} catch (System.Exception e) {
					Debug.Log(e);
				}
			}
		}
	}

	void Start () {
		rs = this;

		minX = -(floor.localScale.x / 2) + 1;
		maxX =  (floor.localScale.x / 2) - 1;
		minY = -(floor.localScale.y / 2) + 1;
		maxY =  (floor.localScale.y / 2) - 1;

		StartCoroutine(SpawnRobots());
	}
}
