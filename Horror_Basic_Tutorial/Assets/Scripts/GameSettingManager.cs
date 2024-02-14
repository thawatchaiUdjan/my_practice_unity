using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;
using PlayerController;
using System.Linq;


public class GameSettingManager : MonoBehaviour
{
	[Header("Resolution")]
	public TMP_Dropdown resolutionDropdown;
	private Resolution[] resolutions;

	[Header("FullScreen")]
	public Toggle fullScreenToggle;

	[Header("Quality")]
	public TMP_Dropdown qualityDropdown;

	[Header("Sound")]
	public Slider soundSlider;
	public TextMeshProUGUI soundValueText;

	[Header("Settings Value")]
	public int _resolution;
	public bool _isFullScreen;
	public int _qualityLevel;
	public float _volume;
	public float _mouseSens;

	[Header("Settings Default Value")]
	public int _defaultResolution;
	public bool _defaultFullScreen = false;
	public int _defaultQuality;
	public float _defaultVolume = 1f; // = 50 value slider
	public float _defaultMouseSens = 1f;

	//
	public static GameSettingManager instance;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else Destroy(gameObject);
	}

	void Start()
	{
		_defaultQuality = QualitySettings.GetQualityLevel(); //Save Default Quality
		_mouseSens = _defaultMouseSens;
		SetupResolutionDropdown();
		ResetDefaultOnClick();
	}

	private void SetupResolutionDropdown()
	{
		var resOptions = new List<string>();

		resolutions = Screen.resolutions.Where((resolution) => (int)resolution.refreshRateRatio.value == 60).ToArray();
		resolutionDropdown.ClearOptions();

		for (int i = 0; i < resolutions.Length; i++)
		{
			var option = $"{resolutions[i].width} x {resolutions[i].height}";
			resOptions.Add(option);

			if (resolutions[i].width == Screen.width & resolutions[i].height == Screen.height)
			{
				_defaultResolution = i;
			}
		}

		resolutionDropdown.AddOptions(resOptions);
		resolutionDropdown.RefreshShownValue();
	}

	public void StartGameSettings()
	{
		FirstPersonController.instance.RotationSpeed = _mouseSens; //Set Mouse
		SoundManager.instance.AudioVolume = _volume; //Set Sound
		Camera.main.TryGetComponent<PostProcessVolume>(out var _ppVolume); //Set PostProcessing

		if (_ppVolume != null & _qualityLevel >= 2)
		{
			var profile = _ppVolume.profile;
			profile.GetSetting<AmbientOcclusion>().active = true;
			profile.GetSetting<Bloom>().active = true;
		}
	}

	public void SetupSettingsUI()
	{
		resolutionDropdown.value = _resolution;
		fullScreenToggle.isOn = _isFullScreen;
		qualityDropdown.value = _qualityLevel;
		OnChangeSound(_volume * 50f);
	}

	public void SetSettings()
	{
		_resolution = resolutionDropdown.value;
		_isFullScreen = fullScreenToggle.isOn;
		_qualityLevel = qualityDropdown.value;
		_volume = soundSlider.value / 50f;
	}

	public void ApplyChange()
	{
		Resolution resolution = resolutions[_resolution];
		Screen.SetResolution(resolution.width, resolution.height, _isFullScreen);
		QualitySettings.SetQualityLevel(_qualityLevel);
	}

	public void ApplyOnClick()
	{
		SetSettings();
		ApplyChange();
	}

	public void ResetDefaultOnClick()
	{
		_resolution = _defaultResolution;
		_isFullScreen = _defaultFullScreen;
		_qualityLevel = _defaultQuality;
		_volume = _defaultVolume;
		SetupSettingsUI();
		ApplyChange();
	}

	public void QuitOnClick()
	{
		Application.Quit();
	}

	public void OnChangeSound(float volume)
	{
		soundSlider.value = volume;
		soundValueText.text = volume.ToString();
	}

}
