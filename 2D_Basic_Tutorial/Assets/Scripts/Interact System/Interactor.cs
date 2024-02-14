using TMPro;
using UnityEngine;

public class Interactor : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI interactText;

	//
	private Interactable interactTarget;
	private PlayerInputControl _input;

	//Input String Key
	private string _strInteract = "Interact";

	void Start()
	{
		_input = PlayerInputControl.instance;
		interactText.text = _input.GetKeyBind(_strInteract);
		interactText.gameObject.SetActive(false);
	}

	void Update()
	{
		if (_input.interact)
		{
			_input.interact = false;

			if (interactTarget != null)
			{
				interactTarget.Interact(this);
			}
			else
			{
				Debug.Log("Nothing to interact.");
			}

		}
	}

	public void ShowText()
	{
		Vector2 interactPos = interactTarget.GetComponent<BoxCollider2D>().bounds.center;
		Vector2 textPos = interactPos + Vector2.up * interactTarget.farDistance;
		interactText.transform.position = textPos;
		interactText.gameObject.SetActive(true);
	}

	public void HideText()
	{
		if (interactText == null) return;
		interactText.gameObject.SetActive(false);
		interactTarget = null;
	}

	private void OnTriggerStay2D(Collider2D target)
	{
		if (target.gameObject.TryGetComponent(out Interactable interact))
		{
			if (interact.isInteractable)
			{
				interactTarget = interact;
				ShowText();
			}

		}
	}

	private void OnTriggerExit2D(Collider2D target)
	{
		if (interactTarget != null)
		{
			HideText();
		}
	}
}
