using System.Collections.Generic;
using UnityEngine;

public enum SkillType { Attack, Heal, Buff, Debuff, Ultimate, None }
public enum BuffType { None, Attack, Defense, Health, Speed }
public enum ActionType { Normal, Melee }

public class Skill : ScriptableObject
{
	public SkillType skillType;
	public ActionType actionType;
	public new string name = "Skill Name";

	//
	protected EnemyManager _enemyManager;
	protected PlayerManager _playerManager;

	protected PlayerStatus _playerStatus;
	protected EnemyStatus _enemyStatus;

	//
	protected List<StatusInfo> _statuses;

	protected void CheckEntity()
	{
		SetupStatus();

		if (_enemyManager == null | _playerManager == null)
		{
			SetupComponents();
		}
	}

	private void SetupComponents()
	{
		_playerManager = PlayerManager.instance;
		_enemyManager = EnemyManager.instance;

		_playerStatus = _playerManager.Player.GetComponent<PlayerStatus>();
		_enemyStatus = _enemyManager.Enemy.GetComponent<EnemyStatus>();
	}

	private void SetupStatus()
	{
		_statuses = new List<StatusInfo>();
	}
}
