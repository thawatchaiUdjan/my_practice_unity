using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
	[SerializeField] private GameObject _settingMenu;

	[Header("Sound Settings")]
	[SerializeField] private Slider _masterSound;
	[SerializeField] private Slider _BGMSound;
	[SerializeField] private Slider _SFXSound;

	//
	private SettingsData _settingData;
	private SaveManager _save;
	private SoundManager _sound;

	//
	public static SettingManager instance;
	private void Awake()
	{
		instance = this;
	}

	void Start()
	{
		_settingData = SettingsData.Instance;
		_save = SaveManager.instance;
		_sound = SoundManager.instance;

		_save.LoadSettingData();
		ApplySetting();
	}

	public void OnClickApply()
	{
		SaveSettings();
		_save.SaveSettingData();
		ApplySetting();
	}

	public void OnClickResetDefault()
	{
		_settingData.ResetDefaultValue();
		_save.SaveSettingData();
		ApplySetting();
		UpdateUI();
	}

	private void SaveSettings()
	{
		_settingData.masterVolume = _masterSound.value;
		_settingData.bgmVolume = _BGMSound.value;
		_settingData.sfxVolume = _SFXSound.value;
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

	public void ShowSetting()
	{
		_settingMenu.SetActive(true);
		UpdateUI();
	}

}
