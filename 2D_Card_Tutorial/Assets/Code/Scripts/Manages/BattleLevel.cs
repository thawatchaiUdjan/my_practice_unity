using System;
using UnityEngine;

[Serializable]
public class BattleLevel
{
	public DifficultType difficult;
	public int battleLevel;

	public BattleLevel(DifficultType difficult, int battleLevel)
	{
		this.difficult = difficult;
		this.battleLevel = battleLevel;
	}
}
