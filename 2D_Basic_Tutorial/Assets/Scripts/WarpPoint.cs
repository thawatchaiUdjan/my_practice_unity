using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPoint : MonoBehaviour
{
	[SerializeField] private int _sceneLoad;

	[Header("Condition Kill")]
	[SerializeField] private List<Enemy> _enemies;

	//
	private PlayerInputControl _input;
	private LoadSceneManager _scene;
	private PlayerController _player;

	private void Start()
	{
		_input = PlayerInputControl.instance;
		_scene = LoadSceneManager.instance;
	}

	private void OnTriggerEnter2D(Collider2D target)
	{
		if (target.gameObject.CompareTag("Player") & CheckCondition())
		{
			_player = target.gameObject.GetComponent<PlayerController>();
			_player.isWarp = true;
			StartCoroutine(OnWarp());
		}
	}

	private IEnumerator OnWarp()
	{
		_input.LockAllInput();
		_input.ResetAllInputVal();
		SoundManager.instance.OnWarpTravel();
		SaveManager.instance.SaveData();
		yield return StartCoroutine(_player.GetComponent<DissolveEffect>().Dissolve());
		_scene.LoadScene(_sceneLoad);
	}

	private bool CheckCondition()
	{
		var isTrue = true;
		foreach (var enemy in _enemies)
		{
			if (enemy.gameObject.activeSelf)
			{
				isTrue = false;
				break;
			}
		}
		return isTrue;
	}
}
