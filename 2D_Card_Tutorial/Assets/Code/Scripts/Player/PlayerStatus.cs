public class PlayerStatus : EntityStatus
{
	//
	private PlayerData _playerData;

	public override void Start()
	{
		base.Start();
		_playerData = PlayerData.Instance;
		_game.onSaveDataCallback += SaveData;
	}

	private void SaveData()
	{
		_playerData.attackUp = attackUp;
		_playerData.defenseUp = defenseUp;
		_playerData.healthUp = healthUp;
	}

	protected override void SetupBattleStatus()
	{
		attackUp = _playerData.attackUp;
		defenseUp = _playerData.defenseUp;
		healthUp = _playerData.healthUp;
		base.SetupBattleStatus();
		_ui.UpdateBattleStatusUI();
	}

	public void GetCardBattle(CardBattleStatus cardBattle)
	{
		switch (cardBattle.buffType)
		{
			case BuffType.Attack:
				attackUp += cardBattle.percentStatus;
				break;
			case BuffType.Defense:
				defenseUp += cardBattle.percentStatus;
				break;
			case BuffType.Health:
				healthUp += cardBattle.percentStatus;
				break;
		}
		_ui.UpdateBattleStatusUI();
	}

}
