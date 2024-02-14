using System.Collections.Generic;
using UnityEngine;

public class CardBattleManager : MonoBehaviour
{
	[SerializeField] private List<CardBattleStatus> cardBattles;
	[SerializeField] private GameObject _continueButtonMenu;

	//
	private CardBattle _cardBattle;
	private GameManager _game;
	private PlayerManager _playerManager;

	public delegate void OnCardConfirmSelectCallBack(CardBattle card, CardBattleStatus cardBattle);
	public OnCardConfirmSelectCallBack onCardConfirmSelectCallBack;

	public delegate void OnCardSelectedCallBack(CardBattle card);
	public OnCardSelectedCallBack onCardSelectedCallBack;

	//
	public static CardBattleManager instance;
	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		_game = GameManager.instance;
		_playerManager = PlayerManager.instance;
	}

	private CardBattleStatus RandomCardBattle()
	{
		var index = Random.Range(0, cardBattles.Count);
		return cardBattles[index];
	}

	public void SelectedCard(CardBattle cardBattle)
	{
		_cardBattle = cardBattle;
	}

	public void ShowContinueMenu(bool isShow)
	{
		_continueButtonMenu.SetActive(isShow);
	}

	public void OnClickConfirmSelectCard() //OnClick Confirm Select Card
	{
		var cardBattleStatus = RandomCardBattle();
		var playerStatus = _playerManager.Player.GetComponent<PlayerStatus>();
		playerStatus.GetCardBattle(cardBattleStatus);
		_game.SavePlayerData();
		onCardConfirmSelectCallBack?.Invoke(_cardBattle, cardBattleStatus);
	}



}
