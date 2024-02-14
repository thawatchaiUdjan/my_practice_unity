using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
	[SerializeField] private GameObject _gameUI;

	[Header("Entity Status")]
	public float moveSpeed = 12;
	public bool isAction;
	public bool isDead;
	public bool isDeadFinish;

	//
	protected string _animName;
	protected int _damageOut;
	private int _totalDamageIn;
	private int _hitCount;
	private bool _isMove;
	private bool _isDamaged;
	private bool _isEvade;
	private bool _revert;
	private List<StatusInfo> _statuses;
	private Vector2 _defaultPos;
	private Vector2 _moveTarget;
	private Vector2 _currMovePos;

	//
	protected GameManager _game;
	protected EntityStatus _status;
	protected EntityEvent _event;
	protected EntityHealth _health;
	private Animator _animator;
	private BoxCollider2D _collider;

	protected virtual void Start()
	{
		if (_game != null) return;
		_game = GameManager.instance;
		_event = GetComponent<EntityEvent>();
		_health = GetComponent<EntityHealth>();
		_status = GetComponent<EntityStatus>();
		_animator = GetComponent<Animator>();
		_collider = GetComponent<BoxCollider2D>();
		_game.onActionDoneCallback += OnActionDone;
		_defaultPos = transform.position;
	}

	public virtual void SetupData(Character character)
	{
		Start();
		_status.SetupStatus(character);
		_health.SetupHealth();
	}

	private void FixedUpdate()
	{
		Move();
	}

	private void Move()
	{
		if (_isMove)
		{
			_currMovePos = Vector2.MoveTowards(_currMovePos, _moveTarget, moveSpeed * Time.deltaTime);
			transform.position = _currMovePos;

			if (Vector2.Distance(transform.position, _moveTarget) <= 0.1f)
			{
				MoveDone();
			}
		}
	}

	private void MoveDone()
	{
		_isMove = false;
		_moveTarget = Vector2.zero;
		_event.OnMove(false);
	}

	private void LookAtTarget(Vector2 target)
	{
		if (transform.position.x > target.x)
		{
			transform.eulerAngles = new Vector2(0f, _revert ? 0f : 180f);
		}
		else if (transform.position.x < target.x)
		{
			transform.eulerAngles = new Vector2(0f, _revert ? 180f : 0f);
		}
	}

	protected void CheckActionSkill(Transform target, ActionType actionType)
	{
		_revert = target.GetComponent<PlayerController>() != null; //If Target is Player
		switch (actionType)
		{
			case ActionType.Normal:
				StartCoroutine(NormalSkill());
				break;
			case ActionType.Melee:
				StartCoroutine(MeleeSkill(target));
				break;
		}
	}

	private IEnumerator NormalSkill()
	{
		yield return StartCoroutine(PlayAnimation(_animName));
		ActionDone();
	}

	private IEnumerator MeleeSkill(Transform target)
	{
		ShowEntityGameUI(false);
		yield return StartCoroutine(MoveToTarget(GetAttackPosition(target)));
		yield return StartCoroutine(PlayAnimation(_animName));
		yield return StartCoroutine(MoveToTarget(_defaultPos));
		LookAtTarget(target.position);
		ShowEntityGameUI(true);
		ActionDone();
	}

	public IEnumerator MoveToTarget(Vector2 target, bool isLook = true, bool isAnim = true)
	{
		_currMovePos = transform.position;
		_moveTarget = target;
		_isMove = true;
		if (isAnim) _event.OnMove(true);
		if (isLook) LookAtTarget(target);
		yield return new WaitUntil(() => !_isMove);
	}

	private IEnumerator PlayAnimation(string animName)
	{
		_animator.Play(animName);
		yield return new WaitForSeconds(0.1f);
		yield return new WaitUntil(() => !_animator.GetCurrentAnimatorStateInfo(0).IsName(animName));
	}

	private Vector2 GetAttackPosition(Transform target)
	{
		var position = target.position - Vector3.right * _collider.size.x * (_revert ? -1f : 1f);
		var smoothPosition = position - Vector3.right * (_collider.size.x / 1.8f) * (_revert ? -1f : 1f); //Depend On you
		return smoothPosition;
	}

	private void TakeDamage(int damageTaken, List<StatusInfo> statuses, bool evadeAble, int hits)
	{
		if (!_isDamaged)
		{
			_isDamaged = true;
			_statuses = statuses;
			_isEvade = evadeAble & _status.CalculateEvade();
		}
		if (!_isEvade) //Can't Evade
		{
			var damageIn = _status.CalculateDamageIn(damageTaken, hits);
			var oldHealth = _health.health;
			_hitCount++;
			_totalDamageIn += damageIn;
			_health.TakeHealth(damageIn);
			if (hits == 1) TakeStatus(); //If One hit take status effect immediately
			Debug.Log($"<{name}> Take Damaged: [{damageIn} DMG], HP [{oldHealth} => {_health.health}]");
		}
		else //Can Evade
		{
			_event.OnEvade();
			Debug.Log($"<{name}> has Evaded! the Attack");
		}
	}

	public void AttackDamage(Entity target, int percentAttack, List<StatusInfo> statuses = null, int hits = 1, bool evadeAble = true)
	{
		var damageTaken = _status.CalculateDamageOut(percentAttack);

		target.TakeDamage(damageTaken, statuses, evadeAble, hits);
	}

	public void ShowEntityGameUI(bool isShow)
	{
		var alpha = isShow ? 1f : 0f;
		_gameUI.GetComponent<CanvasGroup>().alpha = alpha;
	}

	private IEnumerator ShowTotalDamage()
	{
		if (_hitCount > 1)
		{
			yield return new WaitForSeconds(1f);
			_event.OnTotalHit(_totalDamageIn);
			Debug.Log($"<{name}> Take Total Damaged: [{_totalDamageIn} DMG]");
		}
		_hitCount = 0;
		_totalDamageIn = 0;
	}

	private void TakeStatus()
	{
		if (_statuses != null & !_isEvade)
		{
			_status.GetStatus(_statuses);	
		}
		_statuses = null;
	}

	private void ResetDamage()
	{
		_isDamaged = false;
	}

	private void OnActionDone()
	{
		TakeStatus();
		ResetDamage();
		StartCoroutine(ShowTotalDamage());
	}

	protected virtual void ActionDone()
	{
		isAction = false;
		_game.onActionDoneCallback?.Invoke();
	}

	protected IEnumerator CheckDead()
	{
		if (isDead)
		{
			_health.Dead();
			yield return new WaitUntil(() => isDeadFinish);
			DeadDone();
		}
		else
		{
			StartAction();
		}
	}

	public void ResetCharacter()
	{
		_status.ResetStatus();
		_health.SetupHealth();
	}

	protected virtual void StartAction() { }
	protected virtual void DeadDone() { }

	private void OnDestroy()
	{
		_game.onActionDoneCallback -= OnActionDone;
	}
}
