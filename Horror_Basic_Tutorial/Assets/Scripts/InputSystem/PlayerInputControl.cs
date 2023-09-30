using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace PlayerController
{
	public class PlayerInputControl : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("General Input Values")]
		public bool flashLightAble = true;
		public bool flashLight;
		public bool escapeAble = true;
		public bool escape;

		[Header("Movement Settings")]
		public bool analogMovement;
		public bool moveable = true;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;
		public bool cursorCameraRotate = true;

		//
		public static PlayerInputControl instance;

#if ENABLE_INPUT_SYSTEM
		private void Awake() {
			instance = this;
		}
		public void OnMove(InputValue value)
		{
			if (moveable)
			{
				MoveInput(value.Get<Vector2>());
			}
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnFlashLight(InputValue value)
		{
			if (flashLightAble)
			{
				FlashLightInput(value.isPressed);
			}
		}

		public void OnEscape(InputValue value){
			if (escapeAble)
			{
				EscapeInput(value.isPressed);
			}
		}

#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void FlashLightInput(bool newFlashLightState)
		{
			flashLight = newFlashLightState;
		}

		public void EscapeInput(bool newEscapeState)
		{
			escape = newEscapeState;
		}
		
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

		public void LockInputCanRotate(){
			moveable = false;
			cursorCameraRotate = false;
			flashLightAble = false;
			escapeAble = false;
			move = Vector2.zero;
			look = Vector2.zero;
		}

		public void LockAll(bool escapable = false, bool isCursorQuit = false){
			cursorLocked = false;
			cursorInputForLook = false;
			moveable = false;
			flashLightAble = false;
			escapeAble = escapable;
			move = Vector2.zero;
			look = Vector2.zero;
			if (isCursorQuit) SetCursorState(cursorLocked);
		}

		public void ResetLockAll(){
			cursorLocked = true;
			cursorInputForLook = true;
			cursorCameraRotate = true;
			moveable = true;
			flashLightAble = true;
			escapeAble = true;
			SetCursorState(cursorLocked);
		}
	}
	
}