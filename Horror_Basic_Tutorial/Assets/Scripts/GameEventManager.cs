using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
	[Header("Game Objects")]
	public GameObject[] puzzleOnMap;
	public GameObject fontDoor;
	public GameObject metalDoor;
	public GameObject ThunderStorm;
	public GameObject exitDoor;
	public GameObject objectsOnClue;
	public GameObject talisman;
	//

	private SoundManager _sound;

	//
	public static GameEventManager instance;
    private void Awake() {
		instance = this;
	}
    void Start()
    {
        _sound = SoundManager.instance;
    }

	private void PlayAnimate(GameObject obj, AudioClip clip, float volume = 0f){
		var sound = obj.GetComponent<AudioSource>();
		sound.volume = (volume == 0f) ? _sound.AudioVolume : volume;
		sound.clip = clip;
		sound.Play();

		obj.GetComponent<Animation>().Play();
	}

	public void FontDoorOpen(){
		PlayAnimate(fontDoor, _sound.fontDoorOpen, _sound.AudioVolume * 0.5f);
	}

	public IEnumerator PreEventGameClear(){ 
		PlayAnimate(ThunderStorm, _sound.thunder);
		yield return new WaitForSeconds(3f);
		PlayAnimate(objectsOnClue, _sound.tableFall);
		yield return new WaitForSeconds(1);
		PlayAnimate(metalDoor, _sound.metalDoorOpen);
		exitDoor.SetActive(true);
	}

	public void IsPuzzleClear(){
		if(CheckPuzzleClear()){
			foreach (var obj in puzzleOnMap){
				var puzzle = obj.GetComponent<WorldGlobeInteract>();
				puzzle.isInteractable = false;
				puzzle.TargetOff();
				talisman.SetActive(true);
			}
		}
	}

	public bool CheckPuzzleClear(){
		var isClear = true;

		foreach (var obj in puzzleOnMap){
			var puzzle = obj.GetComponent<WorldGlobeInteract>();
			if(!puzzle.isClear){
				isClear = false;
				break;
			}
		}

		return isClear;
	}
		
	
}
