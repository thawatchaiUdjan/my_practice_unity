using UnityEngine;

public interface IPlaySFXEvent
{
	public void OnPlaySFX(AudioClip audio);
}

public interface IPlayEffectEvent
{
	public void OnPlayEffect(GameObject effect);
}


