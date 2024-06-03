using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

using static Unity.Mathematics.math;

//Handles maze data, accessing and modifying maze cells, and conversion between different coordinate systems (coordinates, index, world position)
public struct Maze
{
	//Size of maze, x and y
	int2 size;

	//Native array representing maze cells, and is safe to access from parallel jobs without restrictions
	[NativeDisableParallelForRestriction]
	NativeArray<MazeFlags> cells;

	//Allows accessing individual cells within index
	public MazeFlags this[int index]
	{
		get => cells[index];
		set => cells[index] = value;
	}

	//Total number of cells in maze
	public int Length => cells.Length;

	//Following two ints return size of maze in east-west and north-south directions (x and y)
	public int SizeEW => size.x;

	public int SizeNS => size.y;

	//Following four ints return step size of each cardinal direction
	public int StepN => size.x;

	public int StepE => 1;

	public int StepS => -size.x;

	public int StepW => -1;

	//Initialize maze th specified size and allocates memory for cells within
	public Maze (int2 size)
	{
		this.size = size;
		cells = new NativeArray<MazeFlags>(size.x * size.y, Allocator.Persistent);
	}

	//Disposes cells
	public void Dispose ()
	{
		if (cells.IsCreated)
		{
			cells.Dispose();
		}
	}

	//Sets flags for a cell at a given index
	public MazeFlags Set (int index, MazeFlags mask) =>
		cells[index] = cells[index].With(mask);

	//Unsets flags for a cell at a given index
	public MazeFlags Unset (int index, MazeFlags mask) =>
		cells[index] = cells[index].Without(mask);

	//Converts 1D index to 2D coordinates
	public int2 IndexToCoordinates (int index)
	{
		int2 coordinates;
		coordinates.y = index / size.x;
		coordinates.x = index - size.x * coordinates.y;
		return coordinates;
	}

	//Converts 2D coordinates to 3D world position vector
	public Vector3 CoordinatesToWorldPosition (int2 coordinates, float y = 0f) =>
		new Vector3(
			2f * coordinates.x + 1f - size.x,
			y,
			2f * coordinates.y + 1f - size.y
		);

	//Converts 1D index to 3D world position vector
	public Vector3 IndexToWorldPosition (int index, float y = 0f) =>
		CoordinatesToWorldPosition(IndexToCoordinates(index), y);

	//Converts 2D coordinates to 1D index
	public int CoordinatesToIndex (int2 coordinates) =>
		coordinates.y * size.x + coordinates.x;

	//Converts 3D world position vector to 2D coordinates
	public int2 WorldPositionToCoordinates (Vector3 position) => int2(
		(int)((position.x + size.x) * 0.5f),
		(int)((position.z + size.y) * 0.5f)
	);

	//Converts 3D worldposition vector to 1D index
	public int WorldPositionToIndex (Vector3 position) =>
		CoordinatesToIndex(WorldPositionToCoordinates(position));
}