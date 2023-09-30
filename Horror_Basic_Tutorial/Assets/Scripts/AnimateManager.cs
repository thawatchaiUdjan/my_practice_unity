using System.Collections;
using System.Collections.Generic;
using PlayerController;
using UnityEngine;

public class AnimateManager : MonoBehaviour
{
	public GameObject ghostGameOver;
	public GameObject ghostGameClear_1;
	
	[Header("Text Assets")]
	[SerializeField] private TextAsset _onGameClear_2;

	private GameManager _game;
	private FirstPersonController _player;
	private GhostManager _ghost;
	private ObjectiveManager _objective;
	private UIManager _ui;
	private DialogueManager _dialogue;
	private SoundManager _sound;
	private FlashLightManager _flashLight;
	private GamePauseManager _gamePause;

	//String Game
	private string _txtHeaderClear = "Game Clear";
	private string _txtHeaderOver = "Game Over";
	private string _txtClearGame = $"you clear game in";

	private void Start() 
	{
		_game = GameManager.instance;
		_player = FirstPersonController.instance;
		_ghost = GhostManager.instance;
		_objective = ObjectiveManager.instance;
		_ui = UIManager.instance;
		_dialogue = DialogueManager.instance;
		_flashLight = FlashLightManager.instance;
		_gamePause = GamePauseManager.instance;
		_sound = SoundManager.instance;
	}
    
	public void OnGameClear_1(){
		var tf = ghostGameClear_1.transform;
		var speed = 0.01f;
		_flashLight.TurnOnLight();

		if (_game.gameClearType == 1)
		{
			ghostGameClear_1.SetActive(true);
			tf.GetComponent<Animator>().Play(GhostTypeManager.Anim.Crying.ToString());
			var sound = tf.GetComponent<AudioSource>();
			sound.clip = _sound.cryingGhost;
			sound.volume = _sound.AudioVolume * 0.3f;
			sound.Play();
		}

		StartCoroutine(_player.LookAt(tf.position, speed));
	}

	public void OnGameClear_2(){
		var speed = 0.05f;
		StartCoroutine(_player.LookAt(_player.transform.position + -Vector3.forward, speed));
	}

	public void OnGameClear_3(){
		ghostGameClear_1.SetActive(false);
		_ui.GameClearFade();
	}

	public void OnGameClear_4(){
		ghostGameClear_1.GetComponent<AudioSource>().Stop();

		if (_game.gameClearType == 1){
			_sound.OnGameClear_1();
		}
		else if (_game.gameClearType == 2){
			_dialogue.OnGameClear_2(_onGameClear_2.text);
			_sound.OnGameClear_2();
		}
		
	}

	public void OnSetGameMenu(){
		_gamePause.PauseGame();
		var textHeader = _txtHeaderOver;
		var txtClearGame = "";

		if (_game.gameClearType != 0)
		{
			textHeader = _txtHeaderClear;
			var minute = Mathf.FloorToInt(_game.gamePlayTime / 60);
			var second = Mathf.FloorToInt(_game.gamePlayTime % 60);
			var txtClearTime = _txtClearGame 
				+ string.Format(" {0:00} minute {1:00} second. ", minute, second);
			var txtClearObject = $"[{_objective.numOfObjectives} Objects]";

			txtClearGame = txtClearTime + txtClearObject;
		}
		_ui.SetGameMenu(textHeader, txtClearGame);
	}

	public void OnGameOver_1(){
		_ghost.ResetGhost();
		_flashLight.TurnOffLight();
		ghostGameOver.SetActive(true);
		AudioSource.PlayClipAtPoint(_sound.screamSound, ghostGameOver.transform.position, _sound.AudioVolume);
	}
}
