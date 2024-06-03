using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Handles the player gameobject in the maze
public class Player : MonoBehaviour
{
	//Used to manipulate gameobject's movement
	public Rigidbody rb;

	//Player's position
    private Vector3 pos;

	//Float that determines player's speed
    [SerializeField, Min(0f)]
	float speed = 4f;

	//Used to pause the player in place when the game needs to be paused
	private int localTimeScale;

	//Sets gameobject to inactive whenever awoken
    void Awake ()
	{
		gameObject.SetActive(false);
	}

	//Sets gameobject to active, moves it's position to the corresponding starting position in the maze, and sets its timescale to 1 whenever a new level is started
    public void StartNewGame (Vector3 position)
	{
		gameObject.SetActive(true);
		transform.localPosition = position;
		localTimeScale = 1;
	}

	//Handles player movement based on rigidbody velocity
    public Vector3 Move ()
	{
		pos.x = Input.GetAxisRaw("Horizontal");
		pos.z = Input.GetAxisRaw("Vertical");

		pos.Normalize();

		rb.velocity = pos * speed * localTimeScale;

		return rb.velocity;
	}

	//Returns player's current position
	public Vector3 Position ()
	{
		return gameObject.transform.position;
	}

	//Sets gameobject to inactive whenever the game ends
	public void EndGame ()
	{
		gameObject.SetActive(false);
	}

	//Pauses player (thus disabling player movement) when the game is paused
	public void InBattle ()
	{
		localTimeScale = 0;
	}

	//Unpauses player (thus reenabling player movement) when the game is unpaused
	public void OutBattle ()
	{
		localTimeScale = 1;
	}
}
