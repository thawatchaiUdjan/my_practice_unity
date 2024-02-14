using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
	[SerializeField] private GameObject _loadScreen;
	public bool isDebug;

	//Private members
	private TextMeshProUGUI _loadText;
	private float refVelcntLoad;
	private float cntSpeed = 100f;
	private float currProg;

	public static LoadSceneManager instance;
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
		_loadText = _loadScreen.GetComponentInChildren<TextMeshProUGUI>();
		if (isDebug & SceneManager.GetActiveScene().buildIndex != 0)
		{
			GameManager.instance.GameSetup();
		}
	}

	public void LoadScene(int scene, bool isTravel = false)
	{
		if (scene == -1) scene = 1;
		StartCoroutine(SceneLoad(scene, isTravel));
	}

	private IEnumerator SceneLoad(int scene, bool isTravel)
	{
		_loadScreen.SetActive(true);
		var sceneLoad = SceneManager.LoadSceneAsync(scene);
		while (!sceneLoad.isDone)
		{
			var progress = Mathf.Clamp01(sceneLoad.progress / 0.9f) * 100f;
			currProg = Mathf.SmoothDamp(currProg, progress, ref refVelcntLoad, cntSpeed * Time.deltaTime);
			_loadText.text = "Loading  " + progress.ToString("0.00") + "%";
			yield return null;
		}
		_loadScreen.SetActive(false);
		if (SceneManager.GetActiveScene().buildIndex != 0)
		{
			GameManager.instance.GameSetup(isTravel);
		}
		SettingsManager.instance.SetUIButtonSound();
	}

}
