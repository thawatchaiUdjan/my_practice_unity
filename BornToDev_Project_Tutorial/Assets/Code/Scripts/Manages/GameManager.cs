using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
	public bool isDebug;

	[Header("Game Objects")]
	[SerializeField] private PlayableDirector _openingScene;
	[SerializeField] private GameObject _partner;

	[Header("Game Conditions")]
	public float timeOut = 5f;
	public int highScore = 25;

	[Header("Game Progress")]
	public int score;

	//
	private Vector3 _spawnPos;

	//
	private SaveManager _save;
	private PlayerData _playerData;
	private LoadSceneManager _scene;
	private PlayerInputController _input;
	private EnemyManager _enemy;
	private TimerManager _timer;
	private UIManager _ui;
	private SoundManager _sound;

	//
	public delegate void OnScoreChange();
	public OnScoreChange onScoreChangeCallBack;

	//
	public static GameManager instance;
	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		_save = SaveManager.instance;
		_playerData = PlayerData.Instance;
		_scene = LoadSceneManager.instance;
		_input = PlayerInputController.instance;
		_enemy = EnemyManager.instance;
		_timer = TimerManager.instance;
		_ui = UIManager.instance;
		_sound = SoundManager.instance;
		
		_timer.onGameTimeOutCallBack += GameOver;
		_spawnPos = GameObject.FindWithTag("Respawn").transform.position;

		SetupGame();
	}

	private void Update()
	{
		if (_input.pause)
		{
			_input.pause = false;
			PauseGame();
		}
	}

	public void SetupGame()
	{
		SetupGamePlay();
		SetupData();
		CheckGameDebug();
	}

	private void SetupGamePlay()
	{
		_input.transform.position = _spawnPos;
		_input.IsCursorLock = false;
		_input.LockAllInput();
		_partner.SetActive(false);
		_ui.ShowGameUI(false);
	}

	private void SetupData()
	{
		// _save.LoadPlayerData();
	}

	private void CheckGameDebug()
	{
		if (isDebug) StartGame();
		else StartEnterGame();
	}

	private void StartEnterGame()
	{
		StartCoroutine(_scene.FadeSceneOut());
		_openingScene.Play();
	}

	private void StartGame()
	{
		_input.IsCursorLock = true;
		_input.ResetAllInput();
		_openingScene.Stop();
		_partner.SetActive(true);
		_enemy.SetupEnemy();
		_ui.ShowGameUI(true);
		_timer.StartTimer();
		_sound.OnPlayBGM(clip: _sound.BGM_Sand_1, volume: 0.5f);
	}

	public void GameOver()
	{
		_timer.onGameTimeOutCallBack -= GameOver;
		_input.IsCursorLock = false;
		_input.LockAllInput();
		_sound.OnPlayBGM(false);
		_ui.ShowGameOverUI(true, StarCalculate(score));
		SaveData();
	}

	private int StarCalculate(int score)
	{
		int star = 0;
		if (score >= 25) star = 3;
		else if (score >= 25 / 2) star = 2;
		else if (score >= 25 / 3) star = 1;
		return star;
	}

	public void AddScore(int score = 1)
	{
		this.score += score;
		onScoreChangeCallBack?.Invoke();
	}

	public void PauseGame()
	{
		Time.timeScale = 0;
		_input.LockAllInput();
		_input.IsCursorLock = false;
		_ui.ShowGamePauseUI(true);
	}

	public void ResumeGame()
	{
		Time.timeScale = 1;
		_input.ResetAllInput();
		_input.IsCursorLock = true;
		_ui.ShowGamePauseUI(false);
	}

	public void SaveData()
	{
		if (score > _playerData.score)
		{
			_playerData.score = score;
		}
		_save.SavePlayerData();
	}
}
