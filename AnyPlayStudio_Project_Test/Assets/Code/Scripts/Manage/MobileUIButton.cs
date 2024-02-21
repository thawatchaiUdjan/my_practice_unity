using UnityEngine;

public class MobileUIButton : MonoBehaviour
{
	[SerializeField] private GameObject _background;
	[SerializeField] private float _disableAlpha = 0.8f;

	//
	private CanvasGroup _canvas;

	private void Awake()
	{
		_canvas = GetComponent<CanvasGroup>();
	}

	public void Active(bool isActive) //Active when Press button
	{
		_background.SetActive(isActive);
	}

	public void Show(bool isShow) //Show or disable the button
	{
		if (isShow)
		{
			SetButton(1f, true);
		}
		else
		{
			SetButton(_disableAlpha, false);
		}
		
	}

	private void SetButton(float alpha, bool interactable) //Set alpha and interact
	{
		_canvas.alpha = alpha;
		_canvas.interactable = interactable;
	}

}
