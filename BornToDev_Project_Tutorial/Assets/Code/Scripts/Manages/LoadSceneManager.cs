using System;
using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
	[SerializeField] private GameObject _loadingScreen;
	[SerializeField] private TextMeshProUGUI _loadingText;
	[SerializeField] private float _speedText = 1f;

	private Animation _loadSceneAnim;

	//AnimID
	private string _animFadeIn = "LoadFadeIn";
	private string _animFadeOut = "LoadFadeOut";

	//
	public static LoadSceneManager instance;
	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		_loadSceneAnim = _loadingScreen.GetComponent<Animation>();
	}

	public void OnClickLoadScene(int sceneIndex)
	{
		Time.timeScale = 1;
		if (sceneIndex == -1)
			sceneIndex = SceneManager.GetActiveScene().buildIndex;
		StartCoroutine(SceneLoad(sceneIndex));
	}

	private IEnumerator SceneLoad(int sceneIndex)
	{
		yield return StartCoroutine(FadeSceneIn());
		
		var currProgress = 0f;
		var scene = SceneManager.LoadSceneAsync(sceneIndex);
		scene.allowSceneActivation = false;

		while (!scene.isDone)
		{
			currProgress = Mathf.MoveTowards(currProgress, scene.progress, _speedText * Time.deltaTime);
			var progress = Mathf.Clamp01(currProgress / 0.9f) * 100f;
			_loadingText.text = "Loading " + Math.Round(progress, 2) + "%";
			if (progress >= 100f)
			{
				scene.allowSceneActivation = true;
			}

			yield return null;
		}
	}

	public IEnumerator FadeSceneIn()
	{
		_loadingScreen.SetActive(true);
		_loadSceneAnim.Play(_animFadeIn);
		yield return new WaitUntil(() => !_loadSceneAnim.isPlaying);
		SoundManager.instance.OnPlayBGM(isPlay: false);
	}

	public IEnumerator FadeSceneOut()
	{
		_loadingScreen.SetActive(true);
		_loadSceneAnim.Play(_animFadeOut);
		yield return new WaitUntil(() => !_loadSceneAnim.isPlaying);
		_loadingScreen.SetActive(false);
	}
}
