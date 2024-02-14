using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	[Range(0, 1)] public float AudioVolume = 0.3f;

	[Header("General Sound")]
	public List<AudioClip> FootStepAudioClip;
	public AudioClip batteryPickup;
	public AudioClip objectivePickup;
	public AudioClip notePickup;
	public AudioClip turnOnFlashLight;
	public AudioClip turnOffFlashLight;
	public AudioClip heartbeatSlow;
	public AudioClip heartbeatMedium;
	public AudioClip heartbeatFast;
	public AudioClip worldGlobeRotate;

	[Header("Game Status Sound")]
	public AudioClip breakingBone;
	public AudioClip clockAlarm;
	public AudioClip cryingGhost;
	public AudioClip firstObjectPickup;
	public AudioClip screamSound;
	public AudioClip fontDoorOpen;
	public AudioClip metalDoorOpen;
	public AudioClip tableFall;
	public AudioClip thunder;
	public AudioClip paperSlide;
	public AudioClip clockCountdown;
	public AudioClip cameraPickup;
	public AudioClip cameraEquip;

	[Header("Ghost Behaviour Sound")]
	public AudioClip ghostHumming;
	public AudioClip ghostCrying;
	public AudioClip ghostLaugh;
	public AudioClip ghostThriller;
	public AudioClip ghostZomIdle;

	[Header("Jump Scare Sound")]
	public AudioClip chairDrag;
	public AudioClip cupMetalDrop;
	public AudioClip doorRoomOpen;
	public AudioClip doorRoomClose;
	public AudioClip bookDrop;

	private AudioSource soundBg;
	private Transform _player;
	//
	public static SoundManager instance;
	private void Awake()
	{
		instance = this;
		_player = GameObject.FindWithTag("Player").transform;
	}

	void Start()
	{
		soundBg = GetComponent<AudioSource>();
		soundBg.volume = AudioVolume * 0.04f;
		soundBg.Play();
	}

	public void StopSoundBg()
	{
		soundBg.Stop();
	}

	public void OnBatteryPickup()
	{
		if (batteryPickup != null)
		{
			AudioSource.PlayClipAtPoint(batteryPickup, _player.position, AudioVolume * 0.5f);
		}
	}

	public void OnObjectivePickup()
	{
		if (objectivePickup != null)
		{
			AudioSource.PlayClipAtPoint(objectivePickup, _player.position, AudioVolume * 0.5f);
		}
	}

	public void OnNotePickup()
	{
		if (notePickup != null)
		{
			AudioSource.PlayClipAtPoint(notePickup, _player.position, AudioVolume);
		}
	}

	public void OnTurnOnFlashLight()
	{
		if (turnOnFlashLight != null)
		{
			AudioSource.PlayClipAtPoint(turnOnFlashLight, _player.position, AudioVolume * 0.5f);
		}
	}

	public void OnTurnOffFlashLight()
	{
		if (turnOffFlashLight != null)
		{
			AudioSource.PlayClipAtPoint(turnOffFlashLight, _player.position, AudioVolume * 0.5f);
		}
	}

	public void OnFirstObjectivePickup()
	{
		if (firstObjectPickup != null)
		{
			AudioSource.PlayClipAtPoint(firstObjectPickup, _player.position, AudioVolume * 0.2f);
		}
	}

	public void OnCameraPickup()
	{
		if (cameraPickup != null)
		{
			AudioSource.PlayClipAtPoint(cameraPickup, _player.position, AudioVolume * 0.5f);
		}
	}

	public void OnCameraEquip()
	{
		if (cameraEquip != null)
		{
			AudioSource.PlayClipAtPoint(cameraEquip, _player.position, AudioVolume * 0.5f);
		}
	}

	public void OnCountdownTime()
	{
		if (clockCountdown != null)
		{
			AudioSource.PlayClipAtPoint(clockCountdown, _player.position, AudioVolume * 0.35f);
		}
	}

	public void OnGameClear_1()
	{
		if (breakingBone != null)
		{
			AudioSource.PlayClipAtPoint(breakingBone, _player.position, AudioVolume * 0.5f);
		}
	}

	public void OnGameClear_2()
	{
		if (clockAlarm != null)
		{
			AudioSource.PlayClipAtPoint(clockAlarm, _player.position, AudioVolume * 0.5f);
		}
	}

	public AudioClip GetGhostSoundAnim(GhostTypeManager.Anim animType)
	{
		AudioClip behaviourSound = null;

		switch (animType)
		{
			case GhostTypeManager.Anim.Humming:
				behaviourSound = ghostHumming;
				break;
			case GhostTypeManager.Anim.Crying:
				behaviourSound = ghostCrying;
				break;
			case GhostTypeManager.Anim.Sitting:
				behaviourSound = ghostLaugh;
				break;
			case GhostTypeManager.Anim.ThrillerIdle:
				behaviourSound = ghostThriller;
				break;
			case GhostTypeManager.Anim.ZombieIdle:
				behaviourSound = ghostZomIdle;
				break;
		}

		return behaviourSound;
	}

}
