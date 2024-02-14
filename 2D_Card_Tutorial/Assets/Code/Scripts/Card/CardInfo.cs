using System;
using UnityEngine;

[Serializable]
public class CardInfo
{
	public CardSkill card;
	public int level;

	public CardInfo(CardSkill card, int level)
	{
		this.card = card;
		this.level = level;
	}

	public CardInfo(CardInfo cardInfo)
	{
		card = cardInfo.card;
		level = cardInfo.level;
	}
}
