using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PlayerData
{
	[Header("Account Data")]
	public List<PlayerCharacter> characters;
	public List<BattleLevel> battleLevels;

	[Header("Battle Data")]
	public PlayerCharacter character;
	public DifficultType difficult;
	public int battleLevel;
	public int currHealth;
	public int attackUp;
	public int defenseUp;
	public int healthUp;
	public int ultimatePoint;
	public bool isUltimate;
	public bool isUltimateGet;
	public List<CardInfo> cards;

	//
	private static PlayerData _instance;
	public static PlayerData Instance
	{
		get
		{
			_instance ??= new PlayerData();
			return _instance;
		}
	}

	public void CreateNewData(List<PlayerCharacter> defaultCharacters)
	{
		ResetAccountData(defaultCharacters);
		ResetBattleData();
	}

	private void ResetAccountData(List<PlayerCharacter> defaultCharacters)
	{
		characters = new List<PlayerCharacter>(defaultCharacters);
		battleLevels = new List<BattleLevel>()
		{
			new BattleLevel(DifficultType.Easy, 0),
			new BattleLevel(DifficultType.Normal, 0),
			new BattleLevel(DifficultType.Hard, 0),
		};
	}

	public void NewGame(PlayerCharacter character, DifficultType difficult)
	{
		ResetBattleData();
		this.character = character;
		this.difficult = difficult;
	}

	public void ResetBattleData()
	{
		difficult = 0;
		character = null;
		battleLevel = 1;
		currHealth = 0;
		attackUp = 0;
		defenseUp = 0;
		healthUp = 0;
		ultimatePoint = 0;
		isUltimate = false;
		isUltimateGet = false;
		cards = new List<CardInfo>();
	}

	public void SetPlayerData(PlayerData data)
	{
		characters = data.characters;
		battleLevels = data.battleLevels;
		difficult = data.difficult;
		character = data.character;
		battleLevel = data.battleLevel;
		currHealth = data.currHealth;
		attackUp = data.attackUp;
		defenseUp = data.defenseUp;
		healthUp = data.healthUp;
		ultimatePoint = data.ultimatePoint;
		isUltimate = data.isUltimate;
		isUltimateGet = data.isUltimateGet;
		cards = data.cards;
	}

	public BattleLevel GetBattleLevel(DifficultType difficult)
	{
		var battle = battleLevels.Where((value) => value.difficult == difficult);
		return battle.First();
	}

	public bool CheckNewCharacter(PlayerCharacter targetCharacter)
	{
		var character = characters.Where((character) => character == targetCharacter).ToList();
		return character.Count == 0;
	}

}
