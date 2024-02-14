using UnityEngine;
using UnityEngine.EventSystems;

public class CardDrop : MonoBehaviour,IPointerEnterHandler , IPointerExitHandler
{
	//
	private CardManager _cardManager;

	void Start()
	{
		_cardManager = CardManager.instance;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (_cardManager.isDrag & _cardManager.isDragOut)
		{
			_cardManager.CardDragOut(false);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{	
		if (_cardManager.isDrag & !_cardManager.isDragOut)
		{
			_cardManager.CardDragOut(true);
		}
	}

	
}
