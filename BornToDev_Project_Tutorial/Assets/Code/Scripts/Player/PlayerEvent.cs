using UnityEngine;

public class PlayerEvent : MonoBehaviour
{
	[Header("Sound Setting")]
	[Range(0f,1f)] public float footstepVolume = 0.2f;
	[Range(0f,1f)] public float ShootVolume = 0.5f;

	//
	private SoundManager _sound;

	void Start()
	{
		_sound = SoundManager.instance;
	}

	public void OnFootStep()
	{
		var index = Random.Range(0, _sound.playerFootStep.Length);
		_sound.OnPlaySFX(_sound.playerFootStep[index], footstepVolume);
	}

	public void OnShooting()
	{
		_sound.OnPlaySFX(_sound.playerShoot, ShootVolume);
	}

}
