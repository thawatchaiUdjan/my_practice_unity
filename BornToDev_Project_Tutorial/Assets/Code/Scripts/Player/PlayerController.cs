using UnityEngine;
using Lean.Pool;

public class PlayerController : MonoBehaviour
{
	public float moveSpeed = 2f;
	public float sprintSpeed = 3.5f;
	public float rotationSpeed = 0.05f;
	public float cameraSpeed = 0.05f;

	[Header("Shoot Data")]
	[SerializeField] private GameObject _ballObject;
	[SerializeField] private Transform _shootPoint;
	public float delayShoot = 0.2f;

	[Header("Cinemachine")]
	[SerializeField] private GameObject _cameraTarget;
	public float TopClamp = 70.0f;
	public float BottomClamp = -30.0f;
	public float CameraAngleOverride = 0.0f;
	public bool isInvertY;

	private float _targetRotation;
	private float _lookVelocity;
	private float _shootDeltaTime;

	//Cinemachine
	private float _cinemachineTargetYaw;
	private float _cinemachineTargetPitch;
	private bool _isCurrentDeviceMouse = true;
	private const float _threshold = 0.01f;

	//
	private PlayerInputController _input;
	private GameManager _game;
	private SoundManager _sound;
	private CharacterController _controller;
	private PlayerEvent _event;
	private Animator _animator;
	private Transform _mainCamera;

	//AnimID
	private string _animSpeed = "Speed";

	//
	public static PlayerController instance;
	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		_input = PlayerInputController.instance;
		_game = GameManager.instance;
		_sound = SoundManager.instance;
		_controller = GetComponent<CharacterController>();
		_event = GetComponent<PlayerEvent>();
		_animator = GetComponent<Animator>();
		_mainCamera = Camera.main.transform;

		_cinemachineTargetYaw = _cameraTarget.transform.rotation.eulerAngles.y;
	}

	private void Update()
	{
		Move();
		Shoot();
	}

	private void LateUpdate()
	{
		CameraRotation();
	}

	private void CameraRotation()
	{
		// if there is an input and camera position is not fixed
		if (_input.look.sqrMagnitude >= _threshold && _input.isCursorForLook)
		{
			//Don't multiply mouse input by Time.deltaTime;
			float deltaTimeMultiplier = _isCurrentDeviceMouse ? cameraSpeed : Time.deltaTime;

			_cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
			_cinemachineTargetPitch += isInvertY ? _input.look.y : -_input.look.y * deltaTimeMultiplier;
		}

		// clamp our rotations so our values are limited 360 degrees
		_cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
		_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

		// Cinemachine will follow this target
		_cameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
			_cinemachineTargetYaw, 0.0f);
	}

	private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
	{
		if (lfAngle < -360f) lfAngle += 360f;
		if (lfAngle > 360f) lfAngle -= 360f;
		return Mathf.Clamp(lfAngle, lfMin, lfMax);
	}

	private void Move()
	{
		if (_input.move != Vector2.zero & _input.moveAble)
		{
			Look();
			var targetSpeed = _input.sprint ? sprintSpeed : moveSpeed;
			var targetDirection = Quaternion.Euler(0f, _targetRotation, 0f) * Vector3.forward;
			_controller.Move(targetDirection.normalized * (targetSpeed * Time.deltaTime));
			_animator.SetFloat(_animSpeed, targetSpeed, 0.1f, Time.deltaTime);
		}
		else _animator.SetFloat(_animSpeed, 0f, 0.025f, Time.deltaTime); //Hack Reset speed

		if (_animator.GetFloat(_animSpeed) <= 0.1f) _animator.SetFloat(_animSpeed, 0f);
	}

	private void Look()
	{
		var inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
		_targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.eulerAngles.y;
		var rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _lookVelocity, rotationSpeed);
		transform.rotation = Quaternion.Euler(0f, rotation, 0f);
	}

	private void Shoot()
	{
		if (_input.shoot & _shootDeltaTime <= 0f)
		{
			_input.shoot = false;
			_shootDeltaTime = delayShoot;
			var ball = LeanPool.Spawn(_ballObject, _shootPoint.position, transform.rotation);
			LeanPool.Despawn(ball, 2f);
			_event.OnShooting();
		}

		if (_shootDeltaTime > 0)
		{
			_shootDeltaTime -= Time.deltaTime;
		}
	}
}
