using Cinemachine;
using FishNet.Object;
using UnityEngine;
using Lean.Pool;
using System;

public class PlayerController : NetworkBehaviour
{
	[Header("Player Setting")]
	public float moveSpeed = 2f;
	public float sprintSpeed = 3.5f;
	public float crouchSpeed = 1.5f;
	public float proneSpeed = 1f;

	[Header("Player Input Action")]
	public bool isSprint;
	public bool isCrouch;
	public bool isProne;

	[Header("Fire Setting")]
	[SerializeField] private GameObject _bullet;
	[SerializeField] private Transform _firePoint;
	public float _rateOfFire = 0.1f;

	[Header("Cinemachine")]
	[SerializeField] private Camera _mainCamera;
	[SerializeField] private CinemachineVirtualCamera _followCamera;
	

	[Header("Camera Setting")]
	[SerializeField] private Transform _standCamPos;
	[SerializeField] private Transform _crouchCamPos;
	[SerializeField] private Transform _proneCamPos;
	public float cameraSpeedX = 3f;
	public float cameraSpeedY = 0.2f;
	public float cameraSpeedFireX = 1f;
	public float cameraSpeedFireY = 0.2f;
	public float cameraMoveSpeed = 1;

	[Space(10)]
	public float TopClamp = 70.0f;
	public float BottomClamp = -30.0f;
	public float CameraAngleOverride = 0.0f;
	public bool isInvertY;

	//
	private float _targetSpeed;
	private float _fireDeltaTime;
	private Transform _targetCamera;

	//Cinemachine
	private float _cinemachineTargetYaw;
	private float _cinemachineTargetPitch;
	private bool _isCurrentDeviceMouse;
	private const float _threshold = 0.01f;

	//
	private PlayerInputController _input;
	private GameManager _game;
	private PlayerUI _ui;
	private CharacterController _controller;
	private PlayerEvent _event;
	private Animator _animator;

	//AnimID
	private string _animSpeed = "Speed";

	private void Start()
	{
		_game = GameManager.instance;
		_input = GetComponent<PlayerInputController>();
		_controller = GetComponent<CharacterController>();
		_event = GetComponent<PlayerEvent>();
		_animator = GetComponent<Animator>();
		_ui = GetComponent<PlayerUI>();

		//
		_game.PlayerJoin(this);
		_targetCamera = _standCamPos;
		_cinemachineTargetYaw = _targetCamera.transform.rotation.eulerAngles.y;
		_targetSpeed = moveSpeed;
	}

	public override void OnStartClient()
	{
		base.OnStartClient();
		if (base.IsOwner)
		{
			_mainCamera.gameObject.SetActive(true);
			_followCamera.gameObject.SetActive(true);
		}
	}

	private void Update()
	{
		if (!base.IsOwner) return;
		Move();
		Crouch();
		Prone();
	}

	private void FixedUpdate()
	{
		if (!base.IsOwner) return;
		Fire();
	}

	private void LateUpdate()
	{
		if (!base.IsOwner) return;
		CameraRotation();
	}

	private static float ClampAngle(float lfAngle, float lfMin, float lfMax) //Clamp value e.g.camera angles
	{
		if (lfAngle < -360f) lfAngle += 360f;
		if (lfAngle > 360f) lfAngle -= 360f;
		return Mathf.Clamp(lfAngle, lfMin, lfMax);
	}

	private void CameraRotation() //Move Camera with Mouse
	{
		// Update Follow Camera
		_followCamera.Follow = _targetCamera;

		// if there is an input and camera position is not fixed
		if (_input.look.sqrMagnitude >= _threshold && _input.lookAble)
		{
			var TargetCamSpeedX = _input.fire ? cameraSpeedFireX : cameraSpeedX;
			var TargetCamSpeedY = _input.fire ? cameraSpeedFireY : cameraSpeedY;
			_cinemachineTargetYaw += _input.look.x * TargetCamSpeedX;
			_cinemachineTargetPitch += isInvertY ? _input.look.y : -_input.look.y * TargetCamSpeedY;
		}

		// clamp our rotations so our values are limited 360 degrees
		_cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
		_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

		// Cinemachine will follow this target
		_targetCamera.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
			_cinemachineTargetYaw, 0.0f);

		transform.rotation = Quaternion.Euler(0f, _targetCamera.eulerAngles.y, 0f);
	}

	private void Move() //Player Move 
	{
		if (_input.move != Vector2.zero & _input.moveAble)
		{
			var inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
			var targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
			var targetDirection = Quaternion.Euler(0f, targetRotation, 0f) * Vector3.forward;

			//Move character controller
			_controller.Move(targetDirection.normalized * (_targetSpeed * Time.deltaTime));
			_event.OnMoveEvent(_targetSpeed);
		}
		else
		{
			_event.OnMoveEvent(0f, 0.025f); // For reset move animation
		}

		if (_animator.GetFloat(_animSpeed) <= 0.1f)
		{
			_event.OnMoveEvent(0f);
		}

		if (_input.sprint) //If Player sprint
		{
			_input.sprint = false;
			if (!isSprint)
			{
				isSprint = true;
				_targetSpeed = sprintSpeed;
			}
			else
			{
				isSprint = false;
				_targetSpeed = moveSpeed;
			}
			_ui.UpdateGamePadInputUI();
		}
	}

	private void Crouch() //Player Crouch
	{
		if (_input.crouch & _input.crouchAble)
		{
			_input.crouch = false;
			if (!isCrouch)
			{
				isCrouch = true;
				isSprint = false;
				_targetSpeed = crouchSpeed;
				_targetCamera = _crouchCamPos;
				_input.LockAllInput(reset: false, look: true, crouch: true, fire: true);
				_input.crouchAble = false;
				StartCoroutine(_event.OnStandToCrouchEvent());
			}
			else
			{
				isCrouch = false;
				_targetSpeed = moveSpeed;
				_targetCamera = _standCamPos;
				_input.LockAllInput(reset: false, look: true, fire: true);
				StartCoroutine(_event.OnCrouchToStandEvent());
			}

		}
	}

	private void Prone() //Player Prone
	{
		if (_input.prone & _input.proneAble)
		{
			_input.prone = false;
			if (!isProne)
			{
				isProne = true;
				isCrouch = false;
				_targetSpeed = proneSpeed;
				_targetCamera = _proneCamPos;
				_input.LockAllInput(reset: false, look: true, prone: true, fire: true);
				_input.proneAble = false;
				StartCoroutine(_event.OnCrouchToProneEvent());
			}
			else
			{
				isProne = false;
				isCrouch = true;
				_targetSpeed = crouchSpeed;
				_targetCamera = _crouchCamPos;
				_input.LockAllInput(reset: false, look: true, fire: true);
				StartCoroutine(_event.OnProneToCrouchEvent());
			}

		}
	}

	private void Fire() //Player Fire
	{
		if (_input.fire & _fireDeltaTime <= 0f)
		{
			var bullet = LeanPool.Spawn(_bullet, _firePoint.position, _targetCamera.transform.rotation); //Spawn the bullet
			LeanPool.Despawn(bullet, 2f);
			_fireDeltaTime = _rateOfFire;
			_event.OnFireEvent();
		}

		if (_fireDeltaTime > 0)
		{
			_fireDeltaTime -= Time.deltaTime;
		}
	}

}