using System.Collections.Generic;
using UnityEngine;

//Handles object pooling, making maze cell prefabs reusable, optimizing memory usage
public class MazeCellObject : MonoBehaviour
{
//Only compiles if in unity editor
#if UNITY_EDITOR
	static List<Stack<MazeCellObject>> pools;

	//This method is run before the scene is loaded, which initializes pools if null, or clears it if it exists
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	static void ClearPools ()
	{
		if (pools == null)
		{
			pools = new();
		}
		else
		{
			for (int i = 0; i < pools.Count; i++)
			{
				pools[i].Clear();
			}
		}
	}
#endif

	[System.NonSerialized]
	System.Collections.Generic.Stack<MazeCellObject> pool;

	//This method gets an instance of a maze cell component; if reusable for the next maze level, use it again; else make a new instance and add it to the pool
	public MazeCellObject GetInstance ()
	{
		if (pool == null)
		{
			pool = new();
#if UNITY_EDITOR
			pools.Add(pool);
#endif
		}
		if (pool.TryPop(out MazeCellObject instance))
		{
			instance.gameObject.SetActive(true);
		}
		else
		{
			instance = Instantiate(this);
			instance.pool = pool;
		}
		return instance;
	}

	//This method pushes maze cell components back into the pool and deactivates them when not in use
	public void Recycle ()
	{
		pool.Push(this);
		gameObject.SetActive(false);
	}
}