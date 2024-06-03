using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

//Handles the boss gameobject within the maze
public class Boss : MonoBehaviour
{
	//Sets the color of the object
    [SerializeField]
	Color color = Color.white;

	//References player gameobject
	[SerializeField]
	Player player;

	//References game gameobject
	[SerializeField]
	Game game;

	//References battlesystem gameobject
	[SerializeField]
	BattleSystem battle;

	//References cameratracking gameobject
	[SerializeField]
	CameraTracking cameratracking;

	//Sets the color of the gameobject and sets the gameobject to inactive initially whenever awoken
	void Awake ()
	{
		GetComponent<MeshRenderer>().material.color = color;
		gameObject.SetActive(false);
	}

	//Used to move boss to correct position in maze then turn active whenever new level needs to start
	public void StartNewGame (Vector3 position)
	{
		transform.localPosition = position;
		gameObject.SetActive(true);
		transform.eulerAngles = new Vector3(0,180,0);
	}

	//Gets boss' current position, which is always the same
    public Vector3 Position ()
	{
		return gameObject.transform.position;
	}

	//Used to turn gameobject off when game ends
	public void EndGame ()
	{
		gameObject.SetActive(false);
	}

	//Used to turn gameobject off when player enters battle with it
	public void InBattle ()
	{
		gameObject.SetActive(false);
	}

	//Despite EndGame and InBattle doing the same thing, they are there for clarity of sequences of events elsewhere
	//If the player dies before reaching the boss, the boss gameobject should still be removed, and since the player didn't enter combat with the boss, EndGame is used

	//Update called once per frame, checking if player's current position is close enough to boss' position
	//If so, initiate battle with boss
	public void Update ()
	{
		player.Position();
		Position();
		Vector3 playerPosition = player.Position();
		Vector3 bossPosition = Position();
		if (
			new Vector3 (
				bossPosition.x - playerPosition.x,
				0,
				bossPosition.z - playerPosition.z
			).sqrMagnitude < 0.1f
		)
		{
			//Initiates battle script, sets enemy ID to corresponding boss, and turns off maze camera for the battle
			InBattle();
			int ID = this.game.floorNumber + 2;
			this.battle.enemyID = ID;
			this.battle.StartBattle();
			this.player.InBattle();
			this.cameratracking.Off();
			return;
		}
	}
}