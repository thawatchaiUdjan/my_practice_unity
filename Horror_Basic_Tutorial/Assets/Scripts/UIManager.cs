using System.Collections;
using System.Collections.Generic;
using PlayerController;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;

public class UIManager : MonoBehaviour
{
	[Header("UI")]
	public GameObject gameUI;
	public GameObject gameMenu;
	public GameObject gamePause;
	public GameObject gameClearScene;
	public GameObject gameHintMouse;
	public GameObject gameHintPressBtn;
	public GameObject gameReadNoteScene;
	public GameObject gameActionScene;
	public TextMeshProUGUI textNote;
	public TextMeshProUGUI textCloseNote;
	public TextMeshProUGUI texHintPressBtnText;
	public TextMeshProUGUI texHintPressBtnSuffix;
	public TextMeshProUGUI textRemainingTime;
	public TextMeshProUGUI textHeaderMenu;
	public TextMeshProUGUI textClearGame;

	[Header("Text Assets")]
	[SerializeField] private TextAsset hintTurnOnLight;

	//String Input Actions
	private string _strFlashLight = "FlashLight";
	private string _strInteract = "Interact";

	//Anim ID
	private string _animActionFadeIn = "ActionSceneFadeIn";
	private string _animActionFadeOut = "ActionSceneFadeOut";

	//
	private GameManager _game;
	private PlayerInputControl _input;
	private LoadSceneManager _scene;
	private PlayerInput _inputAction;

	//
	public static UIManager instance;
	private void Awake()
	{
		instance = this;
	}

	void Start()
	{
		_game = GameManager.instance;
		_input = PlayerInputControl.instance;
		_scene = LoadSceneManager.instance;
		_inputAction = _input.GetComponent<PlayerInput>();
		SetupTextKey();
	}

	public void SetupTextKey()
	{
		texHintPressBtnText.text = _inputAction.actions[_strFlashLight].GetBindingDisplayString();
		textCloseNote.text = _inputAction.actions[_strInteract].GetBindingDisplayString();
	}

	public void SetGameMenu(string txtHeader, string txtClear)
	{
		gameMenu.SetActive(true);
		textHeaderMenu.text = txtHeader;
		textClearGame.text = txtClear;
	}

	public void SetRemainingTime(string text)
	{
		textRemainingTime.gameObject.SetActive(true);
		textRemainingTime.text = text;
	}

	public void ShowReadNote(string text)
	{
		HideGameUI();
		gameReadNoteScene.SetActive(true);
		textNote.text = text;
		BlurBgOnOff(true);
	}

	public void HideReadNote()
	{
		ShowGameUI();
		gameReadNoteScene.SetActive(false);
		BlurBgOnOff(false);
	}

	public void BlurBgOnOff(bool isOn)
	{
		var profile = Camera.main.GetComponent<PostProcessVolume>().profile;
		profile.GetSetting<DepthOfField>().active = isOn;
	}

	public IEnumerator ShowHints(GameObject obj)
	{
		obj.SetActive(true);
		obj.TryGetComponent(out Animation objAnim);
		if (objAnim != null) yield return new WaitUntil(() => !objAnim.isPlaying);
		obj.SetActive(false);
	}

	public void ShowHintMouse()
	{
		StartCoroutine(ShowHints(gameHintMouse));
	}

	public void ShowHintOpenLight()
	{
		texHintPressBtnSuffix.text = hintTurnOnLight.text;
		StartCoroutine(ShowHints(gameHintPressBtn));
	}

	public void HideGameUI()
	{
		gameUI.SetActive(false);
	}

	public void ShowGameUI()
	{
		gameUI.SetActive(true);
	}

	public IEnumerator ActionSceneFadeIn()
	{
		gameActionScene.SetActive(true);
		gameActionScene.TryGetComponent(out Animation anim);
		anim.Play(_animActionFadeIn);
		yield return new WaitUntil(() => !anim.isPlaying);
	}

	public IEnumerator ActionSceneFadeOut()
	{
		gameActionScene.TryGetComponent(out Animation anim);
		anim.Play(_animActionFadeOut);
		yield return new WaitUntil(() => !anim.isPlaying);
		gameActionScene.SetActive(false);
	}

	public void GameClearFade()
	{
		gameClearScene.SetActive(true);
	}

	public void LoadSceneOnClick(string sceneName)
	{
		if (!_game.isDebug)
		{
			_scene.LoadSceneOnClick(sceneName);
		}
	}
}
