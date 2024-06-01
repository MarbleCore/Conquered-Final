using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
	Color color = Color.white;

	[SerializeField]
	Player player;

	[SerializeField]
	Game game;

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

	public void StartNewGame (Vector3 position)
	{
		transform.localPosition = position;
		gameObject.SetActive(true);
	}

    public Vector3 Position ()
	{
		return gameObject.transform.position;
	}

	public void EndGame () => gameObject.SetActive(false);

	public void InBattle ()
	{
		gameObject.SetActive(false);
	}

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