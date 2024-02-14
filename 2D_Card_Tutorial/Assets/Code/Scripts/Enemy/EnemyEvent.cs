using System.Collections;
using UnityEngine;

public class EnemyEvent : EntityEvent
{
	//
	private DissolveEffect _dissolve;

	protected override void Start()
	{
		base.Start();
		_dissolve = GetComponent<DissolveEffect>();
	}

	public void OnDeadDone()
	{
		StartCoroutine(Dissolve());
	}

	private IEnumerator Dissolve()
	{
		yield return StartCoroutine(_dissolve.Dissolve());
		_entity.isDeadFinish = true;
	}

}
