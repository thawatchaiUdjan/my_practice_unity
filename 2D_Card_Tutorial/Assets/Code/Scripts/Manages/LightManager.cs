using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
	[SerializeField] private float _darkIntensity = 0.5f;
	[SerializeField] private float _speedChange = 1f;

	//
	private float _lightIntensity;
	private Light2D _light;

	private void Start()
	{
		_light = GetComponent<Light2D>();
		_lightIntensity = _light.intensity;
	}

	public void TurnOn(bool isOn)
	{
		var intensity = _lightIntensity;
		if (!isOn)
		{
			intensity = _darkIntensity;
		}
		StartCoroutine(ChangeLight(intensity));
	}

	private IEnumerator ChangeLight(float targetIntensity)
	{
		while (_light.intensity != targetIntensity)
		{
			_light.intensity = Mathf.MoveTowards(_light.intensity, targetIntensity, _speedChange * Time.deltaTime);
			yield return null;
		}
	}

}
