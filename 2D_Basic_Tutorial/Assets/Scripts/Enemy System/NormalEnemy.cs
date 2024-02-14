using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NormalEnemy : Enemy
{
	[Header("UI Normal Enemy")]
	[SerializeField] private GameObject _healthBarObject;
	[SerializeField] private Vector2 HealthBarOffset = new Vector2(0f, 1f);

	//private
	private GameObject _healthBar;
	private Slider _healthSlider;

	public override void Start()
	{
		base.Start();
		CreateHealthBar();
	}

	private void CreateHealthBar()
	{
		_healthBar = Instantiate(_healthBarObject, _ui.transform);
		_healthBar.transform.position = transform.position;
		_healthSlider = _healthBar.GetComponentInChildren<Slider>();
		_healthSlider.maxValue = maxHealth;
	}

	public override void Update()
	{
		base.Update();
		UpdateHealthBar();
	}

	private void UpdateHealthBar()
	{
		if (!isDetected)
		{
			_healthBar.SetActive(false);
			return;
		}

		_healthBar.SetActive(true);
		_healthBar.transform.position = transform.position;
		_healthSlider.transform.localPosition = HealthBarOffset;
		_healthSlider.value = health;
	}

	public override IEnumerator Dead()
	{
		yield return StartCoroutine(base.Dead());
		StartCoroutine(base.DisableEnemy());
	}

}
