using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
	[Header("Sound Settings")]
	[SerializeField] private Slider _masterSound;
	[SerializeField] private Slider _BGMSound;
	[SerializeField] private Slider _SFXSound;

	//
	private SettingData _settingData;
	private SaveManager _save;
	private SoundManager _sound;

	private void Start()
	{
		_settingData = SettingData.Instance;
		_save = SaveManager.instance;
		_sound = SoundManager.instance;
	}

	private void OnEnable()
	{
		if (_settingData == null) Start();
		_save.LoadSettingData();
		UpdateUI();
	}

	public void OnClickApply()
	{
		SaveSetting();
		ApplySetting();
	}

	public void OnClickResetDefault()
	{
		_settingData.ResetData();
		_save.SaveSettingData();
		ApplySetting();
		UpdateUI();
	}

	private void SaveSetting()
	{
		_settingData.masterVolume = _masterSound.value;
		_settingData.bgmVolume = _BGMSound.value;
		_settingData.sfxVolume = _SFXSound.value;
		_save.SaveSettingData();
	}

	private void UpdateUI()
	{
		_masterSound.value = _settingData.masterVolume;
		_BGMSound.value = _settingData.bgmVolume;
		_SFXSound.value = _settingData.sfxVolume;
	}

	private void ApplySetting()
	{
		_sound.SetVolume();
	}
}
