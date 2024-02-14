using UnityEngine;

[CreateAssetMenu(fileName = "New card", menuName = "CardSkill/Attack/AttackNormal")]
public class AttackNormal : CardSkill
{
	[Header("Specific Info")]
	[SerializeField] private int damage = 100;
	[SerializeField] private int damageLv2 = 150;
	[SerializeField] private int damageLv3 = 250;
	[SerializeField] private int hits = 1;

	protected override void CardUseLv1()
	{
		base.CardUseLv1();
		_playerManager.Player.AttackDamage(_enemyManager.Enemy, damage, hits: hits);
	}
	protected override void CardUseLv2()
	{
		base.CardUseLv2();
		_playerManager.Player.AttackDamage(_enemyManager.Enemy, damageLv2, hits: hits);
	}

	protected override void CardUseLv3()
	{
		base.CardUseLv3();
		_playerManager.Player.AttackDamage(_enemyManager.Enemy, damageLv3, hits: hits);
	}

}
