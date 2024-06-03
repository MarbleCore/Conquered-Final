using UnityEngine;

//Handles visualization of the maze using maze cell prefabs; which prefabs and rotations with each prefab is determind by flags associated with each cell
[CreateAssetMenu]
public class MazeVisualization : ScriptableObject
{
	//Array that holds rotation orientations, no rotation, 90 degrees, 180 degrees, and 270 degrees
	static Quaternion[] rotations =
	{
		Quaternion.identity,
		Quaternion.Euler(0f, 90f, 0f),
		Quaternion.Euler(0f, 180f, 0f),
		Quaternion.Euler(0f, 270f, 0f)
	};
	
	//References maze cell prefab gameobjects
	[SerializeField]
	MazeCellObject end, straight, corner, tJunction, xJunction;

	//Takes maze object and array of maze cell components and places appropriate maze cell components with rotation in maze based on flags
    public void Visualize (Maze maze, MazeCellObject[] cellObjects)
	{
		for (int i = 0; i < maze.Length; i++)
		{
			(MazeCellObject, int) prefabWithRotation = GetPrefab(maze[i]);
			MazeCellObject instance = cellObjects[i] = 
				prefabWithRotation.Item1.GetInstance();
			instance.transform.SetPositionAndRotation(
				maze.IndexToWorldPosition(i), rotations[prefabWithRotation.Item2]
			);
		}
	}

	//Takes flags for a specific cell and returns a tuple containing a maze cell component prefab with a rotation index (to be appropriately rotated when maze is visualized)
	(MazeCellObject, int) GetPrefab (MazeFlags flags) => flags switch
	{
		MazeFlags.PassageN => (end,0),
		MazeFlags.PassageE => (end,1),
		MazeFlags.PassageS => (end,2),
		MazeFlags.PassageW => (end,3),

		MazeFlags.PassageN | MazeFlags.PassageS => (straight,0),
		MazeFlags.PassageE | MazeFlags.PassageW => (straight,1),

		MazeFlags.PassageN | MazeFlags.PassageE => (corner,0),
		MazeFlags.PassageE | MazeFlags.PassageS => (corner,1),
		MazeFlags.PassageS | MazeFlags.PassageW => (corner,2),
		MazeFlags.PassageW | MazeFlags.PassageN => (corner,3),

		MazeFlags.PassageAll & ~MazeFlags.PassageW => (tJunction,0),
		MazeFlags.PassageAll & ~MazeFlags.PassageN => (tJunction,1),
		MazeFlags.PassageAll & ~MazeFlags.PassageE => (tJunction,2),
		MazeFlags.PassageAll & ~MazeFlags.PassageS => (tJunction,3),

		_ => (xJunction,0)
	};
}