using UnityEngine;
using UnityEngine.Playables;

public class CutSceneSkip : MonoBehaviour
{
	[SerializeField] private GameObject _skipScene;
	private PlayableDirector _scene;
	private float _endTimeFrame;

	private void Update()
	{
		if (_scene != null)
		{
			if (_scene.time >= _endTimeFrame)
			{
				IsShowSkip(false);
				_scene = null;
			}
		}
	}

	public void StartSceneSkip(PlayableDirector scene)
	{
		_scene = scene;
		IsShowSkip(true);
	}

	public void EndSceneSkip(float endTimeFrame)
	{
		_endTimeFrame = endTimeFrame;
	}

	public void OnClickSkip()
	{
		if (_endTimeFrame != 0 & _scene != null)
		{
			_scene.time = _endTimeFrame;
			IsShowSkip(false);
		}
		else
		{
			Debug.LogWarning("No Time Frame to Skip this scene" +
			" or no Scene to skip.");
		}
	}

	private void IsShowSkip(bool isShow)
	{
		_skipScene.SetActive(isShow);
	}
}
