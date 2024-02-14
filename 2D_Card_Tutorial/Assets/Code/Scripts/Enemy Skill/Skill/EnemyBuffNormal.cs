using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "EnemySkill/Buff/AttackNormal")]
public class EnemyBuffNormal : EnemySkill
{
    [Header("Specific Info")]
	[SerializeField] StatusType statusType;
	[SerializeField] private int statusUp = 30;
	[SerializeField] private int turnDuration = 2;
	
	public override void UseSkill()
	{
		base.UseSkill();
		_statuses.Add(new StatusInfo(_enemyStatus, this, statusType.buffType, statusUp, turnDuration, statusType));
		_enemyStatus.GetStatus(_statuses);
	}
}
