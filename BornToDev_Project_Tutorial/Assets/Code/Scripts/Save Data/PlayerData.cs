using System;

[Serializable]
public class PlayerData
{
	public int score;

	private static PlayerData _instance;
	public static PlayerData Instance
	{
		get
		{
			_instance ??= new PlayerData();
			return _instance;
		}
	}

	public PlayerData()
	{
		score = 0;
	}

	public void SetPlayerData(PlayerData data)
	{
		score = data.score;
	}
}
