using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardBattle : MonoBehaviour, IPlaySFXEvent
{
	[SerializeField] private Image _card;
	[SerializeField] private Image _icon;
	[SerializeField] private TextMeshProUGUI _text;
	[SerializeField] private Sprite _cardBattleFont;

	[Header("Settings")]
	[SerializeField] private float _moveSpeed = 7f;

	//
	private bool _isSelected;
	private bool _isMove;
	private Vector2 _targetPos;
	private Sprite _originalCard;
	private CardBattleManager _cardBattleManager;
	private SoundManager _sound;
	private CardBattleStatus _cardBattle;
	private Animation _animation;
	private Button _button;

	//AnimID
	private string _animCardDeselect = "CardBattleDeselect";
	private string _animCardSelect = "CardBattleSelect";
	private string _animCardFlip = "CardBattleFlip";
	private string _animCardOut = "CardBattleOut";

	private void OnEnable()
	{
		if (_originalCard == null) _originalCard = _card.sprite;
		_card.sprite = _originalCard;
		_isSelected = false;
	}

	private void Start()
	{
		_cardBattleManager = CardBattleManager.instance;
		_sound = SoundManager.instance;
		_animation = GetComponent<Animation>();
		_button = GetComponent<Button>();
		_button.onClick.AddListener(OnClick);
		_cardBattleManager.onCardConfirmSelectCallBack += CardConfirmSelect;
		_cardBattleManager.onCardSelectedCallBack += CardSelected;
	}

	private void FixedUpdate()
	{
		if (_isMove)
		{
			_card.transform.position = Vector2.Lerp(_card.transform.position, _targetPos, _moveSpeed * Time.deltaTime);
			if (Vector2.Distance(_card.transform.position, _targetPos) < 0.1f)
			{
				_card.transform.position = _targetPos;
				_isMove = false;
			}
		}
	}

	private IEnumerator MoveCard(Vector2 targetPos)
	{
		_targetPos = targetPos;
		_isMove = true;
		yield return new WaitUntil(() => !_isMove);
	}

	private string GetDescription()
	{
		var description = $"<color=#E6A540>+{_cardBattle.percentStatus}%</color=#E6A540> {_cardBattle.buffType}";
		return description;
	}

	private void CardConfirmSelect(CardBattle card, CardBattleStatus cardBattle)
	{
		_button.interactable = false;
		if (card == this)
		{
			_cardBattle = cardBattle;
			StartCoroutine(CardOpenConfirm());
		}
		else
		{
			_animation.Play(_animCardOut);
		}
	}

	private IEnumerator CardOpenConfirm()
	{
		yield return new WaitForSeconds(1.2f);
		yield return StartCoroutine(MoveCard(transform.parent.position));
		_animation.Play(_animCardFlip);
		yield return new WaitForSeconds(1f);
		_cardBattleManager.ShowContinueMenu(true);
	}

	private void CardSelected(CardBattle card)
	{
		if (card == this & !_isSelected)
		{
			_animation.Play(_animCardSelect);
			_isSelected = true;
		}
		else if (card != this & _isSelected)
		{
			_animation.Play(_animCardDeselect);
			_isSelected = false;
		}
	}

	private void OnClick() //On Button Click
	{
		_cardBattleManager.SelectedCard(this);
		_cardBattleManager.onCardSelectedCallBack?.Invoke(this);
	}

	public void OnChangeCard() //On Animation Event 'CardFlip'
	{
		_card.sprite = _cardBattleFont;
		_icon.sprite = _cardBattle.icon;
		_text.text = GetDescription();

		_icon.gameObject.SetActive(true);
		_text.gameObject.SetActive(true);
	}

	public void OnPlaySFX(AudioClip audio)
	{
		_sound.OnPlaySFX(audio);
	}
}
