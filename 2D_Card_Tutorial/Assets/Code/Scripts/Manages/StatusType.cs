using UnityEngine;

[CreateAssetMenu(fileName = "New Status", menuName = "Status")]
public class StatusType : ScriptableObject
{
	[Header("Specific Info")]
	public BuffType buffType;
	public new string name;
	public Sprite icon;
	public TextAsset description;
}
