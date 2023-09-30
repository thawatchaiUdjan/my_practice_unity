using PlayerController;
using UnityEngine;

public class ReadNoteInteract : Interactable
{
	[SerializeField] private TextAsset _textNote;
	[SerializeField] private TextAsset _onLastNoteRead;

	private bool _isOpen;
	private bool _isLastNoteRead;

	private PlayerInputControl _input;
	private UIManager _ui;
	private SoundManager _sound;

	public override void Start()
    {
		base.Start();
		_input = PlayerInputControl.instance;
		_ui = UIManager.instance;
		_sound = SoundManager.instance;
    }

	protected override void Interaction()
    {
        base.Interaction();
		if (_isOpen) CloseNote();
		else OpenNote();
		_isOpen = !_isOpen;
    }

	public void OpenNote(){
		_sound.OnNotePickup();
		Time.timeScale = 0;
		_input.LockAll();
		_ui.ShowReadNote(_textNote.text);
	}

	public void CloseNote(){
		_ui.HideReadNote();
		GamePauseManager.instance.ResumeGame();

		if (interactableName == "DiaryBook4" & !_isLastNoteRead){
			_isLastNoteRead = true;
			DialogueManager.instance.StartDialogue(_onLastNoteRead.text);
		}
	}

}
