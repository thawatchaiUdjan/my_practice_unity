using UnityEngine;

public class CameraController : MonoBehaviour
{
	public float followSpeed = 2f;
	public float yOffset = 1f;
    public Transform target;
	
    void Update()
    {
        var newPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);
		transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime);
    }
}
