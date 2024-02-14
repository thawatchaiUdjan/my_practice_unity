using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveInteract : Interactable
{
    public override void Start()
    {
        base.Start();
    }

    protected override void Interaction()
    {
        base.Interaction();

		if (interactableName.Equals("FirstObjective"))
		{
			isInteractable = false;
			TargetOff();
			GetComponent<Renderer>().enabled = false;
			StartCoroutine(GameManager.instance.LetsGameStart());
			return;
		}

		ObjectiveManager.instance.GetObjective();
		SoundManager.instance.OnObjectivePickup();
    }
	
}