using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	//
	private CardManager _cardManager;
	private GameManager _game;
	private CardPress _press;
	private CardPlay _card;
	private CardEvent _event;
	private CanvasGroup _canvas;
	private Transform _placeHolder;

	//
	private string _placeHolderName = "Card Place Holder";

	private void Start()
	{
		_cardManager = CardManager.instance;
		_game = GameManager.instance;
		_card = GetComponent<CardPlay>();
		_press = GetComponent<CardPress>();
		_event = GetComponent<CardEvent>();
		_canvas = GetComponent<CanvasGroup>();
	}

	private void CreatePlaceHolder()
	{
		_placeHolder = new GameObject(_placeHolderName, typeof(Image)).transform;
		var transform = _placeHolder.GetComponent<RectTransform>();
		transform.localScale = new Vector3(1f, 1f, 1f);
		transform.sizeDelta = new Vector2(_cardManager.cardSpaceDrag, transform.sizeDelta.y);
		_placeHolder.GetComponent<Image>().color = Color.clear;
		_placeHolder.SetParent(_cardManager.cardDeck);
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!_cardManager.isInteractable | !_card.isDragAble) return;
		CreatePlaceHolder();
		_press.returnSibling = transform.GetSiblingIndex();
		_canvas.blocksRaycasts = false;
		_cardManager.CardIsDrag(true, _placeHolder);
		_press.ResetPress();
		_event.OnCardHoverDrag();
		transform.SetParent(transform.parent.parent);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (!_cardManager.isInteractable | !_card.isDragAble | _placeHolder == null) return;
		transform.position = eventData.position;
		if (_cardManager.isDragOut) return;
		for (int i = 0; i < _cardManager.cardDeck.childCount; i++)
		{
			var underCard = _cardManager.cardDeck.GetChild(i);
			if (transform.position.x > underCard.position.x | i == _cardManager.cardDeck.childCount - 1)
			{
				var holderIndex = _placeHolder.GetSiblingIndex();
				var newIndex = i - 1;
				if (transform.position.x < underCard.position.x) newIndex++;
				if (holderIndex != newIndex & holderIndex != i)
				{
					if (newIndex < holderIndex) newIndex++; //For Slide Card from Left->Right
					_placeHolder.SetSiblingIndex(newIndex);
					_event.OnCardDrag();
				}
				break;
			}
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (!_cardManager.isInteractable | !_card.isDragAble | _placeHolder == null) return;
		transform.SetParent(_cardManager.cardDeck);
		_event.OnCardHoverDrop();
		_canvas.blocksRaycasts = true;
		if (!_cardManager.isDragOut)
		{
			transform.SetSiblingIndex(_placeHolder.GetSiblingIndex());
		}
		else
		{
			transform.SetSiblingIndex(_press.returnSibling);
		}

		_cardManager.CardIsDrag(false);

		//New Position of Drag
		if (transform.GetSiblingIndex() != _press.returnSibling)
		{
			_game.CardPlay(transform, isMoveCard: true);
			return;
		}
	}

}
