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
			StartCoroutine(GameManager.instance.LetsGameStart());
			return;
		}

		ObjectiveManager.instance.GetObjective();
		SoundManager.instance.OnObjectivePickup();
    }
	
}