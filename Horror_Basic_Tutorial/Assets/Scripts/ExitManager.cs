using System.Collections;
using System.Collections.Generic;
using PlayerController;
using UnityEngine;

public class ExitManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider target) {
		if (target.tag.Equals("Player"))
		{
			var gameSystem = GameManager.instance;

			if (TalismanInteract.instance.isClear) gameSystem.gameClearType = 2;
			else gameSystem.gameClearType = 1;
			
			gameSystem.GameClear();
		}
	}	
}
