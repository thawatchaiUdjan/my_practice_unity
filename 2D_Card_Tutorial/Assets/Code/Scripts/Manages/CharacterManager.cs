using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
	[SerializeField] private List<PlayerCharacter> _characterList;

	[Header("Character Select")]
	[SerializeField] private Transform _selectCharacter;
	[SerializeField] private GameObject _blankCharacter;
	[SerializeField] private Button _nextButton;
	[SerializeField] private Button _previousButton;
	[SerializeField] private Button _selectButton;

	[Header("Character Info")]
	[SerializeField] private TextMeshProUGUI _name;
	[SerializeField] private TextMeshProUGUI _attack;
	[SerializeField] private TextMeshProUGUI _defense;
	[SerializeField] private TextMeshProUGUI _health;
	[SerializeField] private TextMeshProUGUI _speed;

	//
	private PlayerData _playerData;
	private MainMenuManager _mainMenu;
	private PlayerCharacter _currCharacter;
	private List<PlayerCharacter> _currCharacterList;
	private int _currIndex;

	//StringID
	private string _strUnlock = "UNLOCK BY : ";

	//
	public static CharacterManager instance;
	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		_playerData = PlayerData.Instance;
		_mainMenu = MainMenuManager.instance;
		_currCharacterList = new List<PlayerCharacter>();

		//
		SetupCharacters();
	}

	private void SetupCharacters()
	{
		var tmpCharList = new List<PlayerCharacter>(_characterList);
		foreach (var character in _playerData.characters) //Character's have
		{
			var charObj = Instantiate(character.showPrefab, _selectCharacter);
			charObj.SetActive(false);
			tmpCharList.Remove(character);
			_currCharacterList.Add(character);
		}
		foreach (var character in tmpCharList) //Character's not have
		{
			var charObj = Instantiate(_blankCharacter, _selectCharacter);
			charObj.SetActive(false);
			_currCharacterList.Add(character);
		}
		CheckContinueCharacter();
		ShowCharacter();
		UpdateUI();
	}

	private void CheckContinueCharacter()
	{
		var character = _playerData.character;
		if (character != null)
		{
			_currIndex = _playerData.characters.FindIndex((value) => value == character);
		}
	}

	private void ShowCharacter(bool isShow = true)
	{
		_selectCharacter.GetChild(_currIndex).gameObject.SetActive(isShow);
	}

	private void UpdateUI()
	{
		var isHave = _currIndex < _playerData.characters.Count; //Have Character
		_currCharacter = _currCharacterList[_currIndex];
		_nextButton.interactable = _currIndex != _currCharacterList.Count - 1;
		_previousButton.interactable = _currIndex != 0;
		_selectButton.interactable = isHave;
		ShowUnlockCondition(isHave);
		SetCharacterInfo(isHave);
	}

	private void ShowUnlockCondition(bool isHave)
	{
		if (!isHave)
		{
			_mainMenu.ShowAlertBox(_strUnlock + _currCharacter.unlockCondition);
		}
	}

	private void SetCharacterInfo(bool isHave)
	{
		if (isHave)
		{
			_name.text = _currCharacter.characterName;
			_attack.text = _currCharacter.attack.ToString();
			_defense.text = _currCharacter.defense.ToString();
			_health.text = _currCharacter.health.ToString();
			_speed.text = _currCharacter.speed.ToString();
		}
		else
		{
			_name.text = "????";
			_attack.text = "???";
			_defense.text = "???";
			_health.text = "???";
			_speed.text = "???";
		}
	}

	public CharacterSelect GetCharacterSelect()
	{
		return _selectCharacter.GetChild(_currIndex).GetComponent<CharacterSelect>();
	}

	public void OnClickNavigate(int index)
	{
		ShowCharacter(false);
		_currIndex += index;
		ShowCharacter();
		UpdateUI();
	}

	public void OnClickSelect()
	{
		_mainMenu.OnClickNewGame(_currCharacter);
	}
}
