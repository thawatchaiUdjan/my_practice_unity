using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardEvent : MonoBehaviour, IPlaySFXEvent
{
	[SerializeField] private Color _hoverDragColor;

	//
	private SoundManager _sound;
	private CardPlay _card;
	private Animation _animation;
	private Image _image;
	private bool _isOnHover;

	//AnimID
	private string _animAddingCard = "CardAddingSlide";
	private string _animCombineLeft = "CardCombineLeft";
	private string _animCombineRight = "CardCombineRight";
	private string _animUpgradeLevel = "CardUpgradeLevel";
	private string _animPlayIn = "CardPlayIn";
	private string _animPlayOut = "CardPlayOut";
	private string _animUseCard = "CardUsing";
	private string _animUseCardFinish = "CardUsingFadeOut";
	private string _animLongPress = "CardLongPress";
	private string _animReleaseLongPress = "CardReleaseLongPress";

	void Start()
	{
		if (_animation != null) return;
		_sound = SoundManager.instance;
		_card = GetComponent<CardPlay>();
		_animation = GetComponent<Animation>();
		_image = GetComponentInChildren<Image>();
	}

	private void CheckComponent()
	{
		if (_animation == null)
		{
			Start();
		}
	}

	public void OnCardReturn()
	{
		_sound.OnPlaySFX(_sound.cardReturn);
	}

	public void OnCardLongPress()
	{
		_animation.Play(_animLongPress);
		_sound.OnPlaySFX(_sound.cardPress, 0.5f);
	}

	public void OnCardReleaseLongPress()
	{
		_animation.Play(_animReleaseLongPress);
	}

	public void OnCardHoverDrag()
	{
		_image.color = _hoverDragColor;
		_isOnHover = true;
	}

	public void OnCardHoverDrop()
	{
		if (!_isOnHover) return;
		_image.color = Color.white;
		_isOnHover = false;
	}

	public void OnCardDrag()
	{
		_sound.OnPlaySFX(_sound.cardSwap, 0.65f);
	}

	public IEnumerator OnCardAdding()
	{
		CheckComponent();
		_animation.Play(_animAddingCard);
		_sound.OnPlaySFX(_sound.cardSwap);
		yield return new WaitUntil(() => !_animation.isPlaying);
	}

	public IEnumerator OnCardCombineLeft()
	{
		CheckComponent();
		_animation.Play(_animCombineLeft);
		yield return new WaitUntil(() => !_animation.isPlaying);
	}

	public IEnumerator OnCardCombineRight()
	{
		CheckComponent();
		_animation.Play(_animCombineRight);
		yield return new WaitUntil(() => !_animation.isPlaying);
	}

	public IEnumerator OnCardUpgrade()
	{
		CheckComponent();
		_animation.Play(_animUpgradeLevel);
		yield return new WaitUntil(() => !_animation.isPlaying);
	}
	public void OnCardPlayIn()
	{
		_animation.Play(_animPlayIn);
		_sound.OnPlaySFX(_sound.cardPress);
	}
	public void OnCardPlayOut()
	{
		_animation.Play(_animPlayOut);
	}

	public IEnumerator OnCardUse()
	{
		_animation.Play(_animUseCard);
		yield return new WaitUntil(() => !_animation.isPlaying);
	}

	public IEnumerator OnCardUseFinish()
	{
		_animation.Play(_animUseCardFinish);
		yield return new WaitUntil(() => !_animation.isPlaying);
	}

	public void OnChangeStar()
	{
		_card.ChangeStar();
	}

	public void OnPlaySFX(AudioClip audio)
	{
		_sound.OnPlaySFX(audio);
	}
}
