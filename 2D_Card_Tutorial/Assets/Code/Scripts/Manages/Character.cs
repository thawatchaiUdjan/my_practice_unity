using UnityEngine;

public class Character : ScriptableObject
{
	public GameObject prefab;
	public string characterName;

	[Header("Status Setting")]
	public int attack = 450;
	public int defense = 90;
	public int health = 9000;
	public int speed = 0;
}