using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	private Animation _animation;

	//animID
	private string _animZoomIn = "CameraZoomIn";
	private string _animZoomOut = "CameraZoomOut";

	//
	public static CameraManager instance;
	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		_animation = GetComponent<Animation>();
	}

	public IEnumerator OnZoomIn()
	{
		_animation.Play(_animZoomIn);
		yield return new WaitUntil(() => !_animation.isPlaying);
	}

	public IEnumerator OnZoomOut()
	{
		_animation.Play(_animZoomOut);
		yield return new WaitUntil(() => !_animation.isPlaying);
	}
}
