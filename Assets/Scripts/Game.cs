using TMPro;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using static Unity.Mathematics.math;

//Main script for handling the overall game
public class Game : MonoBehaviour
{
	//References mazevisualization gameobject
	[SerializeField]
	MazeVisualization visualization;

	//Fields where two int values can be changed to determine maze size
	[SerializeField]
	int2 mazeSize = int2(5, 5);

	//Field where int input can determine seed, where 0 will be a random seed for generating maze
	[SerializeField, Tooltip("Use zero for random seed.")]
	int seed;

	//References player gameobject
	[SerializeField]
	Player player;

	//Field where float input determines opening dead ends probability
	[SerializeField, Range(0f, 1f)]
	float openDeadEndProbability = 0.5f;

	//List to contain enemy clones
	[SerializeField]
	List<GameObject> enemies;

	//Field to input number of desired enemies to spawn
	[SerializeField]
	int numberOfEnemies;

	//References enemy gameobject
	public GameObject Enemy;

	//References boss gameobject
	[SerializeField]
	Boss boss;

	//Bool that determines to see if the game is running/being played
	bool isPlaying;

	//Bool that determines if the game should be paused or not
	public bool isPaused;

	//References maze gameobject
	Maze maze;

	//References maze cell objects array
	MazeCellObject[] cellObjects;

	//References pausetracking gameobject
	[SerializeField]
	PauseTracking pausetracking;

	//References endtracking gameobject
	[SerializeField]
	EndTracking endtracking;

	//Int that keeps track of current floor number
	public int floorNumber = 1;

	//Bool that keeps track if the player is alive or dead
	public bool isAlive;

	//Sets floor number to 1 and isAlive to true when program starts
	void Start()
	{
		floorNumber = 1;
		isAlive = true;
	}

	//Initializes all the setup required to start a new game, including generating a maze and setting player, boss and enemy clones correctly
	void StartNewGame ()
	{
		isPlaying = true;
		isPaused = false;
		pausetracking.Unpause();
		Time.timeScale = 1f;
		maze = new Maze(mazeSize);
		new GenerateMazeJob
		{
			maze = maze,
			seed = seed != 0 ? seed : Random.Range(1, int.MaxValue),
			openDeadEndProbability = openDeadEndProbability
		}.Schedule().Complete();
		if (cellObjects == null || cellObjects.Length != maze.Length)
		{
			cellObjects = new MazeCellObject[maze.Length];
		}
		visualization.Visualize(maze, cellObjects);

		if (seed != 0)
		{
			Random.InitState(seed);
		}
		
		player.StartNewGame(new Vector3(mazeSize.x-1, 1, mazeSize.y-1));
		boss.StartNewGame(new Vector3(-mazeSize.x+1, 0, -mazeSize.y+1));
		
		enemies.Clear();
		Enemy.GetComponent<Enemy>().StartNewGame(maze, int2(200, 200), player);

		List<int2> usedCoordinates = new List<int2>();
		Vector3 bossPosition = boss.Position();

		while (usedCoordinates.Count != numberOfEnemies) {
			var newCoordinates = int2(Random.Range(0, mazeSize.x), Random.Range(0, mazeSize.y));
			
			if (!usedCoordinates.Contains(newCoordinates))
			{
				if (((newCoordinates[0] != 0) || (newCoordinates[1] != 0)) && ((newCoordinates[0] != mazeSize.x-1) || (newCoordinates[1] != mazeSize.y-1)))
				{
					usedCoordinates.Add(newCoordinates);
				}
			}
		}
		
		for (int i = 0; i < numberOfEnemies; i++)
		{
			GameObject enemyInstance = Instantiate(Enemy);
			enemies.Add(enemyInstance);
			enemies[i].GetComponent<Enemy>().StartNewGame(maze, usedCoordinates[i], player);
		}
	}

	//Update is called once per frame, which will start a new game if isPlaying is false
	void Update()
	{
		if (isPlaying)
		{
			UpdateGame();
		}
		else
		{
			StartNewGame();
			UpdateGame();
		}
	}

	//This update is also called once per frame (due to Update) and takes player movement while checking if the player pauses the game
	void UpdateGame()
	{
		player.Move();

		if (Input.GetKeyDown(KeyCode.P))
		{
			if (!isPaused)
			{
				isPaused = true;
				pausetracking.Pause();
				Time.timeScale = 0f;
			}
			else
			{
				isPaused = false;
				pausetracking.Unpause();
				Time.timeScale = 1f;
			}
		}
	}

	//Setup for ending the game, including checks for how the game should correctly end, destroying and deactivating things where necessary
	public void EndGame()
	{
		if (isAlive)
		{
			floorNumber = floorNumber + 1;
			if(floorNumber != 4)
			{
				isPlaying = false;
				player.EndGame();
				for (int i = 0; i < enemies.Count; i++)
				{
					enemies[i].GetComponent<Enemy>().EndGame();
				}
				boss.EndGame();

				for (int i = 0; i < cellObjects.Length; i++)
				{
					cellObjects[i].Recycle();
				}
			}
			else
			{
				player.InBattle();
				endtracking.GameEnd();
			}
		}
		else
		{
			player.InBattle();
			endtracking.GameEnd();
		}

		OnDestroy();
	}

	//Disposes the maze
	void OnDestroy ()
	{
		maze.Dispose();
	}
}