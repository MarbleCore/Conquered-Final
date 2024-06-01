using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField]
	Color color = Color.white;

	Maze maze;

	Player player;

	int targetIndex;

	Vector3 targetPosition;

	[SerializeField]
	BattleSystem battle;

	[SerializeField]
	CameraTracking cameratracking;

	void Awake ()
	{
		GetComponent<Light>().color = color;
		GetComponent<MeshRenderer>().material.color = color;
		gameObject.SetActive(false);
	}

	public void StartNewGame (Maze maze, int2 newCoordinates, Player player)
	{	
		this.player = player;
		this.maze = maze;
		targetIndex = maze.CoordinatesToIndex(newCoordinates);
		targetPosition = transform.localPosition =
			this.maze.CoordinatesToWorldPosition(newCoordinates, transform.localPosition.y);
		gameObject.SetActive(true);
	}

	public Vector3 Position ()
	{
		return gameObject.transform.position;
	}

	public void EndGame() => Destroy(gameObject);

	public void InBattle ()
	{
		gameObject.SetActive(false);
	}

	public void Update()
	{
		Vector3 playerPosition = this.player.Position();
		Vector3 enemyPosition = Position();
		if (
			new Vector3 (
				enemyPosition.x - playerPosition.x,
				0,
				enemyPosition.z - playerPosition.z
			).sqrMagnitude < 0.3f
		)
		{
			int ID = Random.Range(0,3);
			this.battle.enemyID = ID;
			this.battle.StartBattle();
			InBattle();
			this.player.InBattle();
			this.cameratracking.Off();
			return;
		}
	}
}