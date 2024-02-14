using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
	[SerializeField] private bool isLoadSave = true;
	[SerializeField] private bool isLocalSave = false;

	//
	private string savePath;
	private string path;
	private string localPath;
	private string playerDir;
	private string settingsDir;
	private string playerFileName = "PlayerData.json";
	private string settingsFileName = "SettingsData.json";

	//
	private PlayerData _playerData;
	private SettingsData _settingsData;

	//
	public static SaveManager instance;
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else Destroy(gameObject);
		SetPath();
		SetDir();
	}

	private void Start()
	{
		_playerData = PlayerData.instance;
		_settingsData = SettingsData.instance;
		SaveSettingsData();
	}

	private void Update()
	{
		SetDir();
	}

	private void SetDir()
	{
		savePath = isLocalSave ? localPath : path;
		playerDir = savePath + playerFileName;
		settingsDir = savePath + settingsFileName;
	}

	private void SetPath()
	{
		//Assets unity Path
		path = Application.dataPath + Path.DirectorySeparatorChar;
		//Local computer Path
		localPath = Application.persistentDataPath + Path.DirectorySeparatorChar;
	}

	public void SaveData()
	{
		if (_playerData == null) Start();
		var jsonStr = JsonUtility.ToJson(_playerData, true);
		using StreamWriter writer = new StreamWriter(playerDir);
		writer.Write(jsonStr);
		writer.Close();
	}

	public void LoadData()
	{
		var playerData = new PlayerData();
		if (isLoadSave)
		{
			if (!CheckPlayerSaveData()) NewGameData();
			using StreamReader reader = new StreamReader(playerDir);
			var json = reader.ReadToEnd();
			playerData = JsonUtility.FromJson<PlayerData>(json);

			reader.Close();
		}

		_playerData.SetPlayerData(playerData);
	}

	public void NewGameData()
	{
		var playerData = new PlayerData();
		if (isLoadSave)
		{
			playerData = new PlayerData(newGame: true);
		}
		_playerData.SetPlayerData(playerData);
		SaveData();
	}

	public void SaveSettingsData()
	{
		if (_settingsData == null) Start();
		var jsonStr = JsonUtility.ToJson(_settingsData, true);
		using StreamWriter writer = new StreamWriter(settingsDir);
		writer.Write(jsonStr);
		writer.Close();
	}

	public void LoadSettingsData()
	{
		if (!File.Exists(settingsDir)) SaveSettingsData();
		using StreamReader reader = new StreamReader(settingsDir);
		var json = reader.ReadToEnd();
		var settingsData = JsonUtility.FromJson<SettingsData>(json);
		reader.Close();


		_settingsData.SetSettingData(settingsData);
	}

	public SceneData NewSceneData()
	{
		_playerData.scenes.Add(new SceneData());
		SaveData();
		return _playerData.FindSceneData();
	}

	public void LoadSceneData()
	{
		var scene = _playerData.FindSceneData();
		scene ??= NewSceneData();

		//Setup object in game
		SetupBoss(scene);
		SetupChest(scene);
		SetupSavePoint(scene);
	}

	private void SetupBoss(SceneData scene)
	{
		var boss = FindObjectOfType<BossEnemy>(true);
		if (boss != null)
		{
			if (!scene.bossDead) boss.SetupBoss();
			else boss.gameObject.SetActive(false);
		}
	}

	private void SetupChest(SceneData scene)
	{
		var chest = FindObjectOfType<ChestInteract>(true);
		if (chest != null) chest.gameObject.SetActive(!scene.chestOpened);
	}

	private void SetupSavePoint(SceneData scene)
	{
		var savePoint = FindObjectOfType<SavePointInteract>(true);
		if (savePoint != null) savePoint.IsSavePointOpen(scene.savePointOpened);
	}

	public bool CheckPlayerSaveData()
	{
		return File.Exists(playerDir);
	}

}
