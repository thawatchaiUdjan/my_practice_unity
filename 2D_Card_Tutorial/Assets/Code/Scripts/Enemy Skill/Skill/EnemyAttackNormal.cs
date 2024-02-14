using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "EnemySkill/Attack/AttackNormal")]
public class EnemyAttackNormal : EnemySkill
{
	[Header("Specific Info")]
	[SerializeField] private int damage = 100;
	[SerializeField] private int hits = 1;
	[SerializeField] private bool evadeAble = true;

	public override void UseSkill()
	{
		base.UseSkill();
		_enemyManager.Enemy.AttackDamage(_playerManager.Player, damage, hits: hits, evadeAble: evadeAble);
	}
}
