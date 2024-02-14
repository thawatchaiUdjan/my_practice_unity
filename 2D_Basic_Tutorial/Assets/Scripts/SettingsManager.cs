using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
	[SerializeField] private GameObject _settingsPanel;
	[SerializeField] private InputActionAsset _inputActions;
	[SerializeField] private Slider _soundSlider;
	[SerializeField] private TextMeshProUGUI _soundValueText;

	//
	private SettingsData _settingsData;
	private SaveManager _save;
	private AudioSource _soundUI;

	//
	public static SettingsManager instance;
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else Destroy(gameObject);
	}

	private void Start()
	{
		_settingsData = SettingsData.instance;
		_save = SaveManager.instance;

		_soundUI = GetComponent<AudioSource>();

		SettingsData.OnDataChange += UpdateSound;
		UpdateSound();
		SetUIButtonSound();
	}

	private void UpdateSound()
	{
		_soundUI.volume = _settingsData.masterVolume * 0.3f;
	}

	public void OnClickResetDefault()
	{
		foreach (var action in _inputActions.actionMaps)
		{
			action.RemoveAllBindingOverrides();
		}
		_settingsData.SetSettingData(new SettingsData());
		UpdateSettingsUI();
		OnClickApply();
	}

	public void OnClickApply()
	{
		SetSettings();
		SaveSetting();
	}

	public void SaveSetting()
	{
		_settingsData.inputBinding = _inputActions.SaveBindingOverridesAsJson();
		_save.SaveSettingsData();
	}

	public void SetSettings()
	{
		_settingsData.masterVolume = _soundSlider.value * 0.006f;
	}

	public void UpdateSettingsUI()
	{
		OnChangeSound(_settingsData.masterVolume / 0.006f); //Set Sound
		_inputActions.LoadBindingOverridesFromJson(_settingsData.inputBinding); //Set Binding Key
	}

	public void OnChangeSound(float value)
	{
		_soundSlider.value = value;
		_soundValueText.text = value.ToString();
	}

	public void ShowSettings()
	{
		_settingsPanel.SetActive(true);
		UpdateSettingsUI();
	}

	public void CloseSettings()
	{
		_settingsPanel.SetActive(false);
		UpdateSettingsUI();
	}

	public void SetUIButtonSound()
	{
		var buttons = FindObjectsOfType<Button>(true);
		foreach (var button in buttons)
		{
			button.onClick.AddListener(() => _soundUI.Play());
		}
	}

}
