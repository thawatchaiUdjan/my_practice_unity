using System.Collections;
using System.Collections.Generic;
using PlayerController;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI dialogueSystem;

	//Setting
	private string[] _msg;
	private int _msgIndex;
	private float _defaultTextSpeed = 0.02f;
	private bool _canSkip = true;
	private Coroutine _onPrintMsg;
	
	//
	private PlayerInputControl _input;

	//
	public static DialogueManager instance; 
	private void Awake() {
		instance = this;
	}
    void Start()
    {
        _input = PlayerInputControl.instance;
    }

    
    void Update()
    {
        if (Input.GetMouseButtonDown(0) & dialogueSystem.gameObject.activeSelf & _canSkip)
		{
			if (dialogueSystem.text == _msg[_msgIndex])
			{
				NextMsg();
			}
			else
			{
				StopAllCoroutines();
				dialogueSystem.text = _msg[_msgIndex];
			}
		}
    }

	public void StartDialogue(string msg, float textSpeed = 0f){
		if (_onPrintMsg != null) return;
		ResetDialogue();
		dialogueSystem.gameObject.SetActive(true);
		_msg = msg.Split("\n");
		_input.LockAll(isCursorQuit: true);
		_onPrintMsg = StartCoroutine(PrintMsg(textSpeed));
	}
	
	private void ResetDialogue(){
		dialogueSystem.text = string.Empty;
		dialogueSystem.color = Color.HSVToRGB(0, 0, 90);
		dialogueSystem.characterSpacing = 3f;
		_canSkip = true;
		_msgIndex = 0;
	}

	private IEnumerator PrintMsg(float textSpeed = 0f){
		textSpeed = textSpeed.Equals(0f) ? _defaultTextSpeed : textSpeed;
		
		foreach (var c in _msg[_msgIndex].ToCharArray())
		{
			dialogueSystem.text += c;
			yield return new WaitForSeconds(textSpeed);
		}
	}

	private void NextMsg(){
		if (_msgIndex < _msg.Length - 1)
		{
			_msgIndex++;
			dialogueSystem.text = string.Empty;
			_onPrintMsg = StartCoroutine(PrintMsg());
		}
		else
		{
			dialogueSystem.gameObject.SetActive(false);
			_onPrintMsg = null;
			_input.ResetLockAll();
		}
	}

	public void OnGameClear_2(string msg){
		StartDialogue(msg, 0.5f);
		dialogueSystem.color = Color.HSVToRGB(0f/360, 100f/100, 55f/100); //Color Red
		dialogueSystem.characterSpacing = 50f;
		_canSkip = false;
	}


}
