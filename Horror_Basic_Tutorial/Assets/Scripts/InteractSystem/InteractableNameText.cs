using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableNameText : MonoBehaviour
{	
	public GameObject InteractableUI;

	private TextMeshProUGUI textUI;
	// Transform cameraTransform;

	private Transform _player;
	void Start()
	{
		// cameraTransform = Camera.main.transform;
		textUI = InteractableUI.GetComponentInChildren<TextMeshProUGUI>();
		_player = GameObject.FindWithTag("Player").transform;
		HideText();
	}
	public void ShowText(Interactable interactable)
	{
		InteractableUI.SetActive(true);

		if (interactable != null)
		{
            textUI.text = "E";
		}
		else
		{
            textUI.text = interactable.interactableName;
        }

	}

	public void HideText()
	{
		textUI.text = "";
		if (InteractableUI != null) InteractableUI.SetActive(false);
	}

	//Position of UI Prompt
	public void SetInteractableNamePosition(Interactable interactable)
	{
		//For Box Collider
        if (interactable.TryGetComponent(out Collider boxCollider))
        {
			var interactPos = interactable.transform.position;
			var interactDis = Vector3.up * (boxCollider.bounds.size.y / 2f + interactable.textDistance);

			//For Door Room Position
			if (interactable.interactableName == "DoorRoom")
			{
				interactDis = Vector3.right * (boxCollider.bounds.size.x + interactable.textDistance);
				interactDis += Vector3.up * (boxCollider.bounds.size.y / 2f);
			}

            transform.position = interactPos + interactDis;
			
			var lookAt = new Vector3(_player.position.x, transform.position.y, _player.position.z);
            transform.LookAt(lookAt);
        }
		else
		{
			print("Error, no collider found!");
		}
      

    }
}