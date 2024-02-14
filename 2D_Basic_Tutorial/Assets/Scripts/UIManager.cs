using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField] private GameObject _gameUI;
	[SerializeField] private GameObject _deadAndReviveScreen;
	[SerializeField] private GameObject _restartMenu;
	[SerializeField] private GameObject _spawnPointMenu;
	[SerializeField] private GameObject _travelMenu;
	[SerializeField] private GameObject _travelSlotButton;
	[SerializeField] private Button PauseButton;

	//
	private PlayerData _playerDate;
	private LoadSceneManager _scene;
	private PlayerInputControl _input;
	private SettingsManager _setting;
	private SoundManager _sound;

	//String Texts
	private string _deadText = "You Dead";
	private string _reviveText = "Revived";
	private string _defeatText = "Defeated";

	//AnimID
	private string _animReviveScreen = "ReviveScreenFade";
	private string _animDeadScreen = "DeadScreenFade";

	//
	public static UIManager instance;
	private void Awake()
	{
		instance = this;
	}

	void Start()
	{
		_playerDate = PlayerData.instance;
		_scene = LoadSceneManager.instance;
		_input = PlayerInputControl.instance;
		_setting = SettingsManager.instance;
		_sound = SoundManager.instance;
	}

	public IEnumerator SetDeadAndReviveScreen(string textPopup)
	{
		var anim = _deadAndReviveScreen.GetComponent<Animation>();
		_deadAndReviveScreen.GetComponentInChildren<TextMeshProUGUI>().text = textPopup;
		_deadAndReviveScreen.SetActive(true);

		if (textPopup == _reviveText | textPopup == _defeatText)
		{
			anim.Play(_animReviveScreen);
			yield return new WaitUntil(() => !anim.isPlaying);
			_deadAndReviveScreen.SetActive(false);
		}
		else
		{
			anim.Play(_animDeadScreen);
			yield return new WaitForSeconds(3f);
			_restartMenu.SetActive(true);
		}
	}

	public void ShowDeadScreen()
	{
		StartCoroutine(SetDeadAndReviveScreen(_deadText));
	}

	public void ShowReviveScreen()
	{
		StartCoroutine(SetDeadAndReviveScreen(_reviveText));
	}

	public void ShowDefeatScreen()
	{
		StartCoroutine(SetDeadAndReviveScreen(_defeatText));
	}

	public void OnClickLoadScene(int sceneIndex)
	{
		Time.timeScale = 1;
		var travel = false;
		if (sceneIndex == 99)
		{
			var scene = _playerDate.FindSavePointNear();
			sceneIndex = scene.sceneIndex;
			travel = scene.savePointOpened;
		}
		_scene.LoadScene(sceneIndex, travel);
	}
	public void ShowGameUI()
	{
		_gameUI.SetActive(true);
	}

	public void HideGameUI()
	{
		_gameUI.SetActive(false);
	}

	public void OnClickPauseMenu()
	{
		_input.LockAllInput();
		Time.timeScale = 0;
	}
	public void OnClickCloseMenu()
	{
		_input.ResetLockAllInput();
		Time.timeScale = 1;
	}

	public void OnClickSetting()
	{
		_setting.ShowSettings();
	}

	public void ShowSpwPointMenu()
	{
		PauseDisable();
		_spawnPointMenu.SetActive(true);
	}

	public void PauseDisable()
	{
		PauseButton.enabled = false;
	}

	public void ShowTravelMenu()
	{
		var contents = _travelMenu.GetComponentInChildren<ContentSizeFitter>().transform;
		RemoveChildren(contents);
		foreach (var scene in _playerDate.scenes)
		{
			if (!scene.savePointOpened) continue;
			var slot = Instantiate(_travelSlotButton, contents).GetComponent<Button>();
			var text = slot.GetComponentInChildren<TextMeshProUGUI>().text = scene.sceneName;
			if (scene.sceneIndex == SceneManager.GetActiveScene().buildIndex)
			{
				slot.interactable = false;
				continue;
			}
			slot.onClick.AddListener(() => StartCoroutine(OnClickTravel(scene)));
		}
		_travelMenu.SetActive(true);
	}

	public void RemoveChildren(Transform parent)
	{
		if (parent.childCount <= 0) return;
		foreach (Transform child in parent)
		{
			Destroy(child.gameObject);
		}
	}

	private IEnumerator OnClickTravel(SceneData scene)
	{
		PauseDisable();
		_travelMenu.SetActive(false);
		_input.LockAllInput();
		_sound.OnWarpTravel();
		yield return StartCoroutine(_input.GetComponent<DissolveEffect>().Dissolve());
		_scene.LoadScene(scene.sceneIndex, isTravel: true);
	}


}
