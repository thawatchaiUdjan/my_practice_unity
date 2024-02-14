using UnityEngine;

public class PlayerEvent : EntityEvent
{
	//AnimID
	private string _animVictory = "Victory";

	protected override void Start()
	{
		base.Start();
	}

	public void OnVictory(bool isVictory)
	{
		_animator.SetBool(_animVictory, isVictory);
	}

	public void OnDeadDone()
	{
		_entity.isDeadFinish = true;
	}

}
