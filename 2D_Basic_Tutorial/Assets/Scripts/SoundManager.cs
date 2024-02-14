using UnityEngine;

public class SoundManager : MonoBehaviour
{
	[Range(0, 1)] public float audioVolume = 0.3f;

	[Header("General Sound")]
	public AudioClip footsteps;
	public AudioClip potionPickup;
	public AudioClip potionUsing;
	public AudioClip potionUseDone;
	public AudioClip restorePlayer;
	public AudioClip gaveStonePickup;
	public AudioClip soulGet;
	public AudioClip openChest;
	public AudioClip warpTravel;

	[Header("Combat Sound")]
	public AudioClip[] swordDeflect;
	public AudioClip[] swordSwipe;
	public AudioClip swordBlock;
	public AudioClip playerDamaged;
	public AudioClip playerDead;
	public AudioClip EnemyDamaged;
	public AudioClip EnemyDead;
	public AudioClip BossDefeated;
	public AudioClip bossFightBackground_1;

	//
	private Transform _player;
	private AudioSource _sound;

	//
	public static SoundManager instance;
	private void Awake()
	{
		instance = this;
	}
	private void Start()
	{
		_player = GameObject.FindWithTag("Player").transform;
		_sound = GetComponent<AudioSource>();

		SettingsData.OnDataChange += UpdateSound;
		UpdateSound();
	}

	private void UpdateSound()
	{
		audioVolume = SettingsData.instance.masterVolume;
		_sound.volume = audioVolume;
	}

	public void PlaySound(AudioClip clip, float volume = 0f)
	{
		if (clip == null) return;
		_sound.clip = clip;
		_sound.volume = volume == 0f ? audioVolume : volume;
		_sound.Play();
	}

	public void StopSound()
	{
		_sound.Stop();
		_sound.volume = audioVolume;
		_sound.clip = null;
	}

	public void OnPotionPickup()
	{
		AudioSource.PlayClipAtPoint(potionPickup, _player.position, audioVolume);
	}

	public void OnPotionUseDone()
	{
		AudioSource.PlayClipAtPoint(potionUseDone, _player.position, audioVolume * 4f);
	}

	public void OnSoulGet()
	{
		AudioSource.PlayClipAtPoint(soulGet, _player.position, audioVolume * 1.5f);
	}

	public void OnGraveStonePickup()
	{
		AudioSource.PlayClipAtPoint(gaveStonePickup, _player.position, audioVolume);
	}

	public void OnRestore()
	{
		AudioSource.PlayClipAtPoint(restorePlayer, _player.position, audioVolume);
	}

	public void OnWarpTravel()
	{
		AudioSource.PlayClipAtPoint(warpTravel, _player.position, audioVolume * 0.5f);
	}

	public void OnOpenChest()
	{
		AudioSource.PlayClipAtPoint(openChest, _player.position, audioVolume * 3f);
	}

	public void OnPlayerDmg()
	{
		AudioSource.PlayClipAtPoint(playerDamaged, _player.position, audioVolume * 0.3f);
	}

	public void OnPlayerDead()
	{
		AudioSource.PlayClipAtPoint(playerDead, _player.position, audioVolume * 2f);
	}

	public void OnBlock()
	{
		AudioSource.PlayClipAtPoint(swordBlock, _player.position, audioVolume * 2.5f);
	}

	public void OnPerfectBlock(int index)
	{
		if (index >= swordDeflect.Length) index = swordDeflect.Length - 1;
		AudioSource.PlayClipAtPoint(swordDeflect[index], _player.position, audioVolume * 3f);
	}

	public void OnEnemyDead(){
		AudioSource.PlayClipAtPoint(EnemyDead, _player.position, audioVolume);
	}

	public void OnBossDefeated(){
		AudioSource.PlayClipAtPoint(BossDefeated, _player.position, audioVolume * 2.5f);
	}

}
