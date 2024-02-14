using System.Collections;
using TMPro;
using UnityEngine;

public class AlertBox : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _text;
	[SerializeField] private float _defaultDelay = 4f;

	//
	private float _delay;
	private Coroutine _isShowing;
	private Animation _animation;

	//AnimID
	private string _animIn = "AlertBoxIn";
	private string _animOut = "AlertBoxOut";

	private void Start()
	{
		_animation = GetComponent<Animation>();
	}

	public void Show(string text, float delay)
	{
		gameObject.SetActive(true);
		_delay = (delay == 0) ? _defaultDelay : delay;
		_text.text = text;
		if (_isShowing != null) ResetShow();
		_isShowing = StartCoroutine(Show());
	}

	private IEnumerator Show()
	{
		yield return new WaitForSeconds(_delay);
		_animation.Play(_animOut);
		yield return new WaitUntil(() => !_animation.isPlaying);
		gameObject.SetActive(false);
		_isShowing = null;
	}

	private void ResetShow()
	{
		_animation.Stop();
		_animation.Play(_animIn);
		StopCoroutine(_isShowing);
	}
}
