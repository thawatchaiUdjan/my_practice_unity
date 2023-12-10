using UnityEngine;
using UnityEngine.AI;

public class PartnerController : MonoBehaviour
{
	public float moveSpeed = 1.5f;
	public float sprintSpeed = 3.5f;
	public float sprintDistance = 3f;
	//
	private NavMeshAgent _agent;
	private Transform _player;
	private Animator _animator;

	//AnimID
	private string _animSpeed = "Speed";

	private void Start()
	{
		_agent = GetComponent<NavMeshAgent>();
		_animator = GetComponent<Animator>();
		_player = GameObject.FindWithTag("Player").transform;
	}

	private void Update()
	{
		//
	}

	private void FixedUpdate()
	{
		Move();
	}

	private void Move()
	{
		_agent.SetDestination(_player.position);
		_agent.speed = _agent.remainingDistance >= sprintDistance ? sprintSpeed : moveSpeed;
		if (_agent.remainingDistance > _agent.stoppingDistance)
		{
			_animator.SetFloat(_animSpeed, _agent.speed, 0.1f, Time.deltaTime);
		}
		else _animator.SetFloat(_animSpeed, 0f, 0.025f, Time.deltaTime);

		if (_animator.GetFloat(_animSpeed) <= 0.1f) _animator.SetFloat(_animSpeed, 0f);
	}
}
