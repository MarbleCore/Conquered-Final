using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

//Handles the enemy gameobject within the maze
public class Enemy : MonoBehaviour
{
	//Sets the color of the object
    [SerializeField]
	Color color = Color.white;

	//References maze gameobject
	Maze maze;

	//References player gameobject
	Player player;

	//Int used for setting enemy coordinates within maze
	int targetIndex;

	//Position of enemy
	Vector3 targetPosition;

	//References battlesystem gameobject
	[SerializeField]
	BattleSystem battle;

	//References cameratracking gameobject
	[SerializeField]
	CameraTracking cameratracking;

	//Gets color and sets gameobject to false on awaken
	void Awake ()
	{
		GetComponent<MeshRenderer>().material.color = color;
		gameObject.SetActive(false);
	}

	//Gets random positions based on maze size and random coordinates, sets itself to that position, and sets itself to active when a new level is started
	public void StartNewGame (Maze maze, int2 newCoordinates, Player player)
	{	
		this.player = player;
		this.maze = maze;
		targetIndex = maze.CoordinatesToIndex(newCoordinates);
		targetPosition = transform.localPosition =
			this.maze.CoordinatesToWorldPosition(newCoordinates, transform.localPosition.y);
		gameObject.SetActive(true);
		transform.eulerAngles = new Vector3(0,180,0);
	}

	//Returns enemy's current position
	public Vector3 Position ()
	{
		return gameObject.transform.position;
	}

	//Destroys the gameobject whens the game ends
	public void EndGame()
	{
		Destroy(gameObject);
	}

	//Sets the gameobject to false when the player enters a battle with it
	public void InBattle ()
	{
		gameObject.SetActive(false);
	}

	//Update is called once per frame, which checks to see if the player's position is close enough to the enemy's position, and starts a battle if so
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