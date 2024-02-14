using UnityEngine;

public class EnemyStatus : EntityStatus
{
	private EnemyManager _enemyManager;

	public override void Start()
	{
		base.Start();
		_enemyManager = EnemyManager.instance;
	}

	protected override void SetupBattleStatus()
	{
		CalculateBattleStatus();
		base.SetupBattleStatus();
	}

	private void CalculateBattleStatus()
	{
		var statusUp = _game.battleLevel * _enemyManager.statusMultiply;
		attackUp += statusUp;
		defenseUp += statusUp;
		healthUp += statusUp;
	}

}
