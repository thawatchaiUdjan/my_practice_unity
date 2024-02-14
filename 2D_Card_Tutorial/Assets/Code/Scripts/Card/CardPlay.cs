using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardPlay : MonoBehaviour
{
	[SerializeField] private Image _star;

	[Header("Images")]
	[SerializeField] private Sprite _starLv1;
	[SerializeField] private Sprite _starLv2;
	[SerializeField] private Sprite _starLv3;

	[Header("Card Information")]
	public SkillType type;
	public string description;

	[Header("Card Interact")]
	public bool isLongPressAble = true;
	public bool isDragAble = true;
	public bool isOnPlay = false;

	//
	public CardInfo cardInfo;
	private bool _isMove;
	private float _moveSpeed;
	private Vector2 _targetMove;
	private Image _image;
	private CardEvent _event;
	private PlayerManager _playerManager;
	private UIManager _ui;

	private void Awake()
	{
		_event = GetComponent<CardEvent>();
		_image = GetComponentInChildren<Image>();
	}

	private void Start()
	{
		_playerManager = PlayerManager.instance;
		_ui = UIManager.instance;
	}

	private void FixedUpdate()
	{
		Move();
	}

	private void Move()
	{
		if (_isMove)
		{
			transform.position = Vector2.Lerp(transform.position, _targetMove, _moveSpeed * Time.deltaTime);
			if (Vector2.Distance(transform.position, _targetMove) <= 1f)
			{
				_isMove = false;
				transform.position = _targetMove;
			}
		}
	}

	public IEnumerator MoveCard(Vector2 targetPos, float speed)
	{
		_targetMove = targetPos;
		_moveSpeed = speed;
		_isMove = true;
		yield return new WaitUntil(() => !_isMove);
	}

	public IEnumerator SetCardInfo(CardSkill card, int level, bool isAnim)
	{
		cardInfo = new CardInfo(card, level);
		description = card.Description(level);
		type = card.skillType;
		_image.sprite = card.artwork;
		ChangeName();
		ChangeStar();
		if (isAnim) yield return StartCoroutine(_event.OnCardAdding());
	}

	public IEnumerator UseCard()
	{
		yield return StartCoroutine(_event.OnCardUse());
		_playerManager.Player.PlayerAction(cardInfo);
		yield return new WaitUntil(() => !_playerManager.Player.isAction); //Wait Until Player Finish Animate Skill
		yield return StartCoroutine(_event.OnCardUseFinish());
		Debug.Log($"Use Card [Lv.{cardInfo.level}]: {cardInfo.card.name}.");
	}

	public IEnumerator UpgradeCard()
	{
		if (CheckUpgradeCard())
		{
			cardInfo.level++;
			yield return StartCoroutine(_event.OnCardUpgrade());
			description = cardInfo.card.Description(cardInfo.level);
			ChangeName();
			Debug.Log($"Upgrade Card: {cardInfo.card.name} [Lv. {cardInfo.level - 1}] => [Lv. {cardInfo.level}].");
		}
	}

	public IEnumerator CancelUseCard()
	{
		if (cardInfo.card.skillType == SkillType.Ultimate) _playerManager.Player.CancelUseUltimate();
		yield return StartCoroutine(_event.OnCardUseFinish());
	}

	public IEnumerator CombineOnLeft()
	{
		yield return StartCoroutine(_event.OnCardCombineLeft());
	}

	public IEnumerator CombineOnRight()
	{
		yield return StartCoroutine(_event.OnCardCombineRight());
	}

	public void ChangeStar()
	{
		if (_star == null) return;
		switch (cardInfo.level)
		{
			case 1:
				_star.sprite = _starLv1;
				break;
			case 2:
				_star.sprite = _starLv2;
				break;
			case 3:
				_star.sprite = _starLv3;
				break;
		}
	}

	private void ChangeName()
	{
		name = cardInfo.card.name + $" [Lv.{cardInfo.level}]";
	}

	public void LockDragAndLongPress(bool isLock)
	{
		isLock = !isLock;
		isLongPressAble = isLock;
		isDragAble = isLock;
	}

	public void CardLongPress(bool isLongPress)
	{
		if (isLongPress)
		{
			LockDragAndLongPress(true);
			_ui.ShowCardDescription(this);
			_event.OnCardLongPress();
		}
		else
		{
			LockDragAndLongPress(false);
			_event.OnCardReleaseLongPress();
		}
	}

	private bool CheckUpgradeCard()
	{
		var isUpgrade = cardInfo.level < 3 & cardInfo.card.skillType != SkillType.Ultimate;
		return isUpgrade;
	}

	public bool CheckCombineCard(CardPlay otherCard)
	{
		var isCombine = false;
		var cardInfo = otherCard.cardInfo;

		if (cardInfo.card == this.cardInfo.card & cardInfo.level == this.cardInfo.level)
		{
			isCombine = true;
		}
		if (this.cardInfo.level >= 3 | otherCard == null)
		{
			isCombine = false;
		}
		return isCombine;
	}

	public bool CheckCancelUseCard()
	{
		var isCanUse = type == SkillType.Attack | type == SkillType.Debuff | type == SkillType.Ultimate;
		return isCanUse;
	}

}
