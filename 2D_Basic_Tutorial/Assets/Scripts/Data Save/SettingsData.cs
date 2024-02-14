using System;

[Serializable]
public class SettingsData
{
	//Variable Key
	public float masterVolume;
	public string inputBinding;

	//Default Value
	private float defaultMasterVolume = 0.3f;

	private static SettingsData Instance;
	public static SettingsData instance
	{
		get
		{
			Instance ??= new SettingsData();
			return Instance;
		}
	}

	//
	public delegate void OnSettingsChange();
	public static event OnSettingsChange OnDataChange;

	public SettingsData()
	{
		masterVolume = defaultMasterVolume;
		inputBinding = "";
	}

	public void SetSettingData(SettingsData data)
	{
		masterVolume = data.masterVolume;
		inputBinding = data.inputBinding;
		OnDataChange?.Invoke();
	}

}
