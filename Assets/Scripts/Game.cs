using TMPro;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

using static Unity.Mathematics.math;

public class Game : MonoBehaviour
{
	[SerializeField]
	MazeVisualization visualization;

	[SerializeField]
	int2 mazeSize = int2(5, 5);

	[SerializeField, Tooltip("Use zero for random seed.")]
	int seed;

	[SerializeField]
	Player player;

	[SerializeField, Range(0f, 1f)]
	float openDeadEndProbability = 0.5f;

	[SerializeField]
	List<GameObject> enemies;

	[SerializeField]
	int numberOfEnemies;

	public GameObject Enemy;

	[SerializeField]
	Boss boss;

	bool isPlaying;

	public bool isPaused;

	Maze maze;

	MazeCellObject[] cellObjects;

	List<int2> coordinates;

	[SerializeField]
	PauseTracking pausetracking;

	[SerializeField]
	BattleSystem battle;

	public int floorNumber = 1;

	[SerializeField]
	EndTracking endtracking;

	public bool isAlive;

	void Start()
	{
		floorNumber = 1;
		isAlive = true;
	}

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

	void OnDestroy ()
	{
		maze.Dispose();
	}
}