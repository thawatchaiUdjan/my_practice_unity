using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusDescription : MonoBehaviour
{
	[SerializeField] private GameObject _statusBox;
	[SerializeField] private TextMeshProUGUI _name;
	[SerializeField] private TextMeshProUGUI _turn;
	[SerializeField] private TextMeshProUGUI _description;

	//stringID
	private string _textPercent = "@Percent";
	private string _textTurn = "@Turn";

	public void SetupData(StatusInfo status, int turn, Vector2 pos)
	{
		_name.text = status.statusType.name;
		_turn.text = $"{turn} Turn Left";
		_description.text = SetDescriptionText(status, status.statusType.description.text);
		
		LayoutRebuilder.ForceRebuildLayoutImmediate(_statusBox.GetComponent<RectTransform>());
		_statusBox.transform.position = pos;
	}

	private string SetDescriptionText(StatusInfo status, string description)
	{
		description = description.Replace(_textPercent, status.percentStatus.ToString());
		description = description.Replace(_textTurn, status.turn.ToString());
		return description;
	}
}
