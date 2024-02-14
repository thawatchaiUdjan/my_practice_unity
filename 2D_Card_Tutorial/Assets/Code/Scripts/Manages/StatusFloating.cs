using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusFloating : MonoBehaviour
{
	[SerializeField] private Image _icon;
	[SerializeField] private TextMeshProUGUI _text;

	public void SetupData(StatusType status)
	{
		_icon.sprite = status.icon;
		_text.text = status.name;
	}
}
