using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRoomInteract : Interactable
{
	private Animation _animation;
	private SoundManager _sound;
	
	//Anim ID
	private string _animDoorOpen = "DoorRoomOpen";
	private string _animDoorClose = "DoorRoomClose";
    public override void Start()
    {
        base.Start();
		_sound = SoundManager.instance;
		_animation = GetComponent<Animation>();
    }

    protected override void Interaction()
    {
        base.Interaction();
		GetComponent<Animation>().Play(_animDoorOpen);
		TargetOff();
		isInteractable = false;
    }

	public void OnDoorRoomOpen(){
		AudioSource.PlayClipAtPoint(_sound.doorRoomOpen, transform.position, _sound.AudioVolume * 0.67f);
	}

	public void OnDoorRoomClose(){
		AudioSource.PlayClipAtPoint(_sound.doorRoomClose, transform.position, _sound.AudioVolume * 4f);
	}

	public void DoorClose(){
		_animation.Play(_animDoorClose);
		isInteractable = true;
	}
}
