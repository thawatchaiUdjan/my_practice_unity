using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundClick : MonoBehaviour
{
	private void Start()
	{
		AddBtnSoundOnClick();
	}

	private void AddBtnSoundOnClick()
	{
		var buttons = gameObject.GetComponentsInChildren<Button>(true);
		foreach (var button in buttons)
		{
			button.onClick.AddListener(OnClickSound);
		}
	}

	private void OnClickSound()
	{
		var sound = SoundManager.instance;
		if (sound != null)
		{
			sound.OnPlaySFX(sound.buttonClick);
		}
		else
		{
			Debug.LogWarning("No Source Sound to play Button Click Sound.");
		}
	}

}
