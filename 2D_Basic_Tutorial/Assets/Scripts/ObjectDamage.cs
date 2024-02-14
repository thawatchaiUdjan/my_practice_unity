using UnityEngine;

public class ObjectDamage : MonoBehaviour
{
	public float damage = 100f;
	public bool blockAble = true; 

	private void OnTriggerEnter2D(Collider2D target)
	{
		if (target.gameObject.CompareTag("Player"))
		{
			if (target.gameObject.TryGetComponent(out HealthManager health))
			{
				health.TakeDamage(transform, damage, blockAble);
			}
		}
	}

}
