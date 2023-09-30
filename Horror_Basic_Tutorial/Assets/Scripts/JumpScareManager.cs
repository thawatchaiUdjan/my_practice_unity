using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScareManager : MonoBehaviour
{
	[Header("Jump Scare Zone")]
	public GameObject chair_1;

	[Header("Jump Scare Zone 1")]
	public GameObject oldCup;
	public GameObject oldCupDummy;

	[Header("Jump Scare Zone 2")]
	public GameObject doorRoom;

	[Header("Jump Scare Zone 3")]
	public GameObject book;
	public float speed;

	private SoundManager _sound;
    
    void Start()
    {
    	_sound = SoundManager.instance;
    }


	private void OnTriggerEnter(Collider target) {
		if (target.tag == "Player")
		{
			switch (gameObject.name)
			{
				
				case "JumpScare Trigger":
					chair_1.GetComponent<Rigidbody>().AddForce(-chair_1.transform.forward * 6f, ForceMode.Impulse);
					PlaySound(_sound.chairDrag, chair_1.transform.position, _sound.AudioVolume * 3f);
				break;

				case "JumpScare Trigger 1" :
					StartCoroutine(JumpScareTrigger1());
				break;

				case "JumpScare Trigger 2" :
					doorRoom.GetComponent<DoorRoomInteract>().DoorClose();
				break;

				case "JumpScare Trigger 3" :
					StartCoroutine(JumpScareTrigger3());
				break;
				
			}

			GameObject.Find(gameObject.name).SetActive(false);
		}
	}

	private void PlaySound(AudioClip clip, Vector3 pos, float volume){
		AudioSource.PlayClipAtPoint(clip, pos, volume);
	}

	public IEnumerator JumpScareTrigger1(){
		oldCup.SetActive(true);
		yield return new WaitForSeconds(0.2f);
		PlaySound(_sound.cupMetalDrop, oldCupDummy.transform.position, _sound.AudioVolume * 4f);
		yield return new WaitForSeconds(4f);
		oldCupDummy.SetActive(false);
		oldCup.GetComponent<Rigidbody>().isKinematic = true;
		oldCup.GetComponent<Collider>().isTrigger = true;
	}
	
	public IEnumerator JumpScareTrigger3(){
		book.SetActive(true);
		PlaySound(_sound.bookDrop, book.transform.position, _sound.AudioVolume);
		yield return new WaitForSeconds(1f);	
		book.GetComponent<Rigidbody>().isKinematic = true; 
		book.GetComponent<Collider>().isTrigger = true;
	}
}
