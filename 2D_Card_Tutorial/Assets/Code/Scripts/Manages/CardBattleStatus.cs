using UnityEngine;

[CreateAssetMenu(fileName = "New Card Battle", menuName = "CardBattle")]
public class CardBattleStatus : ScriptableObject
{
	public Sprite icon;
	public BuffType buffType;
	public int percentStatus;
}
