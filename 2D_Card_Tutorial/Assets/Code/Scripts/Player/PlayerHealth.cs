using UnityEngine;

public class PlayerHealth : EntityHealth
{
	private PlayerData _playerData;

	protected override void Start()
	{
		if (_playerData != null) return;
		base.Start();
		_playerData = PlayerData.Instance;
		_game.onSaveDataCallback += SaveData;
	}

	private void SaveData()
	{
		_playerData.currHealth = health;
	}

	public override void SetupHealth()
	{
		Start();
		health = _playerData.currHealth == 0 ? maxHealth : _playerData.currHealth;
		base.SetupHealth();
	}

}
