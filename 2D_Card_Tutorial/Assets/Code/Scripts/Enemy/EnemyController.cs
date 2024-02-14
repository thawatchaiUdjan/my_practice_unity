using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Entity
{
	[Header("Enemy Setting")]
	[SerializeField] private int _ultimateLimit = 6;

	//
	private int _ultimatePoint;
	private List<EnemySkill> _skills;
	private EnemySkill _ultimateSkill;
	private EnemySkill _skill;
	private PlayerManager _playerManager;

	protected override void Start()
	{
		base.Start();
		_playerManager = PlayerManager.instance;
	}

	public override void SetupData(Character character)
	{
		base.SetupData(character);
		var enemy = character as EnemyCharacter;
		_skills = enemy.skills;
		_ultimateSkill = enemy.UltimateSkill;
	}

	public void StartTurn()
	{
		if (_playerManager == null) Start();
		StartCoroutine(CheckDead());
	}

	protected override void StartAction()
	{
		base.StartAction();
		EnemyAction();
	}

	private void EnemyAction() //Each Action of Enemy Skill
	{
		SelectSkill();
		CheckUltimateSkillUse(true);
		CheckActionSkill(_playerManager.Player.transform, _skill.actionType);
	}

	protected override void ActionDone()
	{
		base.ActionDone();
		GetUltimatePoint();
		CheckUltimateSkillUse(false);
		_game.NextTurn();
	}

	private void CheckUltimateSkillUse(bool isOn)
	{
		if (_skill.skillType == SkillType.Ultimate) _game.OnUseUltimateSkill(isOn);
	}

	private void SelectSkill()
	{
		EnemySkill skill;
		if (_ultimatePoint >= _ultimateLimit)
		{
			skill = _ultimateSkill;
			_ultimatePoint = -1;
		}
		else
		{
			skill = RandomSkill();
		}
		_skill = skill;
		_animName = _skill.animation.name;
	}

	private void GetUltimatePoint()
	{
		if (_ultimateSkill != null)
		{
			_ultimatePoint++;
		}
	}

	private EnemySkill RandomSkill()
	{
		var randomIndex = Random.Range(0, _skills.Count);
		return _skills[randomIndex];
	}

	protected override void DeadDone()
	{
		base.DeadDone();
		_game.GameClear();
		Destroy(gameObject);
	}

	private void OnActiveSkill()
	{
		_skill.UseSkill();
	}

}
