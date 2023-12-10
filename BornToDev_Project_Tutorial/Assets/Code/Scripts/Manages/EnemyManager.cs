using UnityEngine;
using Lean.Pool;

public class EnemyManager : MonoBehaviour
{
	[SerializeField] private GameObject _enemyObject;
	[SerializeField] private Transform[] _spawnPositions;
	public bool isSpawn;
	public float delaySpawn = 5f; //seconds
	public int limitEnemy = 5;

	//
	private float _spawnDeltaTime;
	private int _enemyNum;

	//
	public static EnemyManager instance;
	private void Awake()
	{
		instance = this;
	}

	private void FixedUpdate()
	{
		SpawnEnemy();
	}

	private void SpawnEnemy()
	{
		if (isSpawn & _spawnDeltaTime <= 0f & _enemyNum < limitEnemy)
		{
			_enemyNum++;
			_spawnDeltaTime = delaySpawn;
			var randPos = Random.Range(0, _spawnPositions.Length);
			var spawn = _spawnPositions[randPos];
			var enemy = LeanPool.Spawn(_enemyObject, spawn.position, spawn.rotation);
			enemy.GetComponent<Enemy>().Reset();
		}

		if (_spawnDeltaTime > 0f) _spawnDeltaTime -= Time.deltaTime;
	}

	public void EnemyDecrease()
	{
		_enemyNum--;
	}

	public void SetupEnemy()
	{
		isSpawn = true;
	}

	public void ResetEnemy()
	{
		isSpawn = false;
	}

}
