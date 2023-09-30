using System.Collections;
using System.Collections.Generic;
using PlayerController;
using UnityEngine;

public class FlashLightManager : MonoBehaviour
{
	public Light flashLight;
	public bool isTurnOn = false;
	
	private float _lightIntensity;
	private PlayerInputControl _input;
	private SoundManager _sound;

	//
	public static FlashLightManager instance;
	private void Awake() {
		instance = this;
	}

    void Start()
    {	
		_input = PlayerInputControl.instance;
		_sound = SoundManager.instance;

        _lightIntensity = flashLight.intensity;
		flashLight.enabled = isTurnOn;
    }

    // Update is called once per frame
    void Update()
    {
		if (_input.flashLight)
		{
			if(isTurnOn) TurnOffLight();
			else TurnOnLight();
			_input.flashLight = false;
		}
    }

	public void SetLight(float percentLight){
		flashLight.intensity = _lightIntensity * percentLight/100f;
	}

	public void TurnOnLight(){
		if(isTurnOn) return;
		_sound.OnTurnOnFlashLight();
		flashLight.enabled = true;
		isTurnOn = true;
	}

	public void TurnOffLight(){
		if(!isTurnOn) return;
		_sound.OnTurnOffFlashLight();
		flashLight.enabled = false;
		isTurnOn = false;
	}

	
}
