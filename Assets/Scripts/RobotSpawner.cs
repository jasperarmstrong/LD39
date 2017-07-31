using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotSpawner : MonoBehaviour {
	class TooManyRobotSpawnFailures : System.Exception {
		public TooManyRobotSpawnFailures( string message ) : base( message ) { }
	}
	
	public static RobotSpawner rs;

	SpriteRenderer sr;

	[SerializeField] GameObject robotPrefab;
	[SerializeField] Transform floor;

	[SerializeField] LayerMask cantSpawnLayerMask;

	float minX, maxX, minY, maxY;
	
	float minSpawnTime = 1, maxSpawnTime = 6;

	int maxRobotsIncreaseProbability = 32;
	public int maxRobots = 8;

	public bool shouldSpawn = true;

	public void RollMaxRobotIncrease() {
		if (Random.Range(0, maxRobotsIncreaseProbability) == 0) {
			maxRobots++;
		}
	}

	public static void HandleDeath(RobotController robot) {
		Destroy(robot.gameObject);
		GameManager.kills++;
		GameManager.UpdateKillsUI();
		rs?.RollMaxRobotIncrease();
	}

	public static void KillAll() {
		foreach (GameObject go in GameObject.FindGameObjectsWithTag("Robot")) {
			go.GetComponent<Health>().Damage(DamageType.INSTAKILL);
		}
	}

	Vector3 PotentialRandomSpawnLocation() {
		return new Vector3(
			Random.Range(minX, maxX),
			Random.Range(minY, maxY)
		);
	}

	bool SpawnLocationCollides(Vector3 location) {
		return Physics2D.OverlapCircle(location, 3, cantSpawnLayerMask);
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
		while (!GameManager.isGameOver && !GameManager.isGameWon) {
			yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
			if (shouldSpawn) {
				if (GameObject.FindGameObjectsWithTag("Robot").Length < maxRobots) {
					try {
						Instantiate(robotPrefab, RandomSpawnLocation(), Quaternion.identity);
					} catch (System.Exception e) {
						Debug.Log(e);
					}
				}
			}
		}
	}

	void Start () {
		rs = this;

		sr = floor.GetComponent<SpriteRenderer>();

		minX = -(sr.size.x / 2) + 1;
		maxX =  (sr.size.x / 2) - 1;
		minY = -(sr.size.y / 2) + 1;
		maxY =  (sr.size.y / 2) - 1;

		StartCoroutine(SpawnRobots());
	}
}
