using System.Collections;
using TMPro;
using UnityEngine;

public enum TurnType { Player, Enemy }
public enum DifficultType { Easy, Normal, Hard }

public class GameManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _battleLevelText;
	[SerializeField] private TextMeshProUGUI _gameOverText;

	[Header("Game Setting")]
	public int battleLevel = 1;
	public int limitCard = 7;
	public float delayTurn = 0.8f;
	public int bossLevel = 10;
	public bool isLoadCard = true;
	public bool isGameStart;
	public bool isBossBattle;

	[Header("Enemy Status Multiply")]
	public int easyStatus = 1;
	public int normalStatus = 2;
	public int hardStatus = 4;

	//
	private TurnType _turn = TurnType.Player;
	private CardManager _cardManager;
	private PlayerManager _playerManager;
	private EnemyManager _enemyManager;
	private NewUnlockManager _unlockManager;
	private MapManager _map;
	private PlayerData _playerData;
	private SaveManager _save;
	private UIManager _ui;
	private SoundManager _sound;

	//
	public delegate void OnSaveDataCallback();
	public OnSaveDataCallback onSaveDataCallback;
	public delegate void OnStatusUpdateCallback();
	public OnStatusUpdateCallback onStatusDecreaseCallback;
	public delegate void OnActionDoneCallback();
	public OnActionDoneCallback onActionDoneCallback;

	//
	public static GameManager instance;
	private void Awake()
	{
		instance = this;
	}

	private IEnumerator Start()
	{
		_cardManager = CardManager.instance;
		_playerManager = PlayerManager.instance;
		_enemyManager = EnemyManager.instance;
		_unlockManager = NewUnlockManager.instance;
		_map = MapManager.instance;
		_playerData = PlayerData.Instance;
		_save = SaveManager.instance;
		_ui = UIManager.instance;
		_sound = SoundManager.instance;
		onSaveDataCallback += SaveData;

		//
		SetupGame();

		//
		yield return new WaitForSeconds(1f);
		StartTurn();
	}

	private void SaveData()
	{
		_playerData.battleLevel = battleLevel;
		SaveHighestBattleLevel();
	}

	private void SaveHighestBattleLevel()
	{
		var battle = _playerData.GetBattleLevel(_playerData.difficult);
		if (battleLevel > battle.battleLevel)
		{
			battle.battleLevel = battleLevel - 1;
		}
	}

	public void SavePlayerData()
	{
		onSaveDataCallback?.Invoke();
		_save.SavePlayerData();
	}

	private void SetupGame()
	{
		_ui.ShowBackgroundBlock(true);
		SetupData();
	}

	private void SetupData()
	{
		SetupBattle();
		_playerManager.SetupData();
		_enemyManager.SetupData();
		_cardManager.SetupCard();
	}

	private void SetupBattle()
	{
		isGameStart = true;
		SetupBattleLevel();
		SetupSound();
	}

	private void SetupSound()
	{
		if (isBossBattle)
		{
			_map.BossMap();
			_sound.OnPlayBGM(true, _sound.BGM_Boss_Battle, 4.5f);
		}
		else
		{
			_map.NormalMap();
			_sound.OnPlayBGM(true, _sound.BGM_Battle_1, 2.5f);
		}
	}

	private void SetupBattleLevel()
	{
		battleLevel = _playerData.battleLevel;
		isBossBattle = battleLevel % bossLevel == 0;
		UpdateBattleLevelUI();
	}

	private void PlayerTurn()
	{
		onStatusDecreaseCallback?.Invoke();
		_turn = TurnType.Player;
		_playerManager.Player.StartTurn();
	}

	private void EnemyTurn()
	{
		_turn = TurnType.Enemy;
		_enemyManager.Enemy.StartTurn();
	}

	public void StartTurn()
	{
		switch (_turn)
		{
			case TurnType.Player:
				PlayerTurn();
				break;
			case TurnType.Enemy:
				EnemyTurn();
				break;
		}
	}

	public void NextTurn()
	{
		_turn = (_turn == TurnType.Player) ? TurnType.Enemy : TurnType.Player;
		StartCoroutine(DelayStartTurn());
	}

	private IEnumerator DelayStartTurn()
	{
		if (!_enemyManager.Enemy.isDead) yield return new WaitForSeconds(delayTurn);
		StartTurn();
	}

	public void GameClear()
	{
		battleLevel++;
		_unlockManager.UnlockDarkWitchCharacter();
		_playerManager.Player.OnVictory(true);
		_ui.ShowUI(_ui.gameClearUI, true);
		_ui.ShowBackgroundBlock(false);
		_sound.OnPlayBGM(false);
		_sound.OnPlaySFX(_sound.battleWin);
		Debug.Log("Game Clear");
	}

	public void GameOver()
	{
		_gameOverText.text = $"You can survive\n{battleLevel - 1} Stage\n({_playerData.difficult} Mode)";
		_playerData.ResetBattleData();
		_save.SavePlayerData();
		_ui.ShowUI(_ui.gameOverUI, true);
		_ui.ShowBackgroundBlock(false);
		_sound.OnPlayBGM(false);
		_sound.OnPlaySFX(_sound.battleLose, 2.5f);
		Debug.Log("Game Over");
	}

	public void CardPlay(Transform card, bool isMoveCard = false)
	{
		StartCoroutine(_cardManager.CardPlaySlot(card, isMoveCard));
	}

	public void CardReturn()
	{
		StartCoroutine(_cardManager.CardSaveReturn());
	}

	public void OnGamePause(bool isPause) //Game Pause
	{
		if (isPause)
		{
			Time.timeScale = 0;
		}
		else
		{
			Time.timeScale = 1;
		}
	}

	public void OnUseUltimateSkill(bool isUse)
	{
		_map.OnTurnLight(!isUse);
		_ui.ShowGameUI(!isUse);
		_sound.OnPauseBGM(isUse);
	}

	public void GameReset()
	{
		SetupBattle();
		_playerManager.Player.OnVictory(false);
		_playerManager.Player.ResetCharacter();
		_enemyManager.SetupData();
	}

	public void UpdateBattleLevelUI() //Update Battle Level UI
	{
		_battleLevelText.text = battleLevel.ToString();
		_battleLevelText.GetComponent<Animation>().Play();
	}

	public void OnClickGameContinue() //OnClick Continue Game 
	{
		GameReset();
		NextTurn();
	}

}
