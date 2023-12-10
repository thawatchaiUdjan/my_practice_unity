using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	[SerializeField] private GameObject _gameUI;
	[SerializeField] private GameObject _gamePauseMenu;
	[SerializeField] private TextMeshProUGUI _scoreText;

	[Header("Game Over UI")]
	[SerializeField] private GameObject _gameOverUI;
	[SerializeField] private GameObject _starGroup;
	[SerializeField] private Sprite _star;
	[SerializeField] private Sprite _starEmpty;

	//AnimID
	private string _animStar = "StarGetting";
	private string _animEmpty = "StarEmpty";

	//
	private GameManager _game;
	private SoundManager _sound;

	//
	public static UIManager instance;
	private void Awake()
	{
		instance = this;
	}

	void Start()
	{
		_game = GameManager.instance;
		_sound = SoundManager.instance;
		_game.onScoreChangeCallBack += UpdateScore;

		UpdateScore();
	}

	public void ShowGameUI(bool isShow)
	{
		_gameUI.SetActive(isShow);
	}

	public void ShowGamePauseUI(bool isShow)
	{
		_gamePauseMenu.SetActive(isShow);
	}

	public void ShowGameOverUI(bool isShow, int star)
	{
		_gameOverUI.SetActive(isShow);
		_sound.OnPlaySFX(_sound.gameOverOpen, 0.2f);
		StartCoroutine(StarGetting(star));
	}

	private IEnumerator StarGetting(int star)
	{
		var starImg = _starGroup.GetComponentsInChildren<Image>(true);

		for (int i = 0; i < 3; i++)
		{
			var sprite = _starEmpty;
			var anim = _animEmpty;
			var starAnim = starImg[i].GetComponent<Animation>();
			var volume = 0.25f;

			if (i < star)
			{
				sprite = _star;
				anim = _animStar;
				volume = 0.55f;
			}

			starImg[i].sprite = sprite;
			starImg[i].gameObject.SetActive(true);
			starAnim.Play(anim);
			_sound.OnPlaySFX(_sound.starGetting, volume);
			yield return new WaitUntil(() => !starAnim.isPlaying);
		}
	}

	private void UpdateScore()
	{
		_scoreText.text = "X " + _game.score;
	}
}
