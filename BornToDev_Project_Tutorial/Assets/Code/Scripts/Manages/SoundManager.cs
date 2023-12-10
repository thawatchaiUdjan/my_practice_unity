using UnityEngine;
using UnityEngine.Audio;

public enum volumeType { Master, BGM, SFX }

public class SoundManager : MonoBehaviour
{
	[SerializeField] private AudioMixer _audioMixer;
	[SerializeField] private AudioSource _audioBGM;
	[SerializeField] private AudioSource _audioSFX;

	[Header("BGM Audio Clips")]
	public AudioClip BGM_MainMenu;
	public AudioClip BGM_Sand_1;

	[Header("SFX Audio Clips")]
	public AudioClip[] playerFootStep;
	public AudioClip playerShoot;
	public AudioClip shootImpact;
	public AudioClip enemyDead;

	[Header("SFX UI Audio Clips")]
	public AudioClip buttonClick;
	public AudioClip gameOverOpen;
	public AudioClip starGetting;


	//Private members
	private float _multiply = 0.2f;

	//Sound Mixer Parameters
	private string _soundMaster = "VolumeMaster";
	private string _soundBGM = "VolumeBGM";
	private string _soundSFX = "VolumeSFX";

	//
	private SettingsData _settingData;

	//
	public static SoundManager instance;
	private void Awake()
	{
		instance = this;
	}

	void Start()
	{
		_settingData = SettingsData.Instance;
	}

	public void OnPlayBGM(bool isPlay = true, AudioClip clip = null, float volume = 1f)
	{
		volume *= _multiply;
		_audioBGM.volume = volume;
		_audioBGM.clip = clip != null ? clip : BGM_Sand_1;
		if (isPlay) _audioBGM.Play();
		else _audioBGM.Stop();
	}

	public void OnPlaySFX(AudioClip clip, float volume = 1f)
	{
		volume *= _multiply;
		if (clip != null)
		{
			_audioSFX.PlayOneShot(clip, volume);
		}
	}

	public void SetVolume()
	{
		_audioMixer.SetFloat(_soundMaster, CalSetVolume(_settingData.masterVolume));
		_audioMixer.SetFloat(_soundBGM, CalSetVolume(_settingData.bgmVolume));
		_audioMixer.SetFloat(_soundSFX, CalSetVolume(_settingData.sfxVolume));
	}

	private float CalSetVolume(float volume)
	{
		//Min = -45db, Max = 20db
		var tmpVolume = (volume - 50f) * 0.4f;
		if (volume < 50f) tmpVolume *= 2.25f;
		return tmpVolume;
	}
}
