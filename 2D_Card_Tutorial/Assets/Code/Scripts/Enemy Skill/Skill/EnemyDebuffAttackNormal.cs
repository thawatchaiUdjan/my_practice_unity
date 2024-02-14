using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "EnemySkill/Debuff/DebuffAttackNormal")]
public class EnemyDebuffAttackNormal : EnemySkill
{
	[Header("Specific Info")]
	[SerializeField] private int damage = 60;
	[SerializeField] private int hits = 1;

	[Header("Status Down Info")]
	[SerializeField] StatusType statusType;
	[SerializeField] private int StatusDown = 50;
	[SerializeField] private int turnDuration = 2;
	
	public override void UseSkill()
	{
		base.UseSkill();
		_statuses.Add(new StatusInfo(_playerStatus, this, statusType.buffType, StatusDown, turnDuration, statusType));
		_enemyManager.Enemy.AttackDamage(_playerManager.Player, damage, _statuses, hits: hits);
	}
}
