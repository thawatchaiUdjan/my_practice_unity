using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PotionManager : MonoBehaviour
{
	[SerializeField] private GameObject _potionPanel;
	[SerializeField] private Image _potionCircle;
	[SerializeField] private GameObject _potionUseEffect;

	public bool isPotionUse;
	[SerializeField] private int Potions;
	public int potions
	{
		get { return Potions; }
		set 
		{
			Potions = value;
			_playerData.potions = Potions;
		}
	}
	public int maxPotion;
	public float hpGain = 30f;
	public int defaultPotion = 2;
	public float potionUseTime = 4f;
	public float _potionCircleDist = 1.1f;

	//Private members
	private Transform[] _potionSlots;
	private float potionDeltaTime;

	//
	private PlayerController _player;
	private HealthManager _playerHp;
	private PlayerData _playerData;
	private PlayerInputControl _input;
	private SoundManager _sound;

	private void StartComponents()
	{
		_playerData = PlayerData.instance;
		_input = PlayerInputControl.instance;
		_sound = SoundManager.instance;
		_player = GetComponent<PlayerController>();
		_playerHp = GetComponent<HealthManager>();
	
		//
		_potionSlots = _potionPanel.GetComponentsInChildren<Transform>(true).Skip(1).ToArray();
		_potionCircle.gameObject.SetActive(false);
	}

	public void SetupPotion(int potions, int maxPotion)
	{
		StartComponents();
		if (maxPotion != 0)
		{
			this.maxPotion = maxPotion;
			this.potions = potions;
		}
		else
		{
			this.maxPotion = defaultPotion;
			this.potions = this.maxPotion;
		}
		UpdatePotion();
	}

	void Update()
	{
		if (isPotionUse)
		{
			_potionCircle.transform.position = transform.position + Vector3.up * _potionCircleDist;
			_potionCircle.fillAmount = potionDeltaTime / potionUseTime;
			potionDeltaTime -= Time.deltaTime;
			if (potionDeltaTime <= 0f) StartCoroutine(PotionUsed());
		}
	}

	public void UpdatePotion()
	{
		for (int i = 0; i < _potionSlots.Length; i++)
		{
			if (i < maxPotion)
			{
				_potionSlots[i].gameObject.SetActive(true);
				var image = _potionSlots[i].GetComponent<Image>();
				var color = image.color;
				color.a = (i < potions) ? 1f : 0.3f;
				image.color = color;
			}
			else
			{
				_potionSlots[i].gameObject.SetActive(false);
			}
		}
	}

	public void AddPotion()
	{
		if (maxPotion >= _potionSlots.Length)
		{
			return;
		}
		maxPotion++;
		potions++;
		_playerData.maxPotion = maxPotion;
		UpdatePotion();
	}

	public void UsePotion()
	{
		if (potions <= 0)
		{
			Debug.Log("you don't have any potion!");
			return;
		}
		isPotionUse = true;
		potionDeltaTime = potionUseTime;
		_input.LockAllInput(move: true);
		_player.MoveSpeed /= 2f;
		_potionCircle.gameObject.SetActive(true);
		_sound.PlaySound(_sound.potionUsing, _sound.audioVolume * 0.3f);
	}

	private IEnumerator PotionUsed()
	{
		potions--;
		_playerHp.GetHealth(hpGain);
		StopPotionUse();
		UpdatePotion();
		_sound.OnPotionUseDone();
		_potionUseEffect.SetActive(true);
		yield return new WaitForSeconds(2);
		_potionUseEffect.SetActive(false);
	}

	public void StopPotionUse()
	{
		if (!isPotionUse) return;
		isPotionUse = false;
		potionDeltaTime = 0f;
		_player.MoveSpeed *= 2f;
		_input.ResetLockAllInput();
		_potionCircle.gameObject.SetActive(false);
		_sound.StopSound();
	}

	public void ResetPotion()
	{
		potions = maxPotion;
		UpdatePotion();
	}
}
