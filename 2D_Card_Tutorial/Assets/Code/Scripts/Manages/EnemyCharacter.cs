using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Character", menuName = "Character/Enemy")]
public class EnemyCharacter : Character
{
	[Header("Specific Setting")]
	public EnemySkill UltimateSkill;
	public List<EnemySkill> skills;
}