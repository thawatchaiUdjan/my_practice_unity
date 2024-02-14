using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _valueText;

	//
	private Slider _slider;

	private void Awake()
	{
		_slider = GetComponent<Slider>();
		_slider.onValueChanged.AddListener(OnChangeValue);
	}

	private void OnEnable()
	{
		OnChangeValue(_slider.value);
	}

	private void OnChangeValue(float value)
	{
		_valueText.text = value.ToString();
	}
}
