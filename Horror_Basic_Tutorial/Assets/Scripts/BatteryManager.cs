using System.Collections;
using System.Collections.Generic;
using PlayerController;
using UnityEngine;
using UnityEngine.UI;

public class BatteryManager : MonoBehaviour
{
	[SerializeField] private Slider _batterySlider;
	[SerializeField] private Image _batteryCell;
	[SerializeField] private float _battery;
	[SerializeField] private float _maxBattery = 100f;

	[Header("Battery Changing")]
	[SerializeField] private float _deceasePerMinute = 1f;
	[SerializeField] private float _batteryPerChange = 25f;

	[Header("Light Intensity")]
	[SerializeField] private float _percentOfLight50 = 50f;
	[SerializeField] private float _percentOfLight25 = 25f;

	[SerializeField] private Color percentOfColor100;
	[SerializeField] private Color percentOfColor50;
	[SerializeField] private Color percentOfColor25;
	
	private FlashLightManager _flashLight;

	//
	public static BatteryManager instance;
	private void Awake() {
		instance = this;
	}

    void Start()
    {	
		if (_flashLight != null) return;
		_flashLight = FlashLightManager.instance;
		_battery = _maxBattery;
		_batterySlider.maxValue = _battery;
		_batterySlider.value = _battery;
    }

    void Update()
    {
        _batterySlider.value = _battery;
		CheckBattery();	
    }

	public void CheckBattery()
	{
		//For Max Battery
		var lightValue = 100f;
		var colorValue = percentOfColor100;

		//Lower 0%
		if (_battery <= 0f){
			lightValue = 5f;
		}
		//Lower 25%
		else if (_battery/_maxBattery * 100 <= 25f){
			lightValue = _percentOfLight25;
			colorValue = percentOfColor25;
		}
		//Lower 50%
		else if (_battery/_maxBattery * 100 <= 50f){
			lightValue = _percentOfLight50;
			colorValue = percentOfColor50;
		}

		_flashLight.SetLight(lightValue);
		_batteryCell.color = colorValue;
	}

	public void SetupBattery(){
		Start();
		StartCoroutine(DecreaseBattery());
	}

	public IEnumerator DecreaseBattery(){
		var time = 0f;
		while (true)
		{	
			if (_flashLight.isTurnOn & _battery > 0f)
			{
				time += Time.deltaTime;

				if (time >= _deceasePerMinute * 60f){
					_battery -= _batteryPerChange;
					time = 0f;
					if(_battery < 0f) _battery = 0f;
				}
			}

			yield return null;
		}
	}

	public void GetBattery(){
		_battery += _batteryPerChange;
		if(_battery > _maxBattery) _battery = _maxBattery;
	}
	

}
