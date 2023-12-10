using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
	[Header("Player Input Values")]
	public Vector2 move;
	public Vector2 look;
	public bool sprint;
	public bool shoot;
	public bool pause;

	[Header("Player InputAble")]
	public bool moveAble = true;
	public bool sprintAble = true;
	public bool shootAble = true;
	public bool pauseAble = true;

	[Header("Mouse Cursor Settings")]
	[SerializeField] private bool _isCursorLock = true;
	public bool isCursorForLook = true;
	public bool IsCursorLock
	{
		get => _isCursorLock;
		set
		{
			_isCursorLock = value;
			SetCursorState(_isCursorLock);
		}
	}

	//
	public static PlayerInputController instance;
	private void Awake()
	{
		instance = this;
	}

#if ENABLE_INPUT_SYSTEM
	public void OnMove(InputValue value)
	{
		move = value.Get<Vector2>();
	}

	public void OnLook(InputValue value)
	{
		if (isCursorForLook)
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

	public void OnShoot(InputValue value)
	{
		if (shootAble)
		{
			shoot = value.isPressed;
		}
	}

	public void OnPause(InputValue value)
	{
		if (pauseAble)
		{
			pause = value.isPressed;
		}
	}

#endif

	public void LockAllInput()
	{
		moveAble = false;
		sprintAble = false;
		shootAble = false;
		pauseAble = false;
		isCursorForLook = false;
		ResetAllValueInput();
	}

	public void ResetAllInput()
	{
		moveAble = true;
		sprintAble = true;
		shootAble = true;
		pauseAble = true;
		isCursorForLook = true;
	}

	public void ResetAllValueInput()
	{
		move = Vector2.zero;
		look = Vector2.zero;
	}

	public void SetCursorState(bool cursorLock)
	{
		Cursor.lockState = cursorLock ? CursorLockMode.Locked : CursorLockMode.None;
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		SetCursorState(IsCursorLock);
	}

}
