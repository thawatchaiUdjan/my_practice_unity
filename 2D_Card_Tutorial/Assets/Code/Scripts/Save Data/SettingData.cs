using UnityEngine;

public class SettingData
{
	public float masterVolume;
	public float bgmVolume;
	public float sfxVolume;

	private float _defaultVolume = 50f;
	//
	private static SettingData _instance;
	public static SettingData Instance
	{
		get
		{
			_instance ??= new SettingData();
			return _instance;
		}
	}

	public SettingData()
	{
		ResetData();
	}

	public void SetSettingData(SettingData data)
	{
		masterVolume = data.masterVolume;
		bgmVolume = data.bgmVolume;
		sfxVolume = data.sfxVolume;
	}

	public void ResetData()
	{
		masterVolume = _defaultVolume;
		bgmVolume = _defaultVolume;
		sfxVolume = _defaultVolume;
	}
}
