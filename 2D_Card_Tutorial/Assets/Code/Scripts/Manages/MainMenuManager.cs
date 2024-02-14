using System.Collections;
using TMPro;
using UnityEngine;

public class MainMenuManager : MonoBehaviour, IPlaySFXEvent
{
	[SerializeField] private GameObject _mainScreen;
	[SerializeField] private GameObject _selectCharacterUI;
	[SerializeField] private GameObject _enterBattleUI;
	[SerializeField] private GameObject _alertBox;

	[Header("Continue Setting")]
	[SerializeField] private GameObject _continueButton;
	[SerializeField] private TextMeshProUGUI _continueProcessText;


	//
	private DifficultType _difficultType;
	private PlayerData _playerData;
	private CharacterManager _character;
	private CameraManager _camera;
	private LoadSceneManager _scene;
	private SaveManager _save;
	private SoundManager _sound;

	//
	public static MainMenuManager instance;
	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		_playerData = PlayerData.Instance;
		_character = CharacterManager.instance;
		_camera = CameraManager.instance;
		_scene = LoadSceneManager.instance;
		_save = SaveManager.instance;
		_sound = SoundManager.instance;
		SetupData();
	}

	private void SetupData()
	{
		SetContinueButton();
		SetupSound();
	}

	private void SetupSound()
	{
		_sound.OnPlayBGM(true, _sound.BGM_MainMenu, 3f);
	}

	private void SetContinueButton()
	{
		var isContinue = _playerData.character != null;
		_continueButton.SetActive(isContinue);
		_continueProcessText.transform.parent.gameObject.SetActive(isContinue);
		_continueProcessText.text = $"{_playerData.difficult} Mode : STAGE {_playerData.battleLevel}";
	}

	public void ShowAlertBox(string text, float delay = 0)
	{
		var alert = _alertBox.GetComponent<AlertBox>();
		alert.Show(text, delay);
	}

	private IEnumerator EnterBattleScene(int scene, bool isZoomOut = false)
	{
		_mainScreen.SetActive(false);
		if (isZoomOut) yield return StartCoroutine(_camera.OnZoomOut());
		StartCoroutine(_character.GetCharacterSelect().EnterBattleScene());
		yield return new WaitForSeconds(1f);
		_enterBattleUI.SetActive(true);
		yield return new WaitUntil(() => !_enterBattleUI.GetComponent<Animation>().isPlaying);
		_scene.LoadScene(scene);
	}

	private IEnumerator ShowSelectCharacterMenu()
	{
		yield return StartCoroutine(_camera.OnZoomIn());
		_selectCharacterUI.SetActive(true);
	}

	public void OnClickNewGame(PlayerCharacter character)
	{
		_playerData.NewGame(character, _difficultType);
		_save.SavePlayerData();
		StartCoroutine(EnterBattleScene(1, isZoomOut: true));
	}

	public void OnClickContinue()
	{
		StartCoroutine(EnterBattleScene(1));
	}

	public void OnClickDifficult(int difficult)
	{
		_difficultType = (DifficultType)difficult;
		StartCoroutine(ShowSelectCharacterMenu());
	}

	public void OnClickQuitGame()
	{
		Application.Quit();
	}

	public void OnPlaySFX(AudioClip audio)
	{
		_sound.OnPlaySFX(audio);
	}
}
