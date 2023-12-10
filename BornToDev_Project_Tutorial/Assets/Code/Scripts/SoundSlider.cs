using TMPro;
using UnityEngine;

public class SoundSlider : MonoBehaviour
{
	private float _volumeValue;
	public void OnChangeValue(float volume)
	{
		_volumeValue = volume;
	}

	public void OnChangeValueText(TextMeshProUGUI textValue)
	{
		textValue.text = _volumeValue.ToString();
	}

}






