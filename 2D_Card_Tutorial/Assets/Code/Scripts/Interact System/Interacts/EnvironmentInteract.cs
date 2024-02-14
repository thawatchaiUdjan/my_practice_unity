using UnityEngine;

public class EnvironmentInteract : MonoBehaviour
{
	public bool isInteractable = true;

	void Start()
	{

	}

	private void OnMouseDown()
	{
		if (!isInteractable) return;
		OnPress();
	}

	public void OnPress()
	{
		Debug.Log("Press: " + name);
	}

}
