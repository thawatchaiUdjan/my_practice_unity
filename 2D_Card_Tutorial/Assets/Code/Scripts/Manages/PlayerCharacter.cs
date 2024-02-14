using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Character", menuName = "Character/Player")]
public class PlayerCharacter : Character
{
	[Header("Specific Setting")]
	public string unlockCondition;
	public GameObject showPrefab;
	public CardSkill ultimateCard;
	public List<CardSkill> cards;
}