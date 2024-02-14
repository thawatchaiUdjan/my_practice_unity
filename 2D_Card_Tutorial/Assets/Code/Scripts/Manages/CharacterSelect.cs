using System.Collections;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
	[SerializeField] private float _moveSpeed = 1f;

	//
	private bool _isMove;
	private Vector2 _target;
	private Animator _animator;

	//animID
	private string _animWalk = "Walk";
	private string _animVictory = "Victory";
	
	private void Start()
	{
		_animator = GetComponent<Animator>();
	}

	private void FixedUpdate()
	{
		Move();
	}

	private void Move()
	{
		if (_isMove)
		{
			transform.position = Vector2.MoveTowards(transform.position, _target, _moveSpeed * Time.deltaTime);

			if (Vector2.Distance(transform.position, _target) <= 0.1f)
			{
				transform.position = _target;
				_isMove = false;
			}
		}
	}

	public IEnumerator EnterBattleScene()
	{
		_animator.SetBool(_animVictory, true);
		yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName(_animVictory));
		_animator.SetBool(_animWalk, true);
		yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName(_animWalk));
		_target = transform.position + Vector3.right * 10f;
		_isMove = true;
		yield return new WaitUntil(() => !_isMove);
	}
}
