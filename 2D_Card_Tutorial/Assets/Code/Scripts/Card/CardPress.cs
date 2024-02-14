using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[Header("Game Object")]
	[SerializeField] private Image _longPressCircle;
	public int returnSibling;

	//
	private bool _isPress;
	private float _longPressDeltaTime;

	//
	private GameManager _game;
	private CardManager _cardManager;
	private CardPlay _card;
	private CardEvent _event;

	void Start()
	{
		_game = GameManager.instance;
		_cardManager = CardManager.instance;
		_card = GetComponent<CardPlay>();
		_event = GetComponent<CardEvent>();
	}

	private void Update()
	{
		OnLongPress();
	}

	private void OnLongPress()
	{
		if (_isPress & _cardManager.isInteractable & _card.isLongPressAble)
		{
			_longPressDeltaTime += Time.deltaTime;

			if (_longPressDeltaTime > _cardManager.pressTime)
			{
				var amount = (_longPressDeltaTime - _cardManager.pressTime) / (_cardManager.longPressTime - _cardManager.pressTime);
				_longPressCircle.fillAmount = amount;
			}

			if (_longPressDeltaTime >= _cardManager.longPressTime)
			{
				ResetPress();
				_card.CardLongPress(true);
			}
		}
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!_cardManager.isInteractable) return;
		_isPress = true;
		if (!_card.isLongPressAble)
		{
			OnPointerUp(eventData);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (!_cardManager.isInteractable) return;
		if (!_isPress | _cardManager.isMove) return;
		if (_longPressDeltaTime <= _cardManager.pressTime)
		{
			if (!_card.isOnPlay)
			{
				returnSibling = transform.GetSiblingIndex();
				_card.isOnPlay = true;
				_card.LockDragAndLongPress(true);
				_event.OnCardPlayIn();
				_game.CardPlay(transform);
			}
			else
			{
				_event.OnCardReturn();
				_game.CardReturn();
			}
		}
		ResetPress();
	}

	public void ResetPress()
	{
		_isPress = false;
		_longPressCircle.fillAmount = 0f;
		_longPressDeltaTime = 0f;
		_event.OnCardHoverDrop();
	}

}
