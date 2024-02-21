using FishNet.Object;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : NetworkBehaviour
{
	[Header("Health Setting")]
	[SerializeField] private int _health;
	[SerializeField] private int _maxHealth = 100;
	[SerializeField] private bool _isDead;
	
	[Header("Game UI")]
	[SerializeField] private Slider _healthBar;
	[SerializeField] private TextMeshProUGUI _healthText;
	[SerializeField] private float _speed = 1f;

	private void Start()
	{
		_health = _maxHealth;
		_healthBar.maxValue = _health;
		_healthBar.value = _health;
	}

	private void Update()
	{
		_healthBar.value = Mathf.MoveTowards(_healthBar.value, _health, _speed * Time.deltaTime);
		_healthText.text = $"{_maxHealth}/{_healthBar.value}";
	}

	public void TakeDamage(int damage)
	{
		if (_health <= 0) return;
		_health -= damage;
		if (_health <= 0)
		{
			_health = 0;
			_isDead = true;
		}
	}

}
