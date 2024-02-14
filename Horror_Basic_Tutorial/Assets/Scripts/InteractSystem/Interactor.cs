using UnityEngine.InputSystem;
using UnityEngine;
using System;
 
public class Interactor : MonoBehaviour
{
    [SerializeField] private float _interactDistance = 1.2f; //How far can see UI and Interact
    [SerializeField] private float _interactRadius = 0.07f; //Radius of Raycast
 
    private LayerMask layerMask;
    private Transform cameraTransform;
    private InputAction interactAction;
    
    //For Gizmo
    private Vector3 origin;
    private Vector3 direction;
    private Vector3 hitPosition;
    private float hitDistance;
 
   	private Interactable _interactTarget;
 
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        layerMask = LayerMask.GetMask("Interactable");
 
        interactAction = GetComponent<PlayerInput>().actions["Interact"];
        interactAction.performed += Interact;
    }
    // Update is called once per frame
    void Update()
    {
        direction = cameraTransform.forward;
        origin = cameraTransform.position;

        if (Physics.SphereCast(origin, _interactRadius, direction, out RaycastHit hit, layerMask))
        {   
            hitPosition = hit.point;
            hitDistance = hit.distance;

            if(hit.transform.TryGetComponent(out Interactable target))
			{ 	
				if (hitDistance <= _interactDistance){
					_interactTarget = target;
					_interactTarget.TargetOn();
				} 
				else if(_interactTarget){
					_interactTarget.TargetOff();
					_interactTarget = null;
				}
            }
			else if(_interactTarget){
				_interactTarget.TargetOff();
				_interactTarget = null;
			}
        }
        
    }
	
    private void Interact(InputAction.CallbackContext obj)
    { 
        if (_interactTarget != null)
		{
			_interactTarget.Interact(); 
        }
        else print("nothing to interact!");

    }
	
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(origin, origin + direction * hitDistance);
		Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(hitPosition, _interactRadius);
    }

    private void OnDestroy()
    {
        interactAction.performed -= Interact;
    }
}
 