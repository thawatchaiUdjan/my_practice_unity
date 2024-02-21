using UnityEngine;

public class SoundManager : MonoBehaviour
{
	[Header("Sound Setting")]
	[SerializeField] private AudioSource _SFXSound;

	[Header("SFX Audio")]
	public AudioClip gunFire;

	//
	public static SoundManager instance;
	private void Awake()
	{
		instance = this;
	}

	public void OnPlaySFX(AudioClip audio, Vector3 pos, float volume = 1f) //On play sound one shot
	{
		volume *= _SFXSound.volume;
		AudioSource.PlayClipAtPoint(audio, pos, volume);
	}
}
