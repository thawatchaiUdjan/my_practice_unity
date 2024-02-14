using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, IPlaySFXEvent
{
	[Header("UI Objects")]
	[SerializeField] private GameObject _alertBox;
	[SerializeField] private GameObject _gameUI;
	[SerializeField] private GameObject _cardDeckSlot;
	[SerializeField] private GameObject _cardPlaySlot;
	[SerializeField] private GameObject _cardUseSlot;
	[SerializeField] private GameObject _statusDescription;
	[SerializeField] private GameObject _backgroundBlock;
	public GameObject gameStartUI;
	public GameObject gameBossUI;
	public GameObject gameClearUI;
	public GameObject gameOverUI;
	public GameObject totalDamageFloatingText;
	public GameObject damageFloatingText;
	public GameObject healingFloatingText;
	public GameObject evadeFloatingText;
	public GameObject statusBuffFloating;
	public GameObject statusDebuffFloating;
	public GameObject statusBuffIcon;
	public GameObject statusDebuffIcon;
	public GameObject ultimateIcon;

	[Header("Card Description")]
	[SerializeField] private GameObject _cardDescription;
	[SerializeField] private GameObject _descriptionBox;

	[Header("Battle Status Setting")]
	[SerializeField] private TextMeshProUGUI _attackUpText;
	[SerializeField] private TextMeshProUGUI _defenseUpText;
	[SerializeField] private TextMeshProUGUI _healthUpText;

	[Header("UI Settings")]
	public float cardDesOffsetY = 210f;
	public float cardDesOffsetX = 50f;
	public float delayFloatingText = 1f;
	public float delayTotalFloatingText = 1.5f;

	//
	private CardPlay _cardPlay;
	private PlayerData _playerData;
	private GameManager _game;
	private SoundManager _sound;

	//AnimID
	private string _animCardDeckIn = "CardDeckSlideIn";
	private string _animCardDeckOut = "CardDeckSlideOut";
	private string _animCardPlayIn = "CardPlaySlideIn";
	private string _animCardPlayOut = "CardPlaySlideOut";
	private string _animFloatingTextOut = "FloatingTextOut";

	//
	public static UIManager instance;
	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		_playerData = PlayerData.Instance;
		_game = GameManager.instance;
		_sound = SoundManager.instance;
	}

	public void ShowUI(GameObject gameObject, bool isShow, bool isAnim = false, bool isActive = false)
	{
		gameObject.SetActive(isShow);
		if (isAnim) StartCoroutine(PlayAnim(gameObject, isActive));
	}

	private IEnumerator PlayAnim(GameObject gameObject, bool isActive, string animID = null, float delay = 0f, bool isDestroy = false)
	{
		var anim = gameObject.GetComponent<Animation>();
		gameObject.SetActive(true);
		yield return new WaitForSeconds(delay);
		if (animID != null) anim.Play(animID);
		yield return new WaitUntil(() => !anim.isPlaying);
		gameObject.SetActive(isActive);
		if (isDestroy) Destroy(gameObject);
	}

	public void ShowBackgroundBlock(bool isShow)
	{
		ShowUI(_backgroundBlock, isShow);
	}

	public IEnumerator ShowCardDeck(bool isShow = true)
	{
		var animIdDeck = isShow ? _animCardDeckIn : _animCardDeckOut;
		var animIdPlaySlot = isShow ? _animCardPlayIn : _animCardPlayOut;
		StartCoroutine(PlayAnim(_cardPlaySlot, isShow, animIdPlaySlot));
		yield return StartCoroutine(PlayAnim(_cardDeckSlot, isShow, animIdDeck));
	}

	public void ShowCardDescription(CardPlay card)
	{
		var pos = new Vector2(card.transform.position.x - cardDesOffsetX, card.transform.position.y + cardDesOffsetY);
		var texts = _descriptionBox.GetComponentsInChildren<TextMeshProUGUI>();
		_cardDescription.SetActive(true);
		_cardPlay = card;
		texts[0].text = card.name;
		texts[1].text = card.description;
		LayoutRebuilder.ForceRebuildLayoutImmediate(_descriptionBox.GetComponent<RectTransform>());
		_descriptionBox.transform.position = pos;
	}

	public void ShowStatusDescription(StatusInfo status, int turn, Vector2 pos)
	{
		_statusDescription.SetActive(true);
		_statusDescription.GetComponent<StatusDescription>().SetupData(status, turn, pos);
	}

	public void ShowFloatingText(GameObject gameObject, Transform transform, string text = null)
	{
		var floatingText = Instantiate(gameObject, transform);
		var textMesh = floatingText.GetComponentInChildren<TextMeshProUGUI>();
		if (text != null) textMesh.text = text;
		StartCoroutine(PlayAnim(floatingText, false, _animFloatingTextOut, delayFloatingText, true));
	}

	public void ShowFloatingStatus(GameObject gameObject, Transform transform, StatusType status)
	{
		var floatingText = Instantiate(gameObject, transform);
		floatingText.GetComponent<StatusFloating>().SetupData(status);
		StartCoroutine(PlayAnim(floatingText, false, isDestroy: true));
	}

	public void ShowFloatingTotalDamage(Transform transform, string text)
	{
		var floatingText = Instantiate(totalDamageFloatingText, transform);
		floatingText.GetComponent<TotalDamageFloating>().SetupData(text);
		StartCoroutine(PlayAnim(floatingText, false, _animFloatingTextOut, delayTotalFloatingText, true));
	}

	public void ShowStartGameUI()
	{
		if (_game.isBossBattle) return;
		ShowUI(gameStartUI, true, isAnim: true);
		_sound.OnPlaySFX(_sound.battleStart);
	}

	public void ShowStartBossUI()
	{
		ShowUI(gameBossUI, true, isAnim: true);
		_sound.OnPlaySFX(_sound.battleBossStart, 1.5f);
	}

	public void ShowAlertBox(string text, float delay = 0)
	{
		var alert = _alertBox.GetComponent<AlertBox>();
		alert.Show(text, delay);
	}

	public void ShowGameUI(bool isShow)
	{
		ShowUI(_gameUI, isShow);
	}

	public void UpdateBattleStatusUI()
	{
		Start();
		var syntax = "@value";
		var text = $"+{syntax}%";
		_attackUpText.text = text.Replace(syntax, _playerData.attackUp.ToString());
		_defenseUpText.text = text.Replace(syntax, _playerData.defenseUp.ToString());
		_healthUpText.text = text.Replace(syntax, _playerData.healthUp.ToString());
	}

	public void OnClickSaveData() //OnClick SaveData
	{
		_game.SavePlayerData();
	}

	public void OnClickHideCardDescription() //OnClick Card Description
	{
		_cardDescription.SetActive(false);
		_cardPlay.CardLongPress(false);
	}

	public void OnPlaySFX(AudioClip audio)
	{
		_sound.OnPlaySFX(audio);
	}

}
