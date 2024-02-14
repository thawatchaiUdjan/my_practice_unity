using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePointInteract : Interactable
{
	[SerializeField] private GameObject _openEffect;

	private PlayerData _playerData;
	private GameObject[] _enemies;

	//AnimID
	private string _animRestoreIn = "RestoreIn";


	private void Start()
	{
		_playerData = PlayerData.instance;
		_enemies = GameObject.FindGameObjectsWithTag("Enemy");
	}

	public override void Interact(Interactor interactor)
	{
		base.Interact(interactor);
		_playerData.scene = SceneManager.GetActiveScene().buildIndex;
		_playerData.pos = transform.position;
		_playerData.FindSceneData().savePointOpened = true;
		_openEffect.SetActive(true);

		interactor.GetComponent<PlayerInputControl>().LockAllInput();
		interactor.GetComponent<PlayerInputControl>().ResetAllInputVal();
		interactor.GetComponent<PlayerController>().ResetAttack();
		interactor.GetComponent<HealthManager>().ResetHealth();
		interactor.GetComponent<PotionManager>().ResetPotion();
		interactor.GetComponent<Animator>().SetTrigger(_animRestoreIn);

		UIManager.instance.ShowSpwPointMenu();
		SoundManager.instance.OnRestore();
		SaveManager.instance.SaveData();

		if (_enemies.Length == 0) return;
		foreach (var enemy in _enemies)
		{
			if (enemy.TryGetComponent(out NormalEnemy normalEnemy)) normalEnemy.ResetEnemy();
		}
	}

	public void IsSavePointOpen(bool isOpen)
	{
		if (isOpen) gameObject.SetActive(true);
		_openEffect.SetActive(isOpen);
	}

	public void Resolve()
	{
		gameObject.SetActive(true);
		if (TryGetComponent(out DissolveEffect dissolve))
		{
			StartCoroutine(dissolve.Resolve());
		}
	}
}
