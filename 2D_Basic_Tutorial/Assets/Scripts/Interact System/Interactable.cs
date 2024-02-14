using UnityEngine;

public class Interactable : MonoBehaviour
{
	public bool isInteractable = true;
	public float farDistance = 0.5f;
	
	public virtual void Interact(Interactor interactor){
		Debug.Log("Player interact with: " + gameObject.name);
	}

}
