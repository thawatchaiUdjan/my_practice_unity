using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] private GameObject _gravestone;

	//
	private PlayerData _playerData;
	private SaveManager _save;
	private PlayerInputControl _input;
	private UIManager _ui;
	private SoundManager _sound;
	private PlayerController _player;
	private Animator _animator;
	private SoulManager _soul;
	private HealthManager _health;
	private PotionManager _potion;
	private Vector2 _spawnPoint;
	private Vector2 _savePoint;


	//Anim ID
	private string _animRestoreIn = "RestoreIn";
	private string _animRestoreOut = "RestoreOut";
	private string _animSpeedAnim = "SpeedAnim";
	private string _animDeath = "Dead";

	//
	public static GameManager instance;
	private void Awake()
	{
		instance = this;
	}

	public void SetupComponent()
	{
		_playerData = PlayerData.instance;
		_save = SaveManager.instance;
		_input = PlayerInputControl.instance;
		_ui = UIManager.instance;
		_sound = SoundManager.instance;

		_player = _input.GetComponent<PlayerController>();
		_animator = _player.GetComponent<Animator>();
		_soul = _player.GetComponent<SoulManager>();
		_health = _player.GetComponent<HealthManager>();
		_potion = _player.GetComponent<PotionManager>();

		_spawnPoint = GameObject.FindWithTag("Respawn").transform.position;
		_savePoint = FindObjectOfType<SavePointInteract>(true).transform.position;
	}

	public void GameSetup(bool isTravel = false)
	{
		SetupComponent();
		SetupData(isTravel);

		SetupPlayerPosition();
		SetupGraveStone();

		_input.LockAllInput();
		_input.ResetAllInputVal();
		_health.SetupHealth(_playerData.health);
		_potion.SetupPotion(_playerData.potions, _playerData.maxPotion);
		_soul.SetupSoul(_playerData.soul);
		StartCoroutine(AnimatePlayerIn(isTravel));
	}

	public void SetupData(bool travel)
	{
		_save.LoadData(); //Load Player Data
		_save.LoadSceneData(); //Load Scene Data

		if (_playerData.pos != Vector2.zero)
			_playerData.pos = travel ? _savePoint : _spawnPoint;
	}

	private void SetupPlayerPosition()
	{
		var extendPos = Vector2.up * 0.63f;
		var position = _playerData.pos;
		position = position == Vector2.zero ? _spawnPoint : position;
		_player.transform.position = position + extendPos;
	}

	private void SetupGraveStone()
	{
		if (_playerData.deadScene == SceneManager.GetActiveScene().buildIndex)
		CreateGraveStone(_playerData.deadPosition);
	}

	private IEnumerator AnimatePlayerIn(bool travel)
	{
		_sound.OnWarpTravel();
		if (travel)
		{
			_animator.SetFloat(_animSpeedAnim, 99f); //Hack Speed Anim
			_animator.SetTrigger(_animRestoreIn);
			yield return StartCoroutine(_player.GetComponent<DissolveEffect>().Resolve());
			_animator.SetFloat(_animSpeedAnim, 1f); //Reset Speed Anim 
			_animator.SetTrigger(_animRestoreOut);
		}
		else
		{
			yield return StartCoroutine(_player.GetComponent<DissolveEffect>().Resolve());
			_input.ResetLockAllInput();
		}
	}

	public void GameOver()
	{
		_input.LockAllInput();
		_input.ResetAllInputVal();
		_player.ResetAttack();
		_ui.ShowDeadScreen();
		_sound.StopSound();
		_sound.OnPlayerDead();
		_animator.SetTrigger(_animDeath); //Animate
		StartCoroutine(SaveDataOnDead());
	}

	public IEnumerator SaveDataOnDead()
	{
		_playerData.deadScene = SceneManager.GetActiveScene().buildIndex;
		_playerData.deadSoulBefore = _soul.soul;
		_playerData.health = _health.maxHealth;
		_playerData.potions = _potion.maxPotion;
		yield return new WaitForSeconds(2f);
		CreateGraveStone(_player.transform.position);
		_playerData.deadPosition = _player.lastGroundPos;
		_soul.UseSoul(_soul.soul);
		_save.SaveData();
	}

	public void GameRevive()
	{
		_soul.GetSoul(_playerData.deadSoulBefore);
		_sound.OnGraveStonePickup();
		_ui.ShowReviveScreen();
		_playerData.ResetDead();
	}

	public void CreateGraveStone(Vector2 deadPos)
	{
		var position = deadPos - Vector2.up * 0.3f;
		Destroy(GameObject.FindGameObjectWithTag("Gravestone"));
		Instantiate(_gravestone, position, Quaternion.identity);
	}

}
