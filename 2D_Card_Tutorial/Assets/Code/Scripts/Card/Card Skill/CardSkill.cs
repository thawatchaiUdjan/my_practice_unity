using UnityEngine;

public class CardSkill : Skill
{
	public Sprite artwork;
	public AnimationClip animation;
	[SerializeField] private TextAsset descriptionLv1;
	[SerializeField] private TextAsset descriptionLv2;
	[SerializeField] private TextAsset descriptionLv3;

	public void UseCard(int level)
	{
		CheckEntity();
		switch (level)
		{
			case 1:
				CardUseLv1();
				break;
			case 2:
				CardUseLv2();
				break;
			case 3:
				CardUseLv3();
				break;
		}
	}

	public string Description(int level)
	{
		CheckEntity();
		var description = "";
		switch (level)
		{
			case 1:
				description = descriptionLv1.text;
				break;
			case 2:
				description = descriptionLv2.text;
				break;
			case 3:
				description = descriptionLv3.text;
				break;
		}
		return description;
	}

	protected virtual void CardUseLv1() { }
	protected virtual void CardUseLv2() { }
	protected virtual void CardUseLv3() { }

}

