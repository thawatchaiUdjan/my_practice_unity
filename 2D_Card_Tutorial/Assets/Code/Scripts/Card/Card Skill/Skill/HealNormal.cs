using UnityEngine;

[CreateAssetMenu(fileName = "New card", menuName = "CardSkill/Heal/HealNormal")]
public class HealNormal : CardSkill
{
	[Header("Specific Info")]
	[SerializeField] private int heal = 3;
	[SerializeField] private int healLv2 = 6;
	[SerializeField] private int healLv3 = 9;

	protected override void CardUseLv1()
	{
		base.CardUseLv1();
		_playerStatus.GetHeal(heal);
	}
	protected override void CardUseLv2()
	{
		base.CardUseLv2();
		_playerStatus.GetHeal(healLv2);
	}

	protected override void CardUseLv3()
	{
		base.CardUseLv3();
		_playerStatus.GetHeal(healLv3);
	}

}
