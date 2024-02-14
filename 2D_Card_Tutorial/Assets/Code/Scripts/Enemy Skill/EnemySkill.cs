using UnityEngine;

public class EnemySkill : Skill
{
	public AnimationClip animation;

	public virtual void UseSkill()
	{
		CheckEntity();
	}
}
