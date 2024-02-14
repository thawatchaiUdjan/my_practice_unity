using UnityEngine;

public class Interactable : MonoBehaviour
{
	public bool isInteractable = true;

	[Header("Long Press Configs")]
	public bool isLongPress;
	public float longPressTime = 0.5f;

	//
	private bool _isPress;
	private float _longPressDeltaTime;

	void Start()
	{

	}

	private void FixedUpdate()
	{
		PressDown();
	}

	private void PressDown()
	{
		if (_isPress & isLongPress)
		{
			_longPressDeltaTime += Time.deltaTime;
			if (_longPressDeltaTime >= longPressTime)
			{
				Reset();
				OnLongPress();
			}
		}
	}

	private void Reset()
	{
		_isPress = false;
		_longPressDeltaTime = 0f;
	}

	private void OnMouseDown()
	{
		if (!isInteractable) return;
		_isPress = true;

		if (!isLongPress)
		{
			OnMouseUp();
			return;
		}
	}

	private void OnMouseUp()
	{
		if (!isInteractable) return;
		if (!_isPress) return;
		Reset();
		OnPress();
	}

	public virtual void OnPress()
	{
		Debug.Log("Press: " + name);
	}

	public virtual void OnLongPress()
	{
		Debug.Log("Long Press: " + name);
	}


}
