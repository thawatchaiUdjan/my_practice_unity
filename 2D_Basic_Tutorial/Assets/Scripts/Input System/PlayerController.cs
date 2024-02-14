using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float MoveSpeed = 3f;
	public float sprintSpeed = 5f;
	public float jumpPower = 250f;
	public float slidePower = 10f;
	public bool moveAble = true;
	public bool isJump;
	public bool isWarp;
	public Vector2 lastGroundPos;

	[Header("Game Objects")]
	[SerializeField] private GameObject[] slashEffects;
	[SerializeField] private GameObject perfectBlockEffect;
	[SerializeField] private GameObject restoreEffect;

	[Header("Player Combat")]
	public Transform attackPoint;
	public int attackDamageCombo_1 = 15;
	public int attackDamageCombo_2 = 25;
	public int attackDamageCombo_3 = 35;
	public int damageRange = 4;
	public float attackRange = 0.8f;
	public float attackDelayTime = 0.7f;
	public float attackComboTime = 0.3f;
	public bool isAttack = false;
	public bool isPerfectBlock = false;

	[Space(10)]
	public float blockDelayTime = 0.5f;
	public float blockPerfectTime = 0.2f;
	public float perfectBlockTimeOut = 2f;

	[Space(10)]
	public float delayDamaged = 1f;
	public float knockBackDamaged = 100f;
	public float distanceCombo_1 = 0.4f;
	public float distanceCombo_3 = 0.8f;

	//private
	private Coroutine _isDamaged;
	private LayerMask _groundMask;
	private Vector2 _groundSize;
	private Vector2 _groundOrigin;
	private float _speed;
	private float _attackDeltaTime = 0f;
	private float _blockDeltaTime = 0f;
	private float _perfectBlockDeltaTime = 0f;
	private int _perfectBlockCombo = 0;
	private int _attackCombo = 0;

	//
	private Rigidbody2D _rigidBody;
	private BoxCollider2D _boxColl;
	private Animator _animator;
	private FlashEffect _flash;
	private PotionManager _potion;
	private PlayerInputControl _input;
	private HealthManager _health;
	private SoundManager _sound;

	//Anim ID
	private string _animSpeed = "Speed";
	private string _animJump = "Jump";
	private string _animAttack = "Attack";
	private string _animAttackCombo = "AttackCombo";
	private string _animBlock = "Block";
	private string _animHit = "Hit";

	void Start()
	{
		_rigidBody = GetComponent<Rigidbody2D>();
		_boxColl = GetComponent<BoxCollider2D>();
		_animator = GetComponent<Animator>();
		_flash = GetComponent<FlashEffect>();
		_potion = GetComponent<PotionManager>();
		_health = GetComponent<HealthManager>();
		_input = PlayerInputControl.instance;
		_sound = SoundManager.instance;

		_groundMask = LayerMask.GetMask("Ground");
	}

	private void Update()
	{
		Move();
		Jump();
		Attack();
		Block();
		PotionUse();
		PerfectBlockTime();
	}

	private void FixedUpdate()
	{
		if (GroundCheck() & !_health.isDeath)
		{
			lastGroundPos = transform.position - transform.right * 0.5f;
		}
	}

	private void Move()
	{
		if (IsPlayerCanAction() | _potion.isPotionUse)
		{
			// var movePosition = (Vector2)transform.position + (_input * targetSpeed * Time.deltaTime * Vector2.right);
			_speed = _input.move.x * (_input.sprint & GroundCheck() ? sprintSpeed : MoveSpeed);
			if (_speed < -0.1f) transform.eulerAngles = new Vector2(0, 180);
			else if (_speed > 0.1f) transform.eulerAngles = new Vector2(0, 0);

			transform.Translate(Mathf.Abs(_speed) * Time.deltaTime * Vector2.right);
			// _rigidBody.MovePosition(movePosition);
		}
		else
		{
			_speed = 0f;
		}

		_animator.SetFloat(_animSpeed, Mathf.Abs(_speed));
	}

	private void Jump()
	{
		if (_input.jump)
		{
			_input.jump = false;
			if (isJump | !GroundCheck()) return;
			ResetAttack();
			isJump = true;
			_input.LockAllInput(move: true);
			_rigidBody.AddForce(Vector2.up * jumpPower);
			_animator.SetBool(_animJump, true);
		}
	}

	public void ResetJump()
	{
		if (!IsPlayerCanAction()) return;
		isJump = false;
		_input.ResetLockAllInput();
		_animator.SetBool(_animJump, false);
	}

	private bool GroundCheck()
	{
		_groundOrigin = _boxColl.bounds.center - Vector3.up * 0.1f;
		_groundSize = _boxColl.bounds.size - new Vector3(0.3f, 0.3f, 0);
		var grounded = Physics2D.BoxCast(_groundOrigin, _groundSize, 0f, Vector2.down, 0.1f, _groundMask);
		return grounded;
	}

	private void Attack()
	{
		if (_input.attack)
		{
			_input.attack = false;
			if (_attackCombo > 2 | !IsPlayerCanAction()) return;
			if (_attackDeltaTime <= attackComboTime)
			{
				moveAble = false;
				_input.LockAllInput(move: true);
				_animator.SetTrigger(_animAttack);
				_animator.SetFloat(_animAttackCombo, ++_attackCombo);
				_attackDeltaTime = attackDelayTime + attackComboTime;
			}
		}

		if (_attackDeltaTime > 0f)
		{
			_attackDeltaTime -= Time.deltaTime;

			if (_attackDeltaTime <= attackComboTime)
			{
				moveAble = true;
				_input.ResetLockAllInput();
			}

			if (_attackDeltaTime <= 0f)
			{
				ResetAttack();
			}
		}
	}

	public void ResetAttack()
	{
		_animator.SetFloat(_animAttackCombo, 0f);
		_attackCombo = 0;
		_attackDeltaTime = 0f;

		foreach (var slash in slashEffects) slash.SetActive(false);
	}

	private void Block()
	{
		if (_input.block & _blockDeltaTime <= 0 & IsPlayerCanAction())
		{
			_input.LockAllInput(block: true, move: true);
			_animator.SetBool(_animBlock, true);
			_blockDeltaTime = blockDelayTime;
			isPerfectBlock = true;
			moveAble = false;
		}
		else if (!_input.block & _blockDeltaTime > 0f & _input.blockAble)
		{
			moveAble = true;
			ResetBlock();
			_input.blockAble = false;
		}

		if (_blockDeltaTime > 0f)
		{
			if (_blockDeltaTime <= blockDelayTime - blockPerfectTime)
			{
				isPerfectBlock = false;
				if (!_input.blockAble) _blockDeltaTime -= Time.deltaTime;
			}
			else
			{
				_blockDeltaTime -= Time.deltaTime;
			}

			if (_blockDeltaTime <= 0)
			{
				_input.blockAble = true;
			}
		}
	}

	public void ResetBlock()
	{
		if (_input.block | !IsPlayerCanAction()) return;
		_input.ResetLockAllInput();
		_animator.SetBool(_animBlock, false);
		_animator.SetBool(_animJump, false);
		isPerfectBlock = false;
	}

	private void PotionUse()
	{
		if (_input.potion & IsPlayerCanAction() & GroundCheck())
		{
			_input.potion = false;
			_potion.UsePotion();
		}

		if (_input.sprint & _potion.isPotionUse) _potion.StopPotionUse();
	}

	public float CheckDamage(Transform enemy, float damage, bool blockAble)
	{
		var delay = delayDamaged;
		var dist = knockBackDamaged;
		var block = false;

		if (isPerfectBlock & blockAble)
		{
			_perfectBlockDeltaTime = perfectBlockTimeOut;
			_sound.OnPerfectBlock(_perfectBlockCombo++);
			StartCoroutine(PerfectBlockAnim());
			return 0f;
		}

		if (_input.block & blockAble)
		{
			_sound.OnBlock();
			damage /= 2f;
			delay /= 2f;
			dist /= 1.5f;
			block = true;
		}
		else
		{
			_sound.OnPlayerDmg();
			_animator.SetBool(_animBlock, false);
			_animator.SetBool(_animHit, true);
		}

		ResetAttack();
		moveAble = false;
		_input.LockAllInput(block: block, move: true);

		_potion.StopPotionUse();

		if (_isDamaged != null) StopCoroutine(_isDamaged);
		_isDamaged = StartCoroutine(AfterTakeDamage(enemy, delay, dist));

		return damage;
	}

	public IEnumerator PerfectBlockAnim()
	{
		perfectBlockEffect.SetActive(true);
		yield return new WaitForSeconds(0.3f);
		perfectBlockEffect.SetActive(false);
	}

	private void PerfectBlockTime()
	{
		if (_perfectBlockDeltaTime > 0f)
		{
			_perfectBlockDeltaTime -= Time.deltaTime;
			if (_perfectBlockDeltaTime <= 0f)
			{
				_perfectBlockCombo = 0;
			}
		}
	}

	public IEnumerator AfterTakeDamage(Transform enemy, float delay, float dist)
	{
		var pos = CheckSideEnemy(enemy) ? -Vector2.right : Vector2.right;
		_rigidBody.AddForce(dist * pos);
		_flash.Flash();
		yield return new WaitForSeconds(delay);
		_animator.SetBool(_animHit, false);
		_isDamaged = null;
		yield return new WaitUntil(() => GroundCheck());
		if (!_input.block) moveAble = true;
		ResetBlock();
		ResetJump();
	}

	private bool CheckSideEnemy(Transform target)
	{
		return transform.position.x < target.transform.position.x;
	}

	public bool IsPlayerCanAction()
	{
		var isBool = _isDamaged == null & !_health.isDeath & !_potion.isPotionUse 
		& moveAble & !isWarp;
		return isBool;
	}

	private void OnCollisionEnter2D(Collision2D target)
	{
		if (LayerMask.LayerToName(target.gameObject.layer) == "Ground"
		& isJump & IsPlayerCanAction())
		{
			StartCoroutine(WaitGround());
		}
	}

	private IEnumerator WaitGround()
	{
		yield return new WaitUntil(() => GroundCheck());
		ResetJump();
	}

	private void OnCollisionStay2D(Collision2D target)
	{
		//Slide Player off Enemy Head
		if (target.gameObject.CompareTag("Enemy") & !GroundCheck())
		{
			var xPos = CheckSideEnemy(target.transform) ? -1 : 1;
			_input.LockAllInput();
			_rigidBody.AddForce(new Vector2(xPos, -1) * slidePower);
			isJump = true;
		}
	}

	private void OnDrawGizmosSelected()
	{
		if (attackPoint != null)
		{
			Gizmos.DrawWireSphere(attackPoint.position, attackRange);
		}

		if (_boxColl != null)
		{
			Gizmos.color = Color.black;
			Gizmos.DrawWireCube(_groundOrigin, _groundSize);
		}
	}

}