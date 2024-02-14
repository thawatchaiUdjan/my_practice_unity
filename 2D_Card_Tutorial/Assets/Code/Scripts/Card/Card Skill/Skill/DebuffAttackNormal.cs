using UnityEngine;

[CreateAssetMenu(fileName = "New card", menuName = "CardSkill/Debuff/DebuffAttackDefenseDown")]
public class DebuffAttackNormal : CardSkill
{
	[Header("Specific Info")]
	[SerializeField] private int damage = 60;
	[SerializeField] private int damageLv2 = 90;
	[SerializeField] private int damageLv3 = 150;
	[SerializeField] private int hits = 1;

	[Header("Status Down Info")]
	[SerializeField] StatusType statusType;
	[SerializeField] private int defenseDown = 20;
	[SerializeField] private int defenseDownLv2 = 30;
	[SerializeField] private int defenseDownLv3 = 50;
	[SerializeField] private int turnDuration = 2;
	
	protected override void CardUseLv1()
	{
		base.CardUseLv1();
		_statuses.Add(new StatusInfo(_enemyStatus, this, statusType.buffType, defenseDown, turnDuration, statusType));
		_playerManager.Player.AttackDamage(_enemyManager.Enemy, damage, _statuses, hits: hits);
	}
	protected override void CardUseLv2()
	{
		base.CardUseLv2();
		_statuses.Add(new StatusInfo(_enemyStatus, this, statusType.buffType, defenseDownLv2, turnDuration, statusType));
		_playerManager.Player.AttackDamage(_enemyManager.Enemy, damageLv2, _statuses, hits: hits);
	}

	protected override void CardUseLv3()
	{
		base.CardUseLv3();
		_statuses.Add(new StatusInfo(_enemyStatus, this, statusType.buffType, defenseDownLv3, turnDuration, statusType));
		_playerManager.Player.AttackDamage(_enemyManager.Enemy, damageLv3, _statuses, hits: hits);
	}

}
