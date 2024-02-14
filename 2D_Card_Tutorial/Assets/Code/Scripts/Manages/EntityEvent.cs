using System.Collections;
using UnityEngine;

public class EntityEvent : MonoBehaviour, IPlaySFXEvent, IPlayEffectEvent
{
	[SerializeField] private Transform _floatingTextLayout;
	[SerializeField] private Transform _floatingStatusLayout;
	[SerializeField] private GameObject _buffEffect;
	[SerializeField] private GameObject _debuffEffect;
	[SerializeField] private GameObject _healingEffect;
	[SerializeField] private bool isRunOnDead;

	//
	protected Animator _animator;
	protected Collider2D _collider;
	protected SpriteRenderer _renderer;
	protected UIManager _ui;
	protected SoundManager _sound;
	protected Entity _entity;
	private MapManager _map;

	//AnimID
	private string _animSpawn = "Spawn";
	private string _animWalk = "Walk";
	private string _animEvade = "Evade";
	private string _animHit = "Hit";
	private string _animDead = "Dead";
	private string _animEscape = "Escape";

	protected virtual void Start()
	{
		_ui = UIManager.instance;
		_sound = SoundManager.instance;
		_map = MapManager.instance;
		_entity = GetComponent<Entity>();
		_animator = GetComponent<Animator>();
		_collider = GetComponent<Collider2D>();
		_renderer = GetComponent<SpriteRenderer>();

		//
		OnSpawn();
	}

	public void OnSpawn()
	{
		_animator.SetTrigger(_animSpawn);
	}

	public void OnEvade()
	{
		_animator.SetTrigger(_animEvade);
		_sound.OnPlaySFX(_sound.entityEvade);
		_ui.ShowFloatingText(_ui.evadeFloatingText, _floatingTextLayout);
	}

	public void OnHit(int damage)
	{
		_animator.SetTrigger(_animHit);
		_sound.OnPlaySFX(_sound.entityHit);
		_ui.ShowFloatingText(_ui.damageFloatingText, _floatingTextLayout, damage.ToString());
	}

	public void OnTotalHit(int damage)
	{
		_ui.ShowFloatingTotalDamage(_floatingTextLayout, damage.ToString());
	}

	public void OnDead()
	{
		if (isRunOnDead) StartCoroutine(OnRunDead());
		else _animator.SetTrigger(_animDead);
	}

	private IEnumerator OnRunDead()
	{
		var flipX = transform.GetComponent<SpriteRenderer>().flipX ? -1 : 1;
		var target = transform.position + Vector3.right * flipX * 10f;
		_animator.SetTrigger(_animEscape);
		yield return StartCoroutine(_entity.MoveToTarget(target, false, false));
		_entity.isDeadFinish = true;
	}

	public void OnMove(bool isMove)
	{
		_animator.SetBool(_animWalk, isMove);
	}

	public void OnHealingUp(int healthTaken)
	{
		OnPlayEffect(_healingEffect);
		_ui.ShowFloatingText(_ui.healingFloatingText, _floatingTextLayout, $"+{healthTaken}");
	}

	public void OnBuff(StatusType statusType)
	{
		OnPlayEffect(_buffEffect);
		_ui.ShowFloatingStatus(_ui.statusBuffFloating, _floatingStatusLayout, statusType);
	}

	public void OnDebuff(StatusType statusType)
	{
		OnPlayEffect(_debuffEffect);
		_sound.OnPlaySFX(_sound.statusDown, 0.3f);
		_ui.ShowFloatingStatus(_ui.statusDebuffFloating, _floatingStatusLayout, statusType);
	}

	public void OnPlaySFX(AudioClip audio)
	{
		_sound.OnPlaySFX(audio);
	}

	public void OnPlayEffect(GameObject effect)
	{
		var angle = _renderer.flipX ? 0f : 180f;
		var direction = Quaternion.Euler(0f, angle, 0f);
		Instantiate(effect, _collider.bounds.center, direction);
	}

	private IEnumerator OnPlayUIAnimate(GameObject gameObject)
	{
		var anim = gameObject.GetComponent<Animation>();
		gameObject.SetActive(true);
		if (anim != null)
		{
			anim.Play();
			yield return new WaitUntil(() => !anim.isPlaying);
			gameObject.SetActive(false);
		}
	}
}
