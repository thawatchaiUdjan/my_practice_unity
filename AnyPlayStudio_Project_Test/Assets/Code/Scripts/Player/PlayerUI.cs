using FishNet.Object;
using UnityEngine;

public class PlayerUI : NetworkBehaviour
{
	[Header("Game UI")]
	[SerializeField] private GameObject _playerGameUI;

	[Header("Mobile Game UI")]
	[SerializeField] private GameObject _mobileGameUI;
	[SerializeField] private MobileUIButton _sprintButton;
	[SerializeField] private MobileUIButton _crouchButton;
	[SerializeField] private MobileUIButton _proneButton;
	[SerializeField] private MobileUIButton _fireButton;

	//
	private GameManager _game;
	private PlayerInputController _input;
	private PlayerController _player;

	private void Start()
	{
		_game = GameManager.instance;
		_input = GetComponent<PlayerInputController>();
		_player = GetComponent<PlayerController>();
		CheckMobilePlatform();
	}

	private void Update()
	{
		UpdateGamePadInputUI();
	}

	public override void OnStartClient()
	{
		base.OnStartClient();
		if (base.IsOwner)
		{
			_playerGameUI.SetActive(true);
		}
	}

	private void CheckMobilePlatform()
	{
		if (Application.isMobilePlatform | _game.isDebug)
		{
			ShowMobileGameUI(true);
		}
	}

	private void ShowMobileGameUI(bool isShow)
	{
		_mobileGameUI.SetActive(isShow);
	}

	public void UpdateGamePadInputUI() //Update GamePad UI if Mobile or Debug
	{
		if (!Application.isMobilePlatform & !_game.isDebug | !_playerGameUI.activeSelf) return;
		UpdateSprintButton();
		UpdateCrouchButton();
		UpdateProneButton();
		UpdateFireButton();
	}

	private void UpdateSprintButton()
	{
		_sprintButton.Show(_input.sprintAble);
		_sprintButton.Active(_player.isSprint);
	}

	private void UpdateCrouchButton()
	{
		_crouchButton.Show(_input.crouchAble);
		_crouchButton.Active(_player.isCrouch);
	}

	private void UpdateProneButton()
	{
		_proneButton.Show(_input.proneAble);
		_proneButton.Active(_player.isProne);
	}

	private void UpdateFireButton()
	{
		_fireButton.Show(_input.fireAble);
		_fireButton.Active(_input.fire);
	}
}
