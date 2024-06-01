using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
	public Rigidbody rb;
    private Vector3 pos;

    [SerializeField, Min(0f)]
	float speed = 4f;

	private int localTimeScale;

	Maze maze;

    void Awake ()
	{
		gameObject.SetActive(false);
	}

    public void StartNewGame (Vector3 position)
	{
		gameObject.SetActive(true);
		transform.localPosition = position;
		localTimeScale = 1;
	}
    public Vector3 Move ()
	{
		pos.x = Input.GetAxisRaw("Horizontal");
		pos.z = Input.GetAxisRaw("Vertical");

		pos.Normalize();

		rb.velocity = pos * speed * localTimeScale;

		return rb.velocity;
	}

	public Vector3 Position ()
	{
		return gameObject.transform.position;
	}

	public void EndGame () => gameObject.SetActive(false);

	public void InBattle ()
	{
		localTimeScale = 0;
	}

	public void OutBattle ()
	{
		localTimeScale = 1;
	}
}
