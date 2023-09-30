using System.Collections;
using System.Collections.Generic;
using PlayerController;
using UnityEngine;

public class CameraInteract : Interactable
{
	private UIManager _ui;
	private PlayerInputControl _input;
	private SoundManager _sound;
	public override void Start()
    {
        base.Start();
		_ui = UIManager.instance;
		_input = PlayerInputControl.instance;
		_sound = SoundManager.instance;
    }

    protected override void Interaction()
    {
        base.Interaction();
		isInteractable = false;
		_sound.OnCameraPickup();
		_input.LockAll();
		StartCoroutine(OnPickup());
    }

	private IEnumerator OnPickup(){
		yield return StartCoroutine(_ui.ActionSceneFadeIn()); //Wait animate finish
		GameManager.instance.GameAfterCameraSetup();
		_sound.OnCameraEquip();
		gameObject.GetComponent<MeshRenderer>().enabled = false;
		TargetOff();
		yield return StartCoroutine(_ui.ActionSceneFadeOut()); //Wait animate finish
		_input.ResetLockAll();
		yield return new WaitForSeconds(5);
		_ui.ShowHintOpenLight();
		gameObject.SetActive(false);
	}
}
