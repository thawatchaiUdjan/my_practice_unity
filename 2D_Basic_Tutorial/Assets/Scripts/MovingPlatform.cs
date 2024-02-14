using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
	[SerializeField] private Transform startPoint;
	[SerializeField] private Transform endPoint;
	public float speed = 1.5f;
	public bool isPlay = true;

	//
	private Vector3 targetPos;

	private void Start()
	{
		transform.position = startPoint.position;
		targetPos = endPoint.position;
	}

	private void Update()
	{
		if (isPlay)
		{
			transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

			if (Vector2.Distance(transform.position, targetPos) <= 0f){
				targetPos = targetPos == endPoint.position 
				? startPoint.position : endPoint.position;
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D target)
	{
		if (target.gameObject.CompareTag("Player")){
			target.transform.SetParent(transform);
		}
	}

	private void OnCollisionExit2D(Collision2D target)
	{
		if (target.gameObject.CompareTag("Player")){
			target.transform.SetParent(null);
		}
	}
}
