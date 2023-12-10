using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
	//Private Members
	private string _settingPath;
	private string _playerPath;

	//
	private string _playerFileName = "PlayerData.json";
	private string _settingsFileName = "SettingsData.json";

	//
	private SettingsData _settingData;
	private PlayerData _playerData;

	//
	public static SaveManager instance;
	private void Awake()
	{
		instance = this;
	}
	private void Start()
	{
		if (_settingData != null) return;
		_settingData = SettingsData.Instance;
		_playerData = PlayerData.Instance;

		SetPath();
	}

	public void SaveSettingData()
	{
		var jsonStr = JsonUtility.ToJson(_settingData, true);
		using StreamWriter writer = new StreamWriter(_settingPath);
		writer.Write(jsonStr);
		writer.Close();
	}

	public void LoadSettingData()
	{
		if (_settingData == null) Start();
		if (!CheckSave(_settingPath)) SaveSettingData(); 
		using StreamReader reader = new StreamReader(_settingPath);
		var jsonStr = reader.ReadToEnd();
		reader.Close();
		var setting = JsonUtility.FromJson<SettingsData>(jsonStr);
		_settingData.SetSettingData(setting);
	}

	public void SavePlayerData()
	{
		var jsonStr = JsonUtility.ToJson(_playerData, true);
		using StreamWriter writer = new StreamWriter(_playerPath);
		writer.Write(jsonStr);
		writer.Close();
	}

	public void LoadPlayerData()
	{
		if (_playerData == null) Start();
		if (!CheckSave(_playerPath)) SavePlayerData(); 
		using StreamReader reader = new StreamReader(_playerPath);
		var jsonStr = reader.ReadToEnd();
		reader.Close();
		var player = JsonUtility.FromJson<PlayerData>(jsonStr);
		_playerData.SetPlayerData(player);
	}

	private void SetPath()
	{
		//Assets unity Path
		var path = Application.dataPath + Path.DirectorySeparatorChar;
		//Local computer Path
		var localPath = Application.persistentDataPath + Path.DirectorySeparatorChar;

		_settingPath = path + _settingsFileName;
		_playerPath = path + _playerFileName;
	}

	private bool CheckSave(string pathFile)
	{
		return File.Exists(pathFile);
	}

}
