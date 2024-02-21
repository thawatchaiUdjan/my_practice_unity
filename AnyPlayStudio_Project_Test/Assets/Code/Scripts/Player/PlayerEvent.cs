using System.Collections;
using FishNet.Object;
using UnityEngine;

public class PlayerEvent : NetworkBehaviour
{
	private PlayerUI _ui;
	private SoundManager _sound;
	private PlayerInputController _input;
	private Animator _animator;

	//AnimID
	private string _animIdle = "Idle";
	private string _animSpeed = "Speed";
	private string _animCrouch = "Crouch";
	private string _animProne = "Prone";
	private string _animFire = "Fire";

	private void Start()
	{
		_sound = SoundManager.instance;
		_input = GetComponent<PlayerInputController>();
		_ui = GetComponent<PlayerUI>();
		_animator = GetComponent<Animator>();
	}

	public void OnMoveEvent(float speed, float dampTime = 0)
	{
		_animator.SetFloat(_animSpeed, speed, dampTime, Time.deltaTime);
	}

	public void OnFireEvent()
	{
		_animator.SetTrigger(_animFire);
		_sound.OnPlaySFX(_sound.gunFire, transform.position);
	}

	public IEnumerator OnStandToCrouchEvent()
	{
		_animator.SetBool(_animCrouch, true);
		yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName(_animCrouch));
		_input.crouchAble = true;
		_input.proneAble = true;
		_input.moveAble = true;
		_input.fireAble = true;
		_ui.UpdateGamePadInputUI();
	}

	public IEnumerator OnCrouchToStandEvent()
	{
		_animator.SetBool(_animCrouch, false);
		yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName(_animIdle));
		_input.ResetLockInput();
		_ui.UpdateGamePadInputUI();
	}

	public IEnumerator OnCrouchToProneEvent()
	{
		_animator.SetBool(_animProne, true);
		yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName(_animProne));
		_input.proneAble = true;
		_input.moveAble = true;
		_input.fireAble = true;
	}

	public IEnumerator OnProneToCrouchEvent()
	{
		_animator.SetBool(_animProne, false);
		yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName(_animCrouch));
		_input.crouchAble = true;
		_input.proneAble = true;
		_input.moveAble = true;
		_input.fireAble = true;
		_ui.UpdateGamePadInputUI();
	}
}
