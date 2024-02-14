using System.Collections;
using System.Collections.Generic;
using PlayerController;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[Header("Game Objects")]
	public GameObject firstObjective;

	[Header("Variables")]
	public bool isPreGameClear;
	public bool isCameraPickedUp;
	public bool isDebug;
	public int gameClearType;
	public float gamePlayTime;
	public float countDownTime = 5f;

	[Header("Text Assets")]
	[SerializeField] private TextAsset _onGameStart;

	//Private Class
	private UIManager _ui;
	private LoadSceneManager _scene;
	private GameSettingManager _settings;
	private FirstPersonController _player;
	private ObjectiveManager _objective;
	private GhostManager _ghost;
	private Animation _animation;
	private PlayerInputControl _input;
	private DialogueManager _dialogue;
	private GameEventManager _event;
	private SoundManager _sound;

	//Anim ID
	private string _animGameOver = "GameOver";
	private string _animGameClear = "GameClear";

	//
	public static GameManager instance;
	private void Awake()
	{
		instance = this;
	}
	void Start()
	{
		_ui = UIManager.instance;
		_scene = LoadSceneManager.instance;
		_settings = GameSettingManager.instance;
		_player = FirstPersonController.instance;
		_objective = ObjectiveManager.instance;
		_ghost = GhostManager.instance;
		_input = PlayerInputControl.instance;
		_dialogue = DialogueManager.instance;
		_event = GameEventManager.instance;
		_sound = SoundManager.instance;

		_animation = _player.GetComponent<Animation>();

		isDebug = _scene == null;
		StartCoroutine(GameStart()); //Game Start OnClick
	}

	void Update()
	{
		if (!isCameraPickedUp) _input.flashLightAble = false;
	}

	// =============================== Setup Field =====================================
	public IEnumerator GameStart()
	{
		GameSetup();
		if (!isDebug)
		{
			_input.LockAll();
			_settings.StartGameSettings();
			_scene.LoadSceneOut();
			yield return new WaitUntil(() => !_scene.IsPlayAnimate());
			_scene.HideSceneLoad();
			_input.ResetLockAll();
			yield return new WaitForSeconds(2);
			_dialogue.StartDialogue(_onGameStart.text);
		}
		else DebugMode();
		StartCoroutine(CountPlayTime());
	}

	public void GameSetup()
	{
		HealthManager.instance.SetupHealth();
		_objective.ReSetObjective();
		_ghost.ResetGhost();
		SetupPlayerPosition();
	}

	public void GameAfterCameraSetup()
	{
		BatteryManager.instance.SetupBattery();
		SetupFirstObjective();
		isCameraPickedUp = true;
		_ui.ShowGameUI();
	}

	public void DebugMode()
	{
		GameAfterCameraSetup();
	}

	public void SetupFirstObjective()
	{
		var firstObjectsOnMap = GameObject.FindGameObjectsWithTag("FirstObjective");
		var pos = Random.Range(0, firstObjectsOnMap.Length);
		var tf = firstObjectsOnMap[pos].transform;
		firstObjective.transform.position = tf.position;
		firstObjective.transform.rotation = tf.rotation;
		firstObjective.SetActive(true);
	}

	public void SetupPlayerPosition()
	{
		var respawn = GameObject.FindWithTag("Respawn").transform;
		_player.transform.position = respawn.position;
		_player.transform.rotation = respawn.rotation;
	}
	// =============================== Setup Field =====================================

	public IEnumerator LetsGameStart()
	{
		_sound.OnFirstObjectivePickup();
		_event.FontDoorOpen();
		yield return new WaitForSeconds(5);
		_objective.SetupObjective();
		_ghost.SetupGhost();
		firstObjective.SetActive(false);
	}

	public void PreGameClear()
	{
		isPreGameClear = true;
		_sound.StopSoundBg();
		_ghost.ResetGhost();
		StartCoroutine(CountdownTime(countDownTime));
		StartCoroutine(_event.PreEventGameClear());
	}

	public void GameClear()
	{
		StopAllCoroutines();
		_input.LockAll();
		_ui.HideGameUI();
		_animation.Play(_animGameClear + "_" + gameClearType);
	}

	public void GameOver()
	{
		StopAllCoroutines();
		_input.LockAll();
		_sound.StopSoundBg();
		_ui.HideGameUI();
		_ghost.StopGhost();
		_animation.Play(_animGameOver);
	}

	public IEnumerator CountdownTime(float time)
	{
		var remainTime = time * 60;

		while (remainTime > 0f)
		{
			yield return new WaitForSeconds(1);
			remainTime -= 1;
			var minute = Mathf.FloorToInt(remainTime / 60);
			var second = Mathf.FloorToInt(remainTime % 60);
			_ui.SetRemainingTime(string.Format("{0:0}:{1:00}", minute, second));
			_sound.OnCountdownTime();
		}

		GameOver(); //Remain time <= 0 sec.
	}

	public IEnumerator CountPlayTime()
	{
		while (true)
		{
			gamePlayTime += Time.deltaTime;
			yield return null;
		}
	}

}
