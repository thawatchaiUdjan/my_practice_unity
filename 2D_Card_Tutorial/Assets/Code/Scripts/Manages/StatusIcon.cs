using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class StatusIcon : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] private Image _icon;
	public int turn;

	[Header("Status Description")]
	[SerializeField] private float _offsetY = 30f;

	//
	
	private StatusInfo _status;
	private Animator _animator;	
	private GameManager _game;
	private UIManager _ui;

	//AnimID
	private string _animTurn = "Turn";

	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}

	private void Start()
	{
		_game = GameManager.instance;
		_ui = UIManager.instance;
		_game.onStatusDecreaseCallback += DecreaseTurn;
	}

	private void UpdateStatus()
	{
		_animator.SetInteger(_animTurn, turn);
		if (turn <= 0)
		{
			var statuses = new List<StatusInfo>() { _status };
			_status.target.GetStatus(statuses, isShow: false);
			Destroy(gameObject);
		}
	}

	public bool CheckReplaceStatus(StatusInfo target)
	{
		var isReplace = true;
		if (target.turn <= turn) //Lower Turn
		{
			if (target.percentStatus < _status.percentStatus) //Lower Efficiency
			{
				isReplace = false;
			}
		}
		return isReplace;
	}

	public bool CheckSameStatus(StatusInfo target)
	{
		var isSame = target.skill == _status.skill; //Same Skill
		return isSame;
	}

	public void SetupStatus(StatusInfo status)
	{
		_status = status;
		_icon.sprite = _status.statusType.icon;
		SetTurn(_status.turn);
	}

	public void SetTurn(int turn)
	{
		this.turn = turn;
		UpdateStatus();
	}

	public void IncreaseTurn()
	{
		SetTurn(turn + 1);
	}

	public void DecreaseTurn()
	{
		SetTurn(turn - 1);
	}

	public void Remove()
	{
		SetTurn(0);
	}

	public SkillType CheckSkillType()
	{
		return _status.skillType;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		var pos = Camera.main.WorldToScreenPoint(transform.position) + Vector3.up * _offsetY;
		_ui.ShowStatusDescription(_status, turn, pos);
	}

	private void OnDestroy()
	{
		_game.onStatusDecreaseCallback -= DecreaseTurn;
	}

	
}
