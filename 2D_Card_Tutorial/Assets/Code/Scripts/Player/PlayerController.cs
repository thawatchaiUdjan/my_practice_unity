using System;
using System.Collections;
using UnityEngine;

public class PlayerController : Entity
{
	[Header("Ultimate Setting")]
	[SerializeField] private Transform _ultimateBar;
	[SerializeField] private int _ultimateLimit;
	public bool isUltimate;
	public bool isUltimateGet;

	//
	private PlayerData _playerData;
	private EnemyManager _enemyManager;
	private PlayerEvent _playerEvent;
	private UIManager _ui;
	private CardInfo _cardInfo;
	private CardManager _card;


	protected override void Start()
	{
		base.Start();
		_playerData = PlayerData.Instance;
		_enemyManager = EnemyManager.instance;
		_card = CardManager.instance;
		_ui = UIManager.instance;
		_playerEvent = GetComponent<PlayerEvent>();
		_game.onSaveDataCallback += SaveData;
	}

	private void SaveData()
	{
		_playerData.ultimatePoint = _ultimateBar.childCount;
		_playerData.isUltimate = isUltimate;
		_playerData.isUltimateGet = isUltimateGet;
	}

	public override void SetupData(Character character)
	{
		base.SetupData(character);
		isUltimate = _playerData.isUltimate;
		isUltimateGet = _playerData.isUltimateGet;
		if (_ultimateBar.childCount != _playerData.ultimatePoint)
		{
			GetUltimatePoint(_playerData.ultimatePoint);
		}
	}

	public void StartTurn()
	{
		if (_card == null) Start();
		StartCoroutine(CheckDead());
	}

	protected override void StartAction()
	{
		base.StartAction();
		_card.GettingCard();
	}

	public void PlayerAction(CardInfo cardInfo) //Each Action of Player Card Skill
	{
		_cardInfo = cardInfo;
		_animName = cardInfo.card.animation.name;
		isAction = true;
		CheckUltimateSkillUse(true);
		CheckActionSkill(_enemyManager.Enemy.transform, _cardInfo.card.actionType);
	}

	protected override void ActionDone()
	{
		CheckUltimateUse();
		base.ActionDone();
	}

	private void CheckUltimateSkillUse(bool isOn)
	{
		if (_cardInfo.card.skillType == SkillType.Ultimate) _game.OnUseUltimateSkill(isOn);
	}

	private void CheckUltimateUse()
	{
		if (_cardInfo.card.skillType == SkillType.Ultimate)
		{
			ResetUltimate();
			CheckUltimateSkillUse(false);
		}
		else if (!isUltimate & _cardInfo.level == 3)
		{
			GetUltimatePoint();
		}
	}

	private void GetUltimatePoint(int count = 1)
	{
		for (int i = 0; i < count; i++)
		{
			Instantiate(_ui.ultimateIcon, _ultimateBar);
		}
		if (_ultimateBar.childCount >= _ultimateLimit)
		{
			isUltimate = true;
		}
	}

	private void ResetUltimate()
	{
		for (int i = 0; i < _ultimateBar.childCount; i++)
		{
			var ultimateIcon = _ultimateBar.GetChild(i);
			Destroy(ultimateIcon.gameObject);
		}
		isUltimateGet = false;
		isUltimate = false;
	}

	public void CancelUseUltimate()
	{
		isUltimateGet = false;
	}

	protected override void DeadDone()
	{
		base.DeadDone();
		_game.GameOver();
	}

	public void OnVictory(bool isVictory)
	{
		_playerEvent.OnVictory(isVictory);
	}

	private void OnActiveSkill()
	{
		_cardInfo.card.UseCard(_cardInfo.level);
	}
}
