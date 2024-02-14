using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
	[Header("Sound Settings")]
	[SerializeField] private AudioMixer _mixer;
	[SerializeField] private AudioSource _SFXSound;
	[SerializeField] private AudioSource _BGMSound;
	[SerializeField] private float _fadeSpeed = 0.5f;

	[Header("BGM Audio")]
	public AudioClip BGM_MainMenu;
	public AudioClip BGM_Boss_Battle;
	public AudioClip BGM_Battle_1;

	[Header("SFX Audio")]
	public AudioClip cardSwap;
	public AudioClip cardPress;
	public AudioClip cardReturn;
	public AudioClip statusDown;
	public AudioClip entityHit;
	public AudioClip entityEvade;
	public AudioClip entitySpawn;
	public AudioClip battleStart;
	public AudioClip battleBossStart;
	public AudioClip battleWin;
	public AudioClip battleLose;

	//
	private readonly float _multiply = 0.2f;
	private float _targetVolume;
	private float _oldVolume;
	private bool _isPause;
	private SettingData _setting;
	private SaveManager _save;

	//Mixer Parameter String
	private string _masterVolume = "MasterVolume";
	private string _bgmVolume = "BGMVolume";
	private string _sfxVolume = "SFXVolume";

	//
	public static SoundManager instance;
	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		_setting = SettingData.Instance;
		_save = SaveManager.instance;
		SetVolume();
	}

	private void Update()
	{
		PauseBGM();
	}

	private void PauseBGM()
	{
		if (_isPause)
		{
			_BGMSound.volume = Mathf.MoveTowards(_BGMSound.volume, _targetVolume, _fadeSpeed * Time.deltaTime);
			if (_BGMSound.volume == _targetVolume)
			{
				_isPause = false;
				if (_targetVolume == 0) _BGMSound.Pause();
			}
		}
	}

	public void SetVolume()
	{
		_mixer.SetFloat(_masterVolume, CalSetVolume(_setting.masterVolume));
		_mixer.SetFloat(_bgmVolume, CalSetVolume(_setting.bgmVolume));
		_mixer.SetFloat(_sfxVolume, CalSetVolume(_setting.sfxVolume));
	}

	public void OnPlayBGM(bool isPlay, AudioClip audio = null, float volume = 1f)
	{
		volume *= _multiply;
		_BGMSound.clip = audio;
		_BGMSound.volume = volume;
		if (isPlay & audio != null)
		{
			_BGMSound.Play();
		}
		else
		{
			_BGMSound.Stop();
		}
	}

	public void OnPauseBGM(bool isPause)
	{
		if (isPause)
		{
			_oldVolume = _BGMSound.volume;
			_targetVolume = 0f;
		}
		else
		{
			_BGMSound.UnPause();
			_targetVolume = _oldVolume;
		}
		_isPause = true;
	}

	public void OnPlaySFX(AudioClip audio, float volume = 1f)
	{
		volume *= _multiply;
		if (audio != null)
		{
			_SFXSound.PlayOneShot(audio, volume);
		}
	}

	private float CalSetVolume(float volume)
	{
		//Min = -45db, Max = 20db, BaseVolume = 0-100f (float Unit), BaseMiddle = 50f (float Unit)
		var tmpVolume = (volume - 50f) * 0.4f;
		if (volume < 50f) tmpVolume *= 2.25f;
		return tmpVolume;
	}

}
