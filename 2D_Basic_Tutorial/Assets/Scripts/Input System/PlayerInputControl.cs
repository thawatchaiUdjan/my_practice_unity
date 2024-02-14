using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputControl : MonoBehaviour
{
	[Header("Character Input Values")]
	public Vector2 move;
	public bool jump;
	public bool sprint;
	public bool attack;
	public bool block;
	public bool potion;
	public bool interact;

	[Header("Player Input able")]
	public bool moveAble = true;
	public bool jumpAble = true;
	public bool attackAble = true;
	public bool blockAble = true;
	public bool potionAble = true;
	public bool interactAble = true;

#if ENABLE_INPUT_SYSTEM
	public static PlayerInputControl instance;

	private void Awake()
	{
		instance = this;
	}
	public void OnMove(InputValue value)
	{
		if (moveAble)
		{
			move = value.Get<Vector2>();
		}
	}

	public void OnSprint(InputValue value)
	{
		sprint = value.isPressed;
	}

	public void OnJump(InputValue value)
	{
		if (jumpAble)
		{
			jump = value.isPressed;
		}
	}
	public void OnAttack(InputValue value)
	{
		if (attackAble)
		{
			attack = value.isPressed;
		}
	}

	public void OnBlock(InputValue value)
	{
		if (blockAble)
		{
			block = value.isPressed;
		}
	}

	public void OnPotion(InputValue value)
	{
		if (potionAble)
		{
			potion = value.isPressed;
		}
	}

	public void OnInteract(InputValue value)
	{
		if (interactAble)
		{
			interact = value.isPressed;
		}
	}

#endif

	public void LockAllInput
	(
		bool move = false,
		bool jump = false,
		bool attack = false,
		bool block = false,
		bool potion = false,
		bool interact = false
	)
	{
		moveAble = move;
		jumpAble = jump;
		attackAble = attack;
		blockAble = block;
		potionAble = potion;
		interactAble = interact;
	}

	public void ResetLockAllInput()
	{
		moveAble = true;
		jumpAble = true;
		attackAble = true;
		blockAble = true;
		potionAble = true;
		interactAble = true;
	}

	public void ResetAllInputVal(){
		move = Vector2.zero;
		sprint = false;
		jump = false;
		attack = false;
		block = false;
		potion = false;
		interact = false;
	}

	public string GetKeyBind(string strAction){
		return GetComponent<PlayerInput>().actions[strAction].GetBindingDisplayString();
	}

}
