using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "EnemySkill/Heal/HealNormal")]
public class EnemyHealNormal : EnemySkill
{
    [Header("Specific Info")]
	[SerializeField] private int heal = 15;
	
	public override void UseSkill()
	{
		base.UseSkill();
		_enemyStatus.GetHeal(heal);
	}
}
