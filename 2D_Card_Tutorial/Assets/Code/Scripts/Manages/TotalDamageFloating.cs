using TMPro;
using UnityEngine;

public class TotalDamageFloating : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

	public void SetupData(string text)
	{
		_text.text = text;
	}
}
