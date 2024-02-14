using UnityEngine;

[CreateAssetMenu(fileName = "New Ultimate card", menuName = "CardSkill/Ultimate/Attack")]
public class UltimateAttack : CardSkill
{
	[Header("Specific Info")]
	[SerializeField] private int damage = 600;
	[SerializeField] private int hits = 1;
	[SerializeField] private bool evadeAble = false;

	protected override void CardUseLv1()
	{
		base.CardUseLv1();
		_playerManager.Player.AttackDamage(_enemyManager.Enemy, damage, hits: hits, evadeAble: evadeAble);
	}

}
