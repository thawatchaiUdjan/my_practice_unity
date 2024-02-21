using FishNet.Object;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
	[Header("Player Input Values")]
	public Vector2 move;
	public Vector2 look;
	public bool sprint;
	public bool jump;
	public bool crouch;
	public bool prone;
	public bool fire;

	[Header("Player InputAble")]
	public bool lookAble = true;
	public bool moveAble = true;
	public bool sprintAble = true;
	public bool jumpAble = true;
	public bool crouchAble = true;
	public bool proneAble = false;
	public bool fireAble = true;

#if ENABLE_INPUT_SYSTEM
	public void OnMove(InputValue value)
	{
		move = value.Get<Vector2>();
	}

	public void OnLook(InputValue value)
	{
		if (lookAble)
		{
			look = value.Get<Vector2>();
		}
	}

	public void OnSprint(InputValue value)
	{
		if (sprintAble)
		{
			sprint = value.isPressed;
		}
	}

	public void OnJump(InputValue value)
	{
		if (jumpAble)
		{
			jump = value.isPressed;
		}
	}

	public void OnCrouch(InputValue value)
	{
		if (crouchAble)
		{
			crouch = value.isPressed;
		}
	}

	public void OnProne(InputValue value)
	{
		if (proneAble)
		{
			prone = value.isPressed;
		}
	}

	public void OnFire(InputValue value)
	{
		if (fireAble)
		{
			fire = value.isPressed;
		}
	}

#endif

	public void LockAllInput(bool reset = true, bool look = false, bool move = false, 
	bool sprint = false, bool jump = false, bool crouch = false, bool prone = false,
	bool fire = false)
	{
		lookAble = look;
		moveAble = move;
		sprintAble = sprint;
		jumpAble = jump;
		crouchAble = crouch;
		proneAble = prone;
		fireAble = fire;
		if (reset) ResetAllValueInput();
	}

	public void ResetLockInput()
	{
		lookAble = true;
		moveAble = true;
		sprintAble = true;
		jumpAble = true;
		crouchAble = true;
		fireAble = true;
	}

	public void ResetAllValueInput()
	{
		move = Vector2.zero;
		look = Vector2.zero;
	}

}
