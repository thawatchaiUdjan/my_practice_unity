using TMPro;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _highScoreText;
	private SaveManager _save;
	private PlayerData _playerData;
	private SoundManager _sound;

	private void Start()
	{
		_save = SaveManager.instance;
		_playerData = PlayerData.Instance;
		_sound = SoundManager.instance;

		_save.LoadPlayerData();
		_sound.OnPlayBGM(clip: _sound.BGM_MainMenu);

		_highScoreText.text = "High Score : " + _playerData.score;
	}

	public void OnClickQuit()
	{
		Application.Quit();
	}
}
