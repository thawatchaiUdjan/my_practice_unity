using System;

[Serializable]
public class SettingsData
{
	public float masterVolume;
	public float bgmVolume;
	public float sfxVolume;

	//
	private float _defaultVolume = 50;

	//
	public delegate void OnChangeVolume();
	public OnChangeVolume onChangeVolume;

	//
	private static SettingsData _instance;
	public static SettingsData Instance
	{
		get
		{
			_instance ??= new SettingsData();
			return _instance;
		}
	}

	public SettingsData()
	{
		masterVolume = _defaultVolume;
		bgmVolume = _defaultVolume;
		sfxVolume = _defaultVolume;
	}

	public void SetSettingData(SettingsData data)
	{
		masterVolume = data.masterVolume;
		bgmVolume = data.bgmVolume;
		sfxVolume = data.sfxVolume;
		onChangeVolume?.Invoke();
	}

	public void ResetDefaultValue()
	{
		masterVolume = _defaultVolume;
		bgmVolume = _defaultVolume;
		sfxVolume = _defaultVolume;
		onChangeVolume?.Invoke();
	}

}
