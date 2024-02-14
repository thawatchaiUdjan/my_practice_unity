using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneManager : MonoBehaviour
{
	[SerializeField] private GameObject _loadSceneUI;
	[SerializeField] private Slider _loadingSlider;
	[SerializeField] private float _speedSlider = 1f;

	//
	public static LoadSceneManager instance;
	private void Awake()
	{
		instance = this;
	}

	public void LoadScene(int sceneIndex)
	{
		_loadSceneUI.SetActive(true);
		StartCoroutine(Load(sceneIndex));
	}

	private IEnumerator Load(int sceneIndex)
	{
		var currProgress = 0f;
		var progress = 0f;
		var scene = SceneManager.LoadSceneAsync(sceneIndex);
		scene.allowSceneActivation = false;
		while (progress < 1f)
		{
			currProgress = Mathf.MoveTowards(currProgress, scene.progress, _speedSlider * Time.deltaTime);
			progress = Mathf.Clamp01(currProgress / 0.9f);
			_loadingSlider.value = progress;
			yield return null;
		}
		scene.allowSceneActivation = true;
	}
}
