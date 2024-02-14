using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundOnClick : MonoBehaviour
{
	[SerializeField] private AudioClip _sound;
	[SerializeField] private float _volume = 1f;
	
	private void Start()
	{
		var sound = SoundManager.instance;
		var button = GetComponent<Button>();
		button.onClick.AddListener(() => sound.OnPlaySFX(_sound, _volume));
	}
}
