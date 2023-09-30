using System.Collections;
using UnityEngine;

public class WorldGlobeInteract : Interactable
{	
	public bool isClear;
	public int clearAnswer;

	[Header("Text Assets")]
	[SerializeField] private TextAsset _unUseableText;
	private GameEventManager _event;
	private SoundManager _sound;
    public override void Start()
    {
        base.Start();	
		_event = GameEventManager.instance;
		_sound = SoundManager.instance;
    }

    protected override void Interaction()
    {
        base.Interaction();
		isInteractable = false;
		if(GameManager.instance.isPreGameClear){
			StartCoroutine(RotateY(90f, 1f));
			AudioSource.PlayClipAtPoint(_sound.worldGlobeRotate, transform.position, _sound.AudioVolume * 0.65f);
			return;
		}

		DialogueManager.instance.StartDialogue(_unUseableText.text);
		isInteractable = true;
    }

	private IEnumerator RotateY(float byAngles, float inTime){	
		var fromAngle = transform.rotation;
		var toAngle = Quaternion.Euler(transform.eulerAngles + Vector3.up * byAngles);
		for(var t = 0f; t < 1; t += Time.deltaTime * inTime) {
			transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
			yield return null;
		}

		transform.rotation = toAngle;
		CheckClear();
		isInteractable = true;
		_event.IsPuzzleClear();
		
	}

	private void CheckClear()
	{
		var AngleY = Mathf.FloorToInt(transform.eulerAngles.y);
		isClear = AngleY == clearAnswer;
	}
}
