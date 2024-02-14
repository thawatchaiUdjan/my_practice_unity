using UnityEngine;

[CreateAssetMenu(fileName = "New card", menuName = "CardSkill/Buff/DefenseNormal")]
public class BuffNormal : CardSkill
{
	[Header("Specific Info")]
	[SerializeField] StatusType statusType;
	[SerializeField] private int defendUp = 20;
	[SerializeField] private int defendUpLv2 = 30;
	[SerializeField] private int defendUpLv3 = 50;
	[SerializeField] private int turnDuration = 2;

	protected override void CardUseLv1()
	{
		base.CardUseLv1();
		_statuses.Add(new StatusInfo(_playerStatus, this, statusType.buffType, defendUp, turnDuration, statusType));
		_playerStatus.GetStatus(_statuses);
	}
	protected override void CardUseLv2()
	{
		base.CardUseLv2();
		_statuses.Add(new StatusInfo(_playerStatus, this, statusType.buffType, defendUpLv2, turnDuration, statusType));
		_playerStatus.GetStatus(_statuses);
	}

	protected override void CardUseLv3()
	{
		base.CardUseLv3();
		_statuses.Add(new StatusInfo(_playerStatus, this, statusType.buffType, defendUpLv3, turnDuration, statusType));
		_playerStatus.GetStatus(_statuses);
	}

}
