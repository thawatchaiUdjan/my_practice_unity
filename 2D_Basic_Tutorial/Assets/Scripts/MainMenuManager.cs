using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
	[SerializeField] private GameObject _continueButton;
	private PlayerData _playerData;
	private SettingsManager _setting;
	private LoadSceneManager _scene;
	private SaveManager _save;
	void Start()
	{
		_playerData = PlayerData.instance;
		_setting = SettingsManager.instance;
		_scene = LoadSceneManager.instance;
		_save = SaveManager.instance;
		CheckContinueData();
	}

	private void CheckContinueData()
	{
		if (_save.CheckPlayerSaveData()) _continueButton.SetActive(true);
	}

	public void OnClickNewGame()
	{
		_save.NewGameData();
		_scene.LoadScene(_playerData.scene);
	}

	public void OnClickContinue()
	{
		_save.LoadData();
		_scene.LoadScene(_playerData.scene, isTravel: true);
	}

	public void OnClickSetting()
	{
		_setting.ShowSettings();
	}

	public void OnClickQuit()
	{
		Application.Quit();
	}

}
