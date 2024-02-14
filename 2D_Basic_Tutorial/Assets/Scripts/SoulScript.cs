using System.Collections;
using UnityEngine;

public class SoulScript : MonoBehaviour
{
	public int soulValue;
	public float soulMoveSpeed = 20f;
	public float delayInteract = 2f;
	public bool isInteract = false;
	
	//
	private Transform _player;

	private IEnumerator Start()
	{
		_player = GameObject.FindWithTag("Player").transform;
		yield return new WaitForSeconds(delayInteract);
		GetComponent<Rigidbody2D>().simulated = false;
		isInteract = true;
	}

	void Update()
	{
		if (isInteract)
		{
			MoveSoul();
			IsSoulCollected();
		}
	}

	private void MoveSoul()
	{
		transform.position = Vector2.MoveTowards(transform.position, _player.position, 
		soulMoveSpeed * Time.deltaTime);
	}

	private void IsSoulCollected()
	{
		if (Vector2.Distance(transform.position, _player.position) <= 0.25f)
		{
			_player.GetComponent<SoulManager>().GetSoul(soulValue);
			Destroy(gameObject);
		}
	}

	public void SetSoul(int value)
	{
		soulValue = value;
	}

}

