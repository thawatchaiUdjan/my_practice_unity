using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class Interactable : MonoBehaviour
{
    [Header("Interaction Data")]
	public bool isInteractable = true;
	public float textDistance = 0f;
    public string interactableName = "";
    
    private InteractableNameText interactableNameText;
    private GameObject interactableNameCanvas;

    public virtual void Start()
    {
        interactableNameCanvas = GameObject.FindGameObjectWithTag("Canvas");
        interactableNameText = interactableNameCanvas.GetComponentInChildren<InteractableNameText>();
    }
 
    public void TargetOn()
    {	
		if (!isInteractable) return;
		if (interactableNameText == null) Start();
		
        interactableNameText.ShowText(this);
        interactableNameText.SetInteractableNamePosition(this);
    }
 
    public void TargetOff()
    {
        interactableNameText.HideText();
    }
 
    public void Interact()
    {
        if (isInteractable) Interaction();
    }
 
    protected virtual void Interaction()
    {
        print("Interact with: " + this.name);
    }
 
    private void OnDestroy()
    {
        TargetOff();
    }

	// private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.blue;
		
	// 	Vector3 interactPos = transform.position;

    //     Gizmos.DrawWireSphere(interactPos, interactionDistance);
    // }
}