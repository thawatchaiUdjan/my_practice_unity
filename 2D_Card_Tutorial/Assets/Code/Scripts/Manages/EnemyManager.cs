using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	[SerializeField] private Transform _enemies;
	[SerializeField] private EnemyCharacter _enemyBoss;
	[SerializeField] private List<EnemyCharacter> _enemyList;
	[HideInInspector] public int statusMultiply;

	//
	public EnemyController Enemy
	{
		get => _enemy;
	}
	private EnemyController _enemy;
	private EnemyCharacter _enemyCharacter;
	private GameManager _game;
	private PlayerData _playerData;
	private UIManager _ui;

	//
	public static EnemyManager instance;
	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		_game = GameManager.instance;
		_playerData = PlayerData.Instance;
		_ui = UIManager.instance;
	}

	public void SetupData()
	{
		Start();
		SetStatusMultiply();
		_enemy = SpawnEnemy().GetComponent<EnemyController>();
		_enemy.SetupData(_enemyCharacter);
	}

	private GameObject SpawnEnemy()
	{
		if (_game.isBossBattle)
		{
			_ui.ShowStartBossUI();
			_enemyCharacter = _enemyBoss;
		}
		else
		{
			_enemyCharacter = RandomEnemy();
		}
		return Instantiate(_enemyCharacter.prefab, _enemies);
	}

	private EnemyCharacter RandomEnemy()
	{
		var index = Random.Range(0, _enemyList.Count);
		return _enemyList[index];
	}

	private void SetStatusMultiply()
	{
		switch (_playerData.difficult)
		{
			case DifficultType.Easy:
				statusMultiply = _game.easyStatus;
				break;
			case DifficultType.Normal:
				statusMultiply = _game.normalStatus;
				break;
			case DifficultType.Hard:
				statusMultiply = _game.hardStatus;
				break;
		}
	}
}
