using UnityEngine;

public class StatusInfo
{
	public EntityStatus target;
	public Skill skill;
	public SkillType skillType;
	public BuffType buffType;
	public int percentStatus;
	public int turn;
	public StatusType statusType;

	public StatusInfo
		(
			EntityStatus target,
			Skill skill,
			SkillType skillType,
			BuffType buffType,
			int percentStatus,
			int turn
		)
	{
		this.target = target;
		this.skill = skill;
		this.skillType = skillType;
		this.buffType = buffType;
		this.percentStatus = percentStatus;
		this.turn = turn;
	}

	public StatusInfo
	(
		EntityStatus target,
		Skill skill,
		BuffType buffType,
		int percentStatus,
		int turn,
		StatusType statusType
	)
	{
		this.target = target;
		this.skill = skill;
		this.buffType = buffType;
		this.percentStatus = percentStatus;
		this.turn = turn;
		this.statusType = statusType;
		skillType = skill.skillType;
	}

	public StatusInfo
	(
		BuffType buffType,
		int percentStatus
	)
	{
		skillType = SkillType.None;
		this.buffType = buffType;
		this.percentStatus = percentStatus;
	}
}
