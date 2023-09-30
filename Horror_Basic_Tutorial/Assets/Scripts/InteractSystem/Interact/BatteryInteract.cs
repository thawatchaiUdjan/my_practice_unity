using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryInteract : Interactable
{
    public override void Start()
    {
        base.Start();
    }

    protected override void Interaction()
    {
        base.Interaction();
		BatteryManager.instance.GetBattery();
		SoundManager.instance.OnBatteryPickup();
		gameObject.SetActive(false);
    }

}