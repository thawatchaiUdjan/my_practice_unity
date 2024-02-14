using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoulManager : MonoBehaviour
{
	[SerializeField] private GameObject _soulItem;
	[SerializeField] private TextMeshProUGUI _soulText;
	[SerializeField] private TextMeshProUGUI _soulGetText;
	[SerializeField] private int Soul;
	public int soul
	{
		get => Soul;
		set
		{
			Soul = value;
			_playerData.soul = Soul;
		}
	}
	public int maxSoul = 9999;
	public int countSpeed = 100;
	public float delaySoulGet = 0.5f;

	[Header("Soul Drop System")]
	public int minNumDrop = 3;
	public int maxNumDrop = 4;
	public int soulRandRang = 8;
	public int forceY = 180;
	public int forceX = 120;

	//Private members
	private Queue<int> soulGetQueue = new Queue<int>();
	private Coroutine soulGet;
	private float refVelcntSoul;
	private float currSoul;

	//
	private LoadSceneManager _scene;
	private PlayerData _playerData;
	private SoundManager _sound;
	private Animation _textAnimation;
	private Animation _textGetAnimation;

	private void Start()
	{
		if (_scene != null) return;
		_scene = LoadSceneManager.instance;
		_playerData = PlayerData.instance;
		_sound = SoundManager.instance;
		_textAnimation = _soulText.GetComponent<Animation>();
		_textGetAnimation = _soulGetText.GetComponent<Animation>();
	}

	void Update()
	{
		UpdateSoul();
		CheckSoulGet();

		if (Input.GetKeyDown(KeyCode.R) & _scene.isDebug)
		{
			GetSoul(100);
		}
	}

	public void SetupSoul(int soul)
	{
		Start();
		if (soul != 0f)
		{
			currSoul = soul;
			this.soul = (int)currSoul;
		}
	}

	private void UpdateSoul()
	{
		currSoul = Mathf.SmoothDamp(currSoul, soul, ref refVelcntSoul, countSpeed * Time.deltaTime);
		_soulText.text = Mathf.Round(currSoul).ToString();
	}

	private void CheckSoulGet()
	{
		if (soulGetQueue.Count > 0 & soulGet == null)
		{
			soulGet = StartCoroutine(SoulGet(soulGetQueue.Dequeue()));
		}
	}

	public void GetSoul(int value)
	{
		soul += value;
		if (soul >= maxSoul) soul = maxSoul;
		soulGetQueue.Enqueue(value);
	}

	private IEnumerator SoulGet(int value)
	{
		_textAnimation.Play();
		_soulGetText.text = $"+{value}";
		_textGetAnimation.Stop();
		_textGetAnimation.Play();
		_sound.OnSoulGet();
		yield return new WaitForSeconds(delaySoulGet);
		soulGet = null;
	}

	public void UseSoul(int value)
	{
		if (value > soul)
		{
			Debug.Log("Can't use soul.");
			return;
		}
		soul -= value;
	}

	public void DropSoul(Vector2 pos, int soulVal)
	{
		int newForceX = 0, totalSoul = 0;
		int numOfSoul = Random.Range(minNumDrop, maxNumDrop + 1);
		int soulPerOne = Mathf.FloorToInt(soulVal / numOfSoul);

		for (int i = 0; i < numOfSoul; i++)
		{
			var soulObj = Instantiate(_soulItem, pos, _soulItem.transform.rotation);
			var soul = Random.Range(soulPerOne - soulRandRang, soulPerOne + soulRandRang);

			totalSoul += soul;
			newForceX = RandomForceX(newForceX);

			soulObj.GetComponent<Rigidbody2D>().AddForce(new Vector2(newForceX, forceY));
			soulObj.GetComponent<SoulScript>()
			.SetSoul(i == numOfSoul - 1 ? soul + (soulVal - totalSoul) : soul);
		}
	}

	private int RandomForceX(int newForceX)
	{
		var min = -forceX;
		var max = forceX;

		if (newForceX > 0) max = 0;
		else if (newForceX < 0) min = 0;

		newForceX = Random.Range(min, max);
		return newForceX;
	}

}
