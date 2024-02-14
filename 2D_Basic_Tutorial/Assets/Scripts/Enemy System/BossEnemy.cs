using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossEnemy : Enemy
{
	[Header("UI Boss Enemy")]
	[SerializeField] private GameObject _bossHealth;
	public float healthSliderSpeed = 1.5f;
	public float delayCameraToBoss = 5f;
	public float delayCameraToPlayer = 3f;

	[Header("Enemy Boss Combat")]
	public int minNumAttack = 1;
	public int numOfAttack;

	//private members
	private Slider _healthBar;
	private TextMeshProUGUI _bossName;
	private SavePointInteract _savePoint;

	//
	private SaveManager _save;
	private PlayerData _playerData;
	private PlayerInputControl _playerInput;

	//Anim ID
	private string _animAttackCombo = "AttackCombo";

	public void StartComponents()
	{
		base.Start();
		_save = SaveManager.instance;
		_playerData = PlayerData.instance;
		_playerInput = PlayerInputControl.instance;

		_healthBar = _bossHealth.GetComponentInChildren<Slider>();
		_bossName = _bossHealth.GetComponentInChildren<TextMeshProUGUI>();
		_savePoint = FindObjectOfType<SavePointInteract>(true);

		_bossName.text = name;
		_healthBar.maxValue = health;
		_healthBar.value = health;
	}

	public override void Update()
	{
		if (isDetected)
		{
			base.Update();
			UpdateHealthBar();
		}
	}

	public void SetupBoss()
	{
		StartComponents();
		StartCoroutine(StartBoss());
	}

	public IEnumerator StartBoss()
	{
		var camera = Camera.main.GetComponent<CameraController>();

		//Player
		_playerInput.LockAllInput();
		_playerInput.ResetAllInputVal();

		//UI
		_ui.HideGameUI();
		_sound.PlaySound(_sound.bossFightBackground_1, _sound.audioVolume * 0.1f);

		_savePoint.gameObject.SetActive(false);
		camera.target = transform;
		yield return new WaitForSeconds(delayCameraToBoss);
		camera.target = _player.transform;
		yield return new WaitForSeconds(delayCameraToPlayer);
		_playerInput.ResetLockAllInput();
		_bossHealth.SetActive(true);
		_ui.ShowGameUI();
		Detected();
	}

	public void UpdateHealthBar()
	{
		var currHealth = Mathf.Lerp(_healthBar.value, health, healthSliderSpeed * Time.deltaTime);
		_healthBar.value = currHealth;
	}

	public override void Attack()
	{
		var attackCombo = Random.Range(minNumAttack, numOfAttack + 1);
		_animator.SetFloat(_animAttackCombo, attackCombo);
		base.Attack();
	}

	public override void ResetAttack()
	{
		base.ResetAttack();
		_animator.SetFloat(_animAttackCombo, 0);
	}

	public override IEnumerator Dead()
	{
		yield return StartCoroutine(base.Dead());
		yield return new WaitForSeconds(1f);
		_ui.ShowDefeatScreen();
		_sound.OnBossDefeated();
		_savePoint.Resolve();
		_bossHealth.SetActive(false);
		SaveData();
		StartCoroutine(base.DisableEnemy());
	}

	private void SaveData()
	{
		_playerData.FindSceneData().bossDead = true;
		_save.SaveData();
	}
}
