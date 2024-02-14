using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityStatus : MonoBehaviour
{
	[Header("Status Setting")]
	[SerializeField] private Transform _statusBar;
	[SerializeField] private float _damageRange = 10f; //Percent

	[Header("Status Info")]
	public int attack;
	public int defense;
	public int speed;

	[Header("Base Status")]
	private int baseAttack;
	private int baseDefense;
	private int baseHealth;
	private int baseSpeed;

	[Header("Battle Status")]
	[HideInInspector] public int attackUp;
	[HideInInspector] public int defenseUp;
	[HideInInspector] public int healthUp;

	//
	private int _skillTypeMultiply;
	protected GameManager _game;
	protected UIManager _ui;
	private EntityHealth _health;
	private EntityEvent _event;

	public virtual void Start()
	{
		_game = GameManager.instance;
		_ui = UIManager.instance;
		_event = GetComponent<EntityEvent>();
		_health = GetComponent<EntityHealth>();
	}

	public void SetupStatus(Character character)
	{
		Start();
		SetupBaseStatus(character);
		SetupBattleStatus();
		RemoveStatus();
	}

	protected void SetupBaseStatus(Character character)
	{
		baseAttack = character.attack;
		baseDefense = character.defense;
		baseHealth = character.health;
		baseSpeed = character.speed;
		attack = baseAttack;
		defense = baseDefense;
		speed = baseSpeed;
		_health.maxHealth = baseHealth;
	}

	protected virtual void SetupBattleStatus()
	{
		var statuses = new List<StatusInfo>()
		{
			new StatusInfo(BuffType.Attack, attackUp),
			new StatusInfo(BuffType.Defense, defenseUp),
			new StatusInfo(BuffType.Health, healthUp),
		};
		GetStatus(statuses, false);
	}

	public void ResetStatus()
	{
		RemoveStatus();
		SetupBattleStatus();
	}

	public bool CalculateEvade()
	{
		var isEvade = Random.Range(0, 100) < speed;
		return isEvade;
	}

	public int CalculateDamageIn(int damage, float hits)
	{
		var damageIn = damage - defense;
		damageIn = Mathf.FloorToInt(damageIn / hits);
		return damageIn;
	}

	public int CalculateDamageOut(int percentDamage)
	{
		var damageOut = Mathf.RoundToInt(percentDamage / 100f * attack);
		var damageRange = Mathf.RoundToInt(_damageRange / 100f * damageOut);
		damageOut = Random.Range(damageOut - damageRange, damageOut + damageRange);
		return damageOut;
	}

	private int CalculateStatusUp(int percentStatus, int baseStatus, bool isShow)
	{
		var statusUp = Mathf.RoundToInt(percentStatus / 100f * baseStatus);
		statusUp *= _skillTypeMultiply;
		if (!isShow) statusUp *= -1;
		return statusUp;
	}

	public List<StatusIcon> GetCurrentStatus()
	{
		var statuses = _statusBar.GetComponentsInChildren<StatusIcon>().ToList();
		return statuses;
	}

	private bool CreateStatusIcon(StatusInfo statusInfo, bool isShow)
	{
		var isCreate = false;
		if (!isShow) return false;
		if (CheckReplaceStatus(statusInfo))
		{
			var icon = statusInfo.skillType == SkillType.Buff ? _ui.statusBuffIcon : _ui.statusDebuffIcon;
			var status = Instantiate(icon, _statusBar);
			status.transform.SetAsFirstSibling();
			status.GetComponent<StatusIcon>().SetupStatus(statusInfo);
			isCreate = true;
		}
		return isCreate;
	}

	private bool CheckReplaceStatus(StatusInfo targetStatus)
	{
		var statuses = GetCurrentStatus();
		var isReplace = true;
		if (statuses.Count <= 0) return isReplace;
		foreach (var status in statuses)
		{
			if (status.CheckSameStatus(targetStatus))
			{
				if (status.CheckReplaceStatus(targetStatus))
				{
					status.SetTurn(0); //Delete Buff
				}
				else
				{
					isReplace = false;
				}
				break;
			}
		}
		return isReplace;
	}

	private void CheckStatusType(StatusInfo status, bool isShow)
	{
		switch (status.skillType)
		{
			case SkillType.Buff:
				_skillTypeMultiply = 1;
				if (isShow) _event.OnBuff(status.statusType);
				break;
			case SkillType.Debuff:
				_skillTypeMultiply = -1;
				if (isShow) _event.OnDebuff(status.statusType);
				break;
			default:
				_skillTypeMultiply = -1;
				break;
		}
	}

	public void GetStatus(List<StatusInfo> statuses, bool isShow = true)
	{
		if (statuses == null) return;

		foreach (var status in statuses)
		{
			CheckStatusType(status, isShow);
			if (CreateStatusIcon(status, isShow) | !isShow)
			{
				switch (status.buffType)
				{
					case BuffType.Attack:
						GetAttack(status, isShow);
						break;
					case BuffType.Defense:
						GetDefense(status, isShow);
						break;
					case BuffType.Health:
						GetHealth(status, isShow);
						break;
				}
			}
		}
	}

	public void RemoveStatus(SkillType skillType = SkillType.None)
	{
		var statuses = GetCurrentStatus();
		if (statuses.Count <= 0) return;
		switch (skillType)
		{
			case SkillType.Buff:
				statuses = statuses.Where((status) => status.CheckSkillType() == SkillType.Buff).ToList();
				break;
			case SkillType.Debuff:
				statuses = statuses.Where((status) => status.CheckSkillType() == SkillType.Debuff).ToList();
				break;
		}
		if (statuses.Count <= 0) return;
		foreach (var status in statuses) status.Remove();
	}

	public void GetHeal(int percentHeal)
	{
		_health.GetHeal(percentHeal);
	}

	private void GetHealth(StatusInfo status, bool isShow)
	{
		var statusUp = CalculateStatusUp(status.percentStatus, baseHealth, isShow);
		_health.GetHealth(statusUp);
	}

	private void GetAttack(StatusInfo status, bool isShow)
	{
		var statusUp = CalculateStatusUp(status.percentStatus, baseAttack, isShow);
		attack += statusUp;
	}

	private void GetDefense(StatusInfo status, bool isShow)
	{
		var statusUp = CalculateStatusUp(status.percentStatus, baseDefense, isShow);
		defense += statusUp;
	}

}