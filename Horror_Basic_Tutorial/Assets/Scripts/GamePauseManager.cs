using System.Collections;
using System.Collections.Generic;
using PlayerController;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseManager : MonoBehaviour
{
	[SerializeField] private GameObject _gamePauseMenu;
	[SerializeField] private GameObject _gameSettingMenu;
	[SerializeField] private GameObject _resetDefaultConfirm;

	[Header("Mouse Setting")]
	[SerializeField] private Slider _mouseSlider;
	[SerializeField] private TextMeshProUGUI _mouseValueText;

	private PlayerInputControl _input;
	private FirstPersonController _player;
	private GameSettingManager _gameSettings;

	//
	public static GamePauseManager instance;
    private void Awake() {
		instance = this;
	}

    void Start()
    {
        _input = PlayerInputControl.instance;
		_player = FirstPersonController.instance;
		_gameSettings = GameSettingManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        IsPauseGame();
    }

	public void IsPauseGame()
	{
		if (_input.escape){
			_input.escape = false;

			if(_resetDefaultConfirm.activeSelf){
				_resetDefaultConfirm.SetActive(false);
				return;
			}

			if(_gameSettingMenu.activeSelf){
				_gameSettingMenu.SetActive(false);
				_gamePauseMenu.SetActive(true);
				return;
			}

			if (Time.timeScale == 1)
			{
				PauseGame();
				_gamePauseMenu.SetActive(true);
			}
			else ResumeGame();
		}
	}

	public void PauseGame(){
		Time.timeScale = 0;
		_input.LockAll(escapable: true, isCursorQuit: true);
	}

	public void ResumeGame(){
		Time.timeScale = 1;
		_input.ResetLockAll();
		_gamePauseMenu.SetActive(false);
	}

	public void SetMouseSensUI(){
		_mouseSlider.value = _gameSettings._mouseSens * 10f;
		_mouseValueText.text = _gameSettings._mouseSens.ToString("0.0");
	}

	public void ApplyChange(){
		_player.RotationSpeed = _gameSettings._mouseSens;
	}

	public void ApplyOnClick(){
		_gameSettings._mouseSens = _mouseSlider.value/10f;
		ApplyChange();
	}

	public void ResetDefaultOnClick(){
		_gameSettings._mouseSens = _gameSettings._defaultMouseSens;
		SetMouseSensUI();
		ApplyChange();
	}


	public void OnChangeMouse(float value){
		_mouseValueText.text = (value/10f).ToString("0.0");
	}

}
