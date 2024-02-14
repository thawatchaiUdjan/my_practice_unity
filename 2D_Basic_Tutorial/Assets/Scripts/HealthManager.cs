using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
	[SerializeField] private Slider healthSlider;
	[SerializeField] private float Health;
	public float health
	{

		get => Health;
		set
		{
			Health = value;
			_playerData.health = Health;
		}
	}
	public float maxHealth = 100f;
	public bool isDeath = false;
	public float sliderSpeed = 1.5f;
	public float delayTakeDamage = 0.5f;

	//
	private Coroutine _damaged;
	private PlayerData _playerData;
	private GameManager _game;
	private PlayerController _player;

	private void StartComponents()
	{
		_playerData = PlayerData.instance;
		_game = GameManager.instance;
		_player = GetComponent<PlayerController>();

		healthSlider.maxValue = maxHealth;
	}

	public void SetupHealth(float health)
	{
		StartComponents();
		this.health = health != 0f ? health : maxHealth;
	}

	void Update()
	{
		var currHealth = Mathf.Lerp(healthSlider.value, health, sliderSpeed * Time.deltaTime);
		healthSlider.value = currHealth;
	}

	public void TakeDamage(Transform enemy, float damage, bool blockAble = true)
	{
		if (isDeath | _damaged != null) return;
		_damaged = StartCoroutine(Damage(enemy, damage, blockAble));
	}

	private IEnumerator Damage(Transform enemy, float damage, bool blockAble)
	{
		damage = _player.CheckDamage(enemy, damage, blockAble);
		health -= damage;
		Debug.Log($"Player Take Damage: [{damage}] from [{enemy.name}]");
		if (health <= 0)
		{
			health = 0f;
			isDeath = true;
			_game.GameOver();
		}
		yield return new WaitForSeconds(delayTakeDamage);
		_damaged = null;
	}

	public void GetHealth(float hpGain)
	{
		health += hpGain;
		if (health >= maxHealth) health = maxHealth;
	}

	public void ResetHealth()
	{
		health = maxHealth;
	}


}
