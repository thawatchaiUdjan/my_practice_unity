using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostManager : MonoBehaviour
{
	public float delayOfRandom = 1f;

	private AudioSource[] _sounds; //0 = Behaviour Sound, 1 = Heartbeat Sound
	private NavMeshAgent _ghost;
	private SoundManager _soundManager;
	private HealthManager _health;
	private Animator _animator;
	private GlitchEffect _glitchEfx;
	private float maxDistance;
	private IEnumerator _healthDecrease;
	private IEnumerator _healthIncrease;

	private GameObject[] _ghostWayPoints;
	private int? _wayPointIndex = null;

	public static GhostManager instance;
	private void Awake() {
		instance = this;
	}

    private void StartComponents()
    {
        _health = HealthManager.instance;
		_soundManager = SoundManager.instance;
		_glitchEfx = GlitchEffect.instance;

		_ghost = GetComponent<NavMeshAgent>();
		_animator = GetComponent<Animator>();
		_sounds = GetComponents<AudioSource>();
	
		_healthDecrease = _health.DecreaseHealth();
		_healthIncrease = _health.IncreaseHealth();	

		// Sound Set Volume
		_sounds[0].volume = _soundManager.AudioVolume * 0.067f;
		_sounds[1].volume = _soundManager.AudioVolume * 0.2f;

		_ghostWayPoints = GameObject.FindGameObjectsWithTag("GhostWayPoint");
    }

	public void SetupGhost(){
		StartComponents();
		gameObject.SetActive(true);
		StartCoroutine(RandomWayPoint());
	}

	public void ResetGhost(){
		StopAllCoroutines();
		if (_glitchEfx != null) _glitchEfx.HideEfx();
		gameObject.SetActive(false);
	}

	public void StopGhost(){
		StopAllCoroutines();
		if (_ghost.enabled) _ghost.isStopped = true;
		if(_animator.GetCurrentAnimatorStateInfo(0).IsName(GhostTypeManager.Anim.Walk.ToString()))
			PlayAnimate(GhostTypeManager.Anim.Idle);
	}

	public IEnumerator RandomWayPoint(){
		while (true)
		{
			_wayPointIndex = RandomIndex();
			var wayPoint = _ghostWayPoints[(int)_wayPointIndex].transform;
			var animType = wayPoint.GetComponent<GhostTypeManager>().animType;
			
			_ghost.enabled = true;
			_ghost.SetDestination(wayPoint.position);
			_animator.Play(GhostTypeManager.Anim.Walk.ToString());
			_sounds[0].Stop();

			while (_ghost.pathPending || _ghost.remainingDistance > _ghost.stoppingDistance){
				yield return null;
			}
			
			_ghost.enabled = false;
			transform.position = wayPoint.position;
			transform.rotation = wayPoint.rotation;
			
			PlayAnimate(animType);

			yield return new WaitForSeconds(delayOfRandom * 60f);
		}
	}

	public int RandomIndex(){
		int RandomIndex;
		while (true){
			RandomIndex = Random.Range(0, _ghostWayPoints.Length);
			if(_wayPointIndex != RandomIndex) break;
		}
		return RandomIndex;
	}

	public void PlayAnimate(GhostTypeManager.Anim animType){
		_animator.Play(animType.ToString());
		_sounds[0].clip = _soundManager.GetGhostSoundAnim(animType);
		_sounds[0].Play();
	}

	void OnTriggerEnter(Collider other)
	{
		CheckComponent();
		GameObject target = other.gameObject; 

		if (target.tag == "Player")
		{
			StopCoroutine(_healthIncrease);
			StartCoroutine(_healthDecrease);
			maxDistance = Vector3.Distance(transform.position, target.transform.position);
			_sounds[1].Play();
			_glitchEfx.ShowEfx(1);
		}
	}

	void OnTriggerExit(Collider other)
	{
		CheckComponent();
		GameObject target = other.gameObject; 

		if (target.tag == "Player")
		{	
			StopCoroutine(_healthDecrease);
			StartCoroutine(_healthIncrease);
			_sounds[1].Stop();
			_glitchEfx.HideEfx();
		}
	}

	void OnTriggerStay(Collider other)
	{
		CheckComponent();
		GameObject target = other.gameObject; 

		if (target.tag == "Player")
		{			
			var distanceFromPlayer = Vector3.Distance(transform.position, target.transform.position);
			
			//For default
			var decreaseHp = _health.decHealthSlow;
			var soundClip = _soundManager.heartbeatSlow;
			var glitchLvl = 1;
			
			//Lower 45%
			if (distanceFromPlayer/maxDistance * 100f < 45f)
			{
				soundClip = _soundManager.heartbeatFast;
				decreaseHp = _health.decHealthFast;
				glitchLvl = 3;
			}
			//Lower 75%
			else if (distanceFromPlayer/maxDistance * 100f < 75f)
			{
				soundClip = _soundManager.heartbeatMedium;
				decreaseHp = _health.decHealthMedium;
				glitchLvl = 2;
			}
			

			if (_sounds[1].clip != soundClip)
			{
				_sounds[1].Stop();
				_sounds[1].clip = soundClip;
				_sounds[1].PlayDelayed(0.3f);

				_health.decreaseHealth = decreaseHp;

				_glitchEfx.ShowEfx(glitchLvl);
			}
		}
	}

	private void CheckComponent()
	{
		if (_health == null) StartComponents();
	}

	private void OnFootStep(AnimationEvent animationEvent){
		var _clip = _soundManager.FootStepAudioClip[0];
		if (_clip != null)
		{
			AudioSource.PlayClipAtPoint(_clip,  transform.position, _soundManager.AudioVolume * 0.6f);
		}
	}
	
}
