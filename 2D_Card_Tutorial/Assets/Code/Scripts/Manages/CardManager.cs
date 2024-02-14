using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
	[Header("Card Object")]
	[SerializeField] private GameObject _cardNormal;
	[SerializeField] private GameObject _cardUltimate;
	[SerializeField] private GameObject _cardMove;

	[Header("Card Setting")]
	public Transform cardDeck;
	public Transform cardPlaySlot;
	public Transform cardUseSlot;
	public bool isInteractable;
	public bool isMove;
	public bool isDrag;
	public bool isDragOut;
	public float pressTime = 0.3f;
	public float longPressTime = 0.7f;
	public float cardSpaceDrag = 80f;
	public float cardUseDelay = 0.7f;
	public float cardMoveSpeed = 12f;
	public float cardSlideSpeed = 6f;

	//
	private int _playSlot;
	private Transform _cardPlaceHolder;
	private CardSkill _cardSkillUltimate;
	private List<CardSkill> _cardsSkill;
	private List<List<CardInfo>> _cardsSave;

	//
	private GameManager _game;
	private PlayerManager _playerManager;
	private EnemyManager _enemyManager;
	private PlayerData _playerData;
	private UIManager _ui;

	//
	public static CardManager instance;
	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		if (_game != null) return;
		_game = GameManager.instance;
		_playerManager = PlayerManager.instance;
		_enemyManager = EnemyManager.instance;
		_playerData = PlayerData.Instance;
		_ui = UIManager.instance;
		_game.onSaveDataCallback += SaveData;

		//
		_playSlot = cardPlaySlot.childCount;
		_cardsSave = new List<List<CardInfo>>();
	}

	public void SaveData()
	{
		_cardsSave.Clear();
		CardSaveMove();
		_playerData.cards = _cardsSave[0];
	}

	public void SetupCard()
	{
		Start();
		_cardsSkill = _playerData.character.cards;
		_cardSkillUltimate = _playerData.character.ultimateCard;
	}

	public void GettingCard()
	{
		StartCoroutine(InitialCard());
	}

	private IEnumerator InitialCard()
	{
		var firstCardIndex = 0;
		isInteractable = false;
		_cardsSave.Clear();
		_ui.ShowBackgroundBlock(true);
		yield return StartCoroutine(_ui.ShowCardDeck(true));
		while (cardDeck.childCount < _game.limitCard) //Adding Card
		{
			if (_playerData.cards.Count != 0 & _game.isLoadCard) //Check For Load Cards Data 
			{
				foreach (var card in _playerData.cards)
				{
					yield return StartCoroutine(CardAdd(card.card, card.level));
				}
				_game.isLoadCard = false;
			}
			else if (_playerManager.Player.isUltimate & !_playerManager.Player.isUltimateGet) //Check For Ultimate Card
			{
				yield return StartCoroutine(CardAdd(_cardSkillUltimate));
				_playerManager.Player.isUltimateGet = true;
			}
			else //Normal Add Card
			{
				if (_game.isGameStart) //Normal Add With 'Fixed Card'
				{
					yield return StartCoroutine(CardAdd(_cardsSkill[firstCardIndex]));
					firstCardIndex++;
					if (firstCardIndex >= _cardsSkill.Count) firstCardIndex = 0;
				}
				else //Normal Add With 'Random Card'
				{
					yield return StartCoroutine(CardAdd()); 
				}
			}
		}
		if (_game.isGameStart) //If First Round
		{
			_ui.ShowStartGameUI();
			_game.isGameStart = false;
			_game.isLoadCard = false;
		}
		_ui.ShowBackgroundBlock(false);
		CardSaveMove();
		isInteractable = true;
	}

	private IEnumerator CardAdd(CardSkill card = null, int level = 1, bool isAnim = true, int sibling = -1)
	{
		var randCard = Random.Range(0, _cardsSkill.Count);
		var cardTarget = card == null ? _cardsSkill[randCard] : card;
		yield return StartCoroutine(CreateCardSkill(cardTarget, level, isAnim, sibling));
		yield return StartCoroutine(CardCombineCheck());
	}

	private IEnumerator CreateCardSkill(CardSkill card, int level, bool isAnim, int sibling)
	{
		var cardTarget = card.skillType == SkillType.Ultimate ? _cardUltimate : _cardNormal;
		var newCard = Instantiate(cardTarget, cardDeck).GetComponent<CardPlay>();
		if (sibling != -1) newCard.transform.SetSiblingIndex(sibling);
		yield return StartCoroutine(newCard.SetCardInfo(card, level, isAnim));
	}

	private IEnumerator CardCombineCheck()
	{
		yield return new WaitForSeconds(0.1f);
		for (int i = 0; i < cardDeck.childCount - 1; i++)
		{
			var currCard = cardDeck.GetChild(i).GetComponent<CardPlay>();
			var nextCard = cardDeck.GetChild(i + 1).GetComponent<CardPlay>();
			if (currCard.CheckCombineCard(nextCard))
			{
				yield return StartCoroutine(CardCombining(currCard, nextCard));
				i = -1; //Start Check again
			}
		}
	}

	private IEnumerator CardCombining(CardPlay currCard, CardPlay nextCard)
	{
		StartCoroutine(nextCard.CombineOnRight());
		yield return StartCoroutine(currCard.CombineOnLeft());
		Destroy(nextCard.gameObject);
		yield return StartCoroutine(currCard.UpgradeCard());
	}

	private IEnumerator CardMove(Transform card, Transform target, bool isParent = true,
	bool isSibling = true, bool isOutParent = true, float speed = 0f)
	{
		isMove = true;
		var cardPlay = card.GetComponent<CardPlay>();
		var targetSpeed = speed == 0f ? cardMoveSpeed : speed;
		if (isOutParent) card.SetParent(card.parent.parent);
		yield return StartCoroutine(cardPlay.MoveCard(target.position, targetSpeed));
		if (isParent) card.SetParent(target.parent);
		if (isSibling) card.SetSiblingIndex(target.GetSiblingIndex());
		isMove = false;
	}

	private void CardSaveMove()
	{
		var cards = new List<CardInfo>();
		for (int i = 0; i < cardDeck.childCount; i++)
		{
			var card = new CardInfo(cardDeck.GetChild(i).GetComponent<CardPlay>().cardInfo);
			cards.Add(card);
		}
		_cardsSave.Add(cards);
	}

	public IEnumerator CardSaveReturn() //Save Card
	{
		var cards = _cardsSave[_cardsSave.Count - 2];
		for (int i = 0; i < cards.Count; i++)
		{
			var card = cards[i];
			if (i < cardDeck.childCount)
			{
				var currCardObject = cardDeck.GetChild(i).gameObject;
				var currCard = currCardObject.GetComponent<CardPlay>().cardInfo;
				if (currCard == card) continue;
				else Destroy(currCardObject);
			}
			yield return StartCoroutine(CreateCardSkill(card.card, card.level, false, i));
		}

		_cardsSave.RemoveAt(_cardsSave.Count - 1);
		Destroy(cardPlaySlot.GetChild(cardPlaySlot.childCount - 1).gameObject);
	}

	public IEnumerator CardPlaySlot(Transform card, bool isMoveCard) //Move Card to 'Play Slot'
	{
		isInteractable = false;
		var target = cardPlaySlot.GetChild(cardPlaySlot.childCount - _playSlot);
		
		if (isMoveCard) //If Move Card
		{
			card = Instantiate(_cardMove, cardPlaySlot).transform; //Create Place holder
			card.position = target.position;
			card.SetParent(cardPlaySlot);
		}
		else //If Use Card
		{
			StartCoroutine(CardMove(card, target, isSibling: false));
		}
		yield return StartCoroutine(CardCombineCheck());
		CardSaveMove();
		yield return new WaitUntil(() => !isMove);

		if (cardPlaySlot.childCount >= _playSlot * 2 | cardDeck.childCount == 0) //If Already use card
		{
			yield return new WaitForSeconds(0.4f);
			StartCoroutine(CardUseSlot());
		}
		else
		{
			isInteractable = true;
		}
	}

	private IEnumerator CardUseSlot() //Move Card to 'Use Slot'
	{
		var index = 0;
		var cardsPlay = cardPlaySlot.GetComponentsInChildren<CardPlay>();
		var cardUseCount = cardUseSlot.childCount + cardsPlay.Length;
		var cardCount = cardPlaySlot.childCount - _playSlot;
		_ui.ShowBackgroundBlock(true);
		for (int i = 0; i < cardCount; i++)
		{
			var card = cardPlaySlot.GetChild(_playSlot);
			if (card.GetComponent<CardPlay>() != null)
			{
				var target = cardUseSlot.GetChild(index++);
				card.GetComponent<CardEvent>().OnCardPlayOut();
				StartCoroutine(CardMove(card, target, true, false, speed: cardMoveSpeed * 0.5f));
				yield return new WaitForSeconds(0.1f); //Delay for each Card move to 'Use Slot'
			}
			else if (card.TryGetComponent(out CardMoving cardMoving))
			{
				StartCoroutine(cardMoving.OnCardUse());
				if (i >= cardCount - 1)
				{
					yield return StartCoroutine(cardMoving.OnCardUse());
				}
			}
		}
		StartCoroutine(_ui.ShowCardDeck(false));
		yield return new WaitUntil(() => cardUseSlot.childCount == cardUseCount);
		StartCoroutine(CardUsing());
	}

	private IEnumerator CardUsing() //Use Card in 'Use Slot'
	{
		var cards = cardUseSlot.GetComponentsInChildren<CardPlay>();
		var isUseCard = true;
		for (int i = 0; i < cards.Length; i++)
		{
			if (_enemyManager.Enemy.isDead & cards[i].CheckCancelUseCard()) //Check Enemy Dead to cancel use card
			{
				yield return StartCoroutine(cards[i].CancelUseCard());
				isUseCard = false;
			}
			else
			{
				yield return StartCoroutine(cards[i].UseCard()); //Use Card.
			}

			for (int j = 0; j < cards.Length - i - 1; j++) //Move Card closer.
			{
				StartCoroutine(CardMove(cards[i + j + 1].transform, cardUseSlot.GetChild(j),
				false, false, false, speed: cardSlideSpeed));
				yield return new WaitForSeconds(0.1f); //Delay for each 'Card closer'
			}

			Destroy(cards[i].gameObject);
			if (isUseCard) yield return new WaitForSeconds(cardUseDelay); //Delay for each 'Card use'
			yield return new WaitUntil(() => !isMove);
		}
		_game.NextTurn();
	}

	public void CardIsDrag(bool isDrag, Transform placeHolder = null)
	{
		this.isDrag = isDrag;
		if (this.isDrag & placeHolder != null)
		{
			_cardPlaceHolder = placeHolder;
		}
		else
		{
			Destroy(_cardPlaceHolder.gameObject);
		}
	}

	public void CardDragOut(bool isOut)
	{
		isDragOut = isOut;
		if (isDragOut & _cardPlaceHolder != null)
		{
			_cardPlaceHolder.SetAsLastSibling();
		}
	}

}
