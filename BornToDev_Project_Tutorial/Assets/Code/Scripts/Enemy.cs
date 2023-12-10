using Lean.Pool;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] private GameObject _deadEffect;
	[SerializeField] private GameObject _scorePopup;
	public bool isMove = true;
	public float distanceMove = 5f;
	public int defaultMoveSpeed = 15;
	public int moveSpeedRang = 5;
	public int maxHealth = 90;
	public int defaultScore = 1;

	[Header("Game Status")]
	public float moveSpeed;
	public int health;
	public int score;

	[Header("UI")]
	public float scoreDistance = 0.5f;

	//Private members
	private Vector3 _targetPoint;
	private Vector3 _startPoint;
	private Vector3 _endPoint;
	private GameManager _game;
	private EnemyManager _enemy;
	private SoundManager _sound;
	private Renderer _renderer;
	private Collider _collider;
	private FlashEffect _flashEffect;
	private Transform _player;

	void Start()
	{
		if (_renderer != null) return;
		_game = GameManager.instance;
		_enemy = EnemyManager.instance;
		_sound = SoundManager.instance;
		_renderer = GetComponent<Renderer>();
		_collider = GetComponent<Collider>();
		_flashEffect = GetComponent<FlashEffect>();
		_player = GameObject.FindWithTag("Player").transform;
	}

	private void FixedUpdate()
	{
		Moving();
	}

	private void Moving()
	{
		if (isMove)
		{
			var currPos = Vector3.MoveTowards(transform.position, _targetPoint, moveSpeed * Time.deltaTime);
			transform.position = currPos;
			if (Vector3.Distance(transform.position, _targetPoint) <= 0.1f)
			{
				_targetPoint = _targetPoint == _endPoint ? _startPoint : _endPoint;
			}
		}
	}

	private void SetupPoint()
	{
		_startPoint = transform.position;
		_endPoint = transform.position + transform.right * distanceMove;
		_targetPoint = _endPoint;
	}

	[ContextMenu("TakeDamage(90)")]
	public void TakeDamage(int damage)
	{
		if (health > 0)
		{
			health -= damage;
			DebugDamage(damage);
			FlashEffect();
			if (health <= 0)
			{
				OnDead();
			}
		}
	}

	public void OnDead()
	{
		var deadEffect = LeanPool.Spawn(_deadEffect, transform.position, Quaternion.identity);
		var scorePopup = LeanPool.Spawn(_scorePopup, transform.position + Vector3.up * scoreDistance, _player.rotation);
		scorePopup.GetComponentInChildren<TextMeshPro>(true).text = "+" + score;
		transform.rotation = Quaternion.identity;
		_renderer.enabled = false;
		_collider.enabled = false;
		_game.AddScore(score);
		_enemy.EnemyDecrease();
		_sound.OnPlaySFX(_sound.enemyDead, 0.5f);

		LeanPool.Despawn(deadEffect, 2f);
		LeanPool.Despawn(scorePopup, 2f);
		LeanPool.Despawn(gameObject, 2f);
	}

	public void Reset()
	{
		if (_renderer == null) Start();
		moveSpeed = Random.Range(defaultMoveSpeed - moveSpeedRang, defaultMoveSpeed + moveSpeedRang) / 10f;
		health = maxHealth;
		score = defaultScore;
		_renderer.enabled = true;
		_collider.enabled = true;
		
		ResetFlashEffect();
		SetupPoint();
	}

	private void FlashEffect()
	{
		if (_flashEffect != null) _flashEffect.Show();
		else Debug.LogWarning("Don't have Flash Effect component");
	}

	private void ResetFlashEffect()
	{
		if (_flashEffect != null) _flashEffect.Reset();
		else Debug.LogWarning("Don't have Flash Effect component");
	}

	private void DebugDamage(int damage)
	{
		Debug.Log($"Enemy: {name} Take Damage [{damage}].");
	}
}
