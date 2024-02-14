using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	[SerializeField] private Transform _players;

	//
	public PlayerController Player
	{
		get => _player;
	}
	private PlayerController _player;
	private PlayerData _playerData;
	private SaveManager _save;

	//
	public static PlayerManager instance;
	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		_playerData = PlayerData.Instance;
		_save = SaveManager.instance;
	}

	public void SetupData()
	{
		Start();
		_player = SpawnPlayer().GetComponent<PlayerController>();
		_player.SetupData(_playerData.character);
	}

	private GameObject SpawnPlayer()
	{
		MakeDataDebug();
		return Instantiate(_playerData.character.prefab, _players);
	}

	private void MakeDataDebug()
	{
		if (_playerData.character == null) //For Make Data Debug//
		{
			_playerData.character = _playerData.characters[0];
			_save.SavePlayerData();
		}
	}
}
