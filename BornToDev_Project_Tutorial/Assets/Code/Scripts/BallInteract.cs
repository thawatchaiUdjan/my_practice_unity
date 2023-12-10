using UnityEngine;

public class BallInteract : MonoBehaviour
{
	public int damage = 30;

	[Header("Sound Setting")]
	[Range(0f,1f)] public float shootImpactVolume = 0.2f;

	//
	private SoundManager _sound;

	// private ParticleSystem _particle;
	// private List<ParticleCollisionEvent> _collisionEvents;

	void Start()
	{
		// _particle = GetComponent<ParticleSystem>();
		_sound = SoundManager.instance;
	}

	private void OnParticleCollision(GameObject target)
	{
		_sound.OnPlaySFX(_sound.shootImpact, shootImpactVolume);
		if (target.TryGetComponent(out Enemy enemy))
		{
			enemy.TakeDamage(damage);
		}
	}
}
