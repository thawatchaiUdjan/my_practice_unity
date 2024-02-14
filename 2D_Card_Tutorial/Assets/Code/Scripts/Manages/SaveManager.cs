using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
	[Header("Default Player Data")]
	[SerializeField] private List<PlayerCharacter> _defaultCharacters;

	//Private Members
	[HideInInspector] public string settingPath;
	[HideInInspector] public string playerPath;

	//
	private string _playerFileName = "PlayerData.json";
	private string _settingFileName = "SettingData.json";

	//
	private SettingData _settingData;
	private PlayerData _playerData;

	//
	public static SaveManager instance;
	private void Awake()
	{
		instance = this;
		_settingData = SettingData.Instance;
		_playerData = PlayerData.Instance;
		SetPath();
		LoadData(); //Load Data from .json
	}

	private void Start()
	{

	}

	private void SetPath()
	{
		var gamePath = Application.dataPath + Path.DirectorySeparatorChar; //Assets unity Path
		var localPath = Application.persistentDataPath + Path.DirectorySeparatorChar; //Local Computer Path
		var path = gamePath; //Change Path

		settingPath = path + _settingFileName;
		playerPath = path + _playerFileName;
	}

	private void LoadData()
	{
		if (!CheckSave(settingPath)) SaveSettingData(isNewSave: true);
		if (!CheckSave(playerPath)) SavePlayerData(isNewSave: true);
		LoadSettingData();
		LoadPlayerData();
	}

	//===============================================================
	public void SaveSettingData(bool isNewSave = false)
	{
		if (isNewSave) _settingData.ResetData();
		var jsonStr = JsonUtility.ToJson(_settingData, true);
		using StreamWriter writer = new StreamWriter(settingPath);
		writer.Write(jsonStr);
		writer.Close();
	}

	public void LoadSettingData()
	{
		using StreamReader reader = new StreamReader(settingPath);
		var jsonStr = reader.ReadToEnd();
		reader.Close();
		var setting = JsonUtility.FromJson<SettingData>(jsonStr);
		_settingData.SetSettingData(setting);
	}

	//===============================================================
	public void SavePlayerData(bool isNewSave = false)
	{
		if (isNewSave) _playerData.CreateNewData(_defaultCharacters);
		var jsonStr = JsonUtility.ToJson(_playerData, true);
		using StreamWriter writer = new StreamWriter(playerPath);
		writer.Write(jsonStr);
		writer.Close();
	}

	public void LoadPlayerData()
	{
		using StreamReader reader = new StreamReader(playerPath);
		var jsonStr = reader.ReadToEnd();
		var player = JsonUtility.FromJson<PlayerData>(jsonStr);
		reader.Close();
		_playerData.SetPlayerData(player);
	}
	//===============================================================

	public bool CheckSave(string pathFile)
	{
		return File.Exists(pathFile);
	}
}
