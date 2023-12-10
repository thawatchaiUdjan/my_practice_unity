using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
	[SerializeField] private GameObject _timerUI;
	[SerializeField] private bool _isTimeCount;
	[Tooltip("Minute")] [SerializeField] private float _timeCount = 1f;

	//
	private float _time;
	private TextMeshProUGUI _timeCountText;

	//
	public delegate void OnTimerFinish();
	public OnTimerFinish onGameTimeOutCallBack;

	//
	public static TimerManager instance;
	private void Awake()
	{
		instance = this;
	}
	void Start()
	{
		_timeCountText = _timerUI.GetComponentInChildren<TextMeshProUGUI>();
		_time = _timeCount * 60f;
	}

	private void FixedUpdate()
	{
		TimeCount();
	}

	private void TimeCount()
	{
		var minute = Mathf.FloorToInt(_time / 60);
		var second = Mathf.FloorToInt(_time % 60);
		_timeCountText.text = string.Format("{0:0}:{1:00}", minute, second);

		if (_isTimeCount & _time > 0f)
		{
			_time -= Time.deltaTime;

			if (_time <= 0f)
			{
				_time = 0f;
				_isTimeCount = false;
				onGameTimeOutCallBack?.Invoke();
			}
		}
	}

	public void StartTimer()
	{
		_isTimeCount = true;
	}

	public void PauseTimer(bool isPause)
	{
		_isTimeCount = !isPause;
	}

	public void AddTime(float time)
	{
		_time += time;
	}
}
