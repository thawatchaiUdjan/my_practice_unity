using UnityEngine;

public class EnemyHealth : EntityHealth
{
	protected override void Start()
	{
		base.Start();
	}

	public override void SetupHealth()
	{
		health = maxHealth;
		base.SetupHealth();
	}

}
