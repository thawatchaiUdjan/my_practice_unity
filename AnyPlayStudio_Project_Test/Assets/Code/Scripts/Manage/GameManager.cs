using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[Header("Game Controller")]
	public bool isDebug;

	[Header("Game Manager")]
	[SerializeField] private List<Transform> _spawnPoints;

	//
	private List<PlayerController> _players = new List<PlayerController>();

	//
	public static GameManager instance;
	private void Awake()
	{
		instance = this;
	}

	public void PlayerJoin(PlayerController player)
	{
		_players.Add(player); //Add player to curr game
		SetPosition(player);
	}

	private void SetPosition(PlayerController player) //Set Player Random position
	{
		var index = Random.Range(0, _spawnPoints.Count);
		var pos = _spawnPoints[index];
		player.transform.SetPositionAndRotation(pos.position, pos.rotation);
	}
}
