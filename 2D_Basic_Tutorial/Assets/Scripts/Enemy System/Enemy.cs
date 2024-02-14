using TMPro;
using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public float health;
	public float maxHealth = 300f;
	public float moveSpeed = 4f;
	public int soulDrop = 100;

	[Header("UI")]
	[SerializeField] private GameObject _deadEffect;
	[SerializeField] private GameObject _damageText;

	[Header("Enemy Combat")]
	public Transform attackPoint;
	public bool isDetecting = true;
	public bool isDetected = false;
	public bool isAttack = false;
	public float attackDamage = 20f;
	public float SpotRange = 2.6f;
	public float attackRange = 4f;
	public float attackPointRange = 2f;
	public float attackDelayTime = 4f;
	public float attackSpeed = 1f;

	//private
	private int soulDropRang = 10;
	private float delayDead = 0.5f;
	private float _attackDeltaTime;
	private Vector2 _spawnPosition;
	private LayerMask _groundMask;
	private LayerMask _attackMask;

	//
	[HideInInspector] public UIManager _ui;
	[HideInInspector] public Transform _player;
	[HideInInspector] public Animator _animator;
	[HideInInspector] public SoundManager _sound;
	private Rigidbody2D _rigidBody;
	private CapsuleCollider2D _collider;
	private SpriteRenderer _sprite;
	private HealthManager _playerHp;
	private SoulManager _soul;
	private FlashEffect _flash;

	//Anim ID
	[HideInInspector] public string _animIdle = "Idle";
	[HideInInspector] public string _animWalk = "Walk";
	[HideInInspector] public string _animAttack = "Attack";
	[HideInInspector] public string _animAttackSpeed = "AttackSpeed";
	[HideInInspector] public string _animHit = "Hit";
	[HideInInspector] public string _animDead = "Dead";

	public virtual void Start()
	{
		_ui = UIManager.instance;
		_sound = SoundManager.instance;
		_rigidBody = GetComponent<Rigidbody2D>();
		_collider = GetComponent<CapsuleCollider2D>();
		_sprite = GetComponent<SpriteRenderer>();
		_animator = GetComponent<Animator>();
		_flash = GetComponent<FlashEffect>();

		_player = GameObject.FindWithTag("Player").transform;
		_playerHp = _player.GetComponent<HealthManager>();
		_soul = _player.GetComponent<SoulManager>();

		_groundMask = LayerMask.GetMask("Ground");
		_attackMask = LayerMask.GetMask("Player");

		_spawnPosition = transform.position;
		health = maxHealth;
	}

	public virtual void Update()
	{
		UpdateAnimate();
		DetectPlayer();
		MoveAndAttack();
	}

	private void UpdateAnimate()
	{
		isAttack = _animator.GetCurrentAnimatorStateInfo(0).IsName(_animAttack);
		_animator.SetFloat(_animAttackSpeed, attackSpeed);
	}

	private void DetectPlayer()
	{
		if (!isDetecting) return;
		if (Vector2.Distance(transform.position, _player.position) <= SpotRange)
		{
			Detected();
		}
	}

	public void Detected()
	{
		isDetecting = false;
		isDetected = true;
	}

	private void MoveAndAttack()
	{
		if (!GroundCheck() | _playerHp.isDeath | !isDetected | health <= 0f) return;

		LookAtPlayer();

		//Attack Checking
		var distance = Vector2.Distance(transform.position, _player.position);
		if (distance <= attackRange)
		{
			_animator.SetBool(_animWalk, false);
			if (_attackDeltaTime <= 0f)
			{
				_attackDeltaTime = attackDelayTime;
				Attack();
			}
		}

		//Move Checking
		else if (!isAttack)
		{
			if (!GroundWalkCheck())
			{
				ResetMove();
				return;
			}
			var movePos = transform.position + (moveSpeed * Time.deltaTime * transform.right);
			// var targetPos = new Vector2(target.transform.position.x, transform.position.y);
			// var movePos = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

			_rigidBody.MovePosition(movePos);
			_animator.SetBool(_animWalk, true);
		}
		else ResetMove();

		if (_attackDeltaTime > 0f & !isAttack)
		{
			_attackDeltaTime -= Time.deltaTime;
			if (_attackDeltaTime <= 0) ResetAttack();
		}
	}

	public virtual void Attack()
	{
		_animator.SetTrigger(_animAttack);
	}

	public virtual void ResetAttack()
	{
		_attackDeltaTime = 0;
	}

	private void ResetMove()
	{
		_animator.SetBool(_animWalk, false);
		_animator.ResetTrigger(_animAttack);
	}

	private void LookAtPlayer()
	{
		if (transform.position.x < _player.position.x)
		{
			transform.eulerAngles = new Vector2(0, 0);
		}
		else if (transform.position.x > _player.position.x)
		{
			transform.eulerAngles = new Vector2(0, -180);
		}
	}

	public bool GroundCheck()
	{
		return Physics2D.BoxCast(_collider.bounds.center,
		_collider.bounds.size, 0f, Vector2.down, 0.1f, _groundMask);
	}

	private bool GroundWalkCheck()
	{
		return Physics2D.BoxCast(_collider.bounds.center + transform.right * 1f,
		_collider.bounds.size, 0f, Vector2.down, 0.1f, _groundMask);
	}

	public void OnAttackHit()
	{
		var hitPoint = Physics2D.OverlapCircle(attackPoint.position, attackPointRange, _attackMask);
		if (hitPoint != null)
		{
			_playerHp.TakeDamage(transform, attackDamage);
		}
	}

	public void TakeDamage(float damage)
	{
		if (health <= 0f) return;

		isDetected = true;
		if (!isAttack) _animator.SetTrigger(_animHit);
		if (_flash != null) _flash.Flash();
		AudioSource.PlayClipAtPoint(_sound.EnemyDamaged, transform.position, _sound.audioVolume * 2f);
		StartCoroutine(DamageText(damage));

		health -= damage;
		if (health <= 0f)
		{
			StartCoroutine(Dead());
		}
	}

	private IEnumerator DamageText(float damage)
	{
		var position = _collider.bounds.center;
		var textDamage = Instantiate(_damageText, position, Quaternion.identity);
		textDamage.GetComponentInChildren<TextMeshProUGUI>().text = damage.ToString();
		yield return new WaitUntil(() => !textDamage.GetComponentInChildren<Animation>().isPlaying);
		Destroy(textDamage);
	}

	public virtual IEnumerator Dead()
	{
		health = 0f;
		isDetected = false;
		_sound.StopSound();
		if (_animator.GetBool(_animHit)) yield return new WaitForSeconds(1f);
		_animator.Play(_animIdle);
		_animator.SetTrigger(_animDead);
		if (_animator.GetBool(_animDead)) yield return new WaitForSeconds(1f);
		_rigidBody.simulated = false;
		_collider.isTrigger = true;
		for (int i = 0; i < 3; i++)
		{
			if (_flash != null) _flash.Flash();
			yield return new WaitForSeconds(delayDead);
		}
		_sound.OnEnemyDead();
		_deadEffect.SetActive(true);
		_sprite.enabled = false;
		SoulDrop();
	}

	private void SoulDrop()
	{
		var randSoul = Random.Range(soulDrop - soulDropRang, soulDrop + soulDropRang);
		_soul.DropSoul(transform.position, randSoul);
	}

	public virtual IEnumerator DisableEnemy()
	{
		yield return new WaitForSeconds(2f);
		gameObject.SetActive(false);
	}

	public void ResetEnemy()
	{
		transform.position = _spawnPosition;
		health = maxHealth;
		isDetecting = true;
		isDetected = false;
		_collider.isTrigger = false;
		_rigidBody.simulated = true;
		_sprite.enabled = true;
		_deadEffect.SetActive(false);
		gameObject.SetActive(true);
		_animator.SetBool(_animWalk, false);
		_animator.Play(_animIdle);
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere(transform.position, SpotRange);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRange);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(attackPoint.position, attackPointRange);
	}
}
