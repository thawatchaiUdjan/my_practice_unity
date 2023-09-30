using System.Collections;
using PlayerController;
using UnityEngine;

public class TalismanInteract : Interactable
{
	[Header("Progress Data")]
	public float minMouseInput = 10f;
	public float delayMouseInput = 0.01f;
	public float progressPerInput = 0.01f;
	public bool isClear = false;

	private FirstPersonController _player;
	private PlayerInputControl _input;
	private SoundManager _soundManager;
	private AudioSource _sound;
	private UIManager _ui;
	//
	
	public static TalismanInteract instance;
	private void Awake() {
		instance = this;
	}

    public override void Start()
    {
        base.Start();
		_player = FirstPersonController.instance;
		_input = PlayerInputControl.instance;
		_ui = UIManager.instance;
		_soundManager = SoundManager.instance;

		_sound = GetComponent<AudioSource>();
		_sound.volume = _soundManager.AudioVolume * 0.3f;
		_sound.clip = _soundManager.paperSlide;
		
		gameObject.SetActive(false);
    }

    protected override void Interaction()
    {
        base.Interaction();
		TargetOff();
		isInteractable = false;
		_input.LockInputCanRotate();
		_ui.HideGameUI();
						
		StartCoroutine(InteractAnimate());
    }

	private void Update() {
		
	}

	public IEnumerator InteractAnimate(){
		//Move Player Position and LookAt
		var basePosition = new Vector3(transform.position.x, 0f, transform.position.z + 0.4f);
		yield return StartCoroutine(_player.MovePosition(basePosition, 3f));
		yield return StartCoroutine(_player.LookAt(transform.position, 1f, false));

		_ui.ShowHintMouse();
		
		StartCoroutine(DissolvePaper()); //Start Play mini game
	}

	public IEnumerator DissolvePaper(){
		var mate = GetComponent<Renderer>().material;
		var progress = 0f;

		while (true) 
		{	
			if (Mathf.Abs(_input.look.x) >= minMouseInput)
			{
				progress += progressPerInput;
				mate.SetFloat("_Amount", progress);
				yield return new WaitForSeconds(delayMouseInput);

				if(!_sound.isPlaying) _sound.Play();
			}

			if(progress >= 0.95f){
				yield return new WaitForSeconds(1.5f);
				_ui.ShowGameUI();
				_input.ResetLockAll();
				gameObject.SetActive(false);
				isClear = true;
				break;
			}

			yield return null;
		}
	}
	
}