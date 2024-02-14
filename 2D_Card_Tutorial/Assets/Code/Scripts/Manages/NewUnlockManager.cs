using UnityEngine;

public class NewUnlockManager : MonoBehaviour
{
	[Header("Unlock rewards")]
	[SerializeField] private PlayerCharacter _darkWitch;

	//
	private PlayerData _playerData;
	private SaveManager _save;
	private UIManager _ui;

	//StringID
	private string _strUnlockNewCharacter = "Unlock new character";

	//
	public static NewUnlockManager instance;
	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		_playerData = PlayerData.Instance;
		_save = SaveManager.instance;
		_ui = UIManager.instance;
	}

	private void NewCharacter(PlayerCharacter character)
	{
		_playerData.characters.Add(character);
		_save.SavePlayerData();
		_ui.ShowAlertBox($"{_strUnlockNewCharacter} \"{character.characterName}\"");
	}

	public void UnlockDarkWitchCharacter()
	{
		if (_playerData.difficult == DifficultType.Hard & _playerData.battleLevel == 10 & _playerData.CheckNewCharacter(_darkWitch))
		{
			NewCharacter(_darkWitch);
		}
	}
}
