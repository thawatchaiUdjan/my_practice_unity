using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardMoving : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] private Sprite _movingCard;

	//
	private GameManager _game;
	private CardManager _cardManager;
	private SoundManager _sound;
	private Animation _animation;
	private Image _image;

	//AnimID
	private string _animCardFlip = "CardMovingFlip";
	private string _animCardFadeOut = "CardMovingFadeOut";

	private void Awake()
	{
		_game = GameManager.instance;
		_cardManager = CardManager.instance;
		_sound = SoundManager.instance;
		_animation = GetComponent<Animation>();
		_image = GetComponentInChildren<Image>();
	}

	private void OnEnable()
	{
		OnCardMove();
	}

	public void OnCardMove()
	{
		_animation.Play(_animCardFlip);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!_cardManager.isInteractable) return;
		_sound.OnPlaySFX(_sound.cardReturn);
		_game.CardReturn();
	}

	public IEnumerator OnCardUse()
	{
		transform.SetParent(transform.parent.parent);
		_animation.Play(_animCardFadeOut);
		yield return new WaitUntil(() => !_animation.isPlaying);
		Destroy(gameObject);
	}

	//Anim Events
	public void OnChangeImage()
	{
		_image.sprite = _movingCard;
	}
}
