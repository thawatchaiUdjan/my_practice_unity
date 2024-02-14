using UnityEngine;
using UnityEngine.UI;

public class EntityHealth : MonoBehaviour
{
	public int health;
	[HideInInspector] public int maxHealth = 1200;

	[Header("Health Bar Setting")]
	[SerializeField] private Slider _healthBar;
	[SerializeField] private float _speedSlider = 5f;

	//
	protected float _currHealth;
	protected EntityEvent _event;
	protected Entity _entity;
	protected GameManager _game;

	protected virtual void Start()
	{
		_game = GameManager.instance;
		_entity = GetComponent<Entity>();
		_event = GetComponent<EntityEvent>();
	}

	private void Update()
	{
		UpdateHealth();
	}

	private void UpdateHealth()
	{
		_currHealth = Mathf.Lerp(_currHealth, health, _speedSlider * Time.deltaTime);
		_healthBar.value = _currHealth;
	}

	public virtual void SetupHealth()
	{
		_healthBar.maxValue = maxHealth;
		_healthBar.value = health;
		_currHealth = health;
	}

	public void GetHealth(int statusUp)
	{
		maxHealth += statusUp;
		_healthBar.maxValue = maxHealth;
	}

	public void GetHeal(int percentHealth)
	{
		var healthTaken = Mathf.RoundToInt(percentHealth / 100f * maxHealth);
		health += healthTaken;
		if (health > maxHealth) health = maxHealth;
		_event.OnHealingUp(healthTaken);
	}

	public void TakeHealth(int damage)
	{
		health -= damage;
		_event.OnHit(damage);

		if (health <= 0)
		{
			health = 0;
			_entity.isDead = true;
		}
	}

	public void Dead()
	{
		_entity.ShowEntityGameUI(false);
		_event.OnDead();
	}
}
