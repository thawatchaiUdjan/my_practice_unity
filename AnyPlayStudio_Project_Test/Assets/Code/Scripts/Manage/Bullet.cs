using UnityEngine;

public class Bullet : MonoBehaviour
{
	[SerializeField] private int _damage = 5;

	private void OnParticleCollision(GameObject target)
	{
		if (target.TryGetComponent(out PlayerHealth player))
		{
			player.TakeDamage(_damage);
		}
	}
}
