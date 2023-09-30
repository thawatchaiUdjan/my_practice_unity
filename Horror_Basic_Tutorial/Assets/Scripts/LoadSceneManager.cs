using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    public GameObject loadGameScene;
	public GameObject mainMenu;
	public TextMeshProUGUI loadProgressText;

	//Anim ID
	private string _animLoadSceneFadeIn = "LoadSceneFadeIn"; //2s
	private string _animLoadSceneFadeOut = "LoadSceneFadeOut"; //5s

	//
	public static LoadSceneManager instance;
	private void Awake() { 
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else Destroy(gameObject);
	}

	public void LoadSceneOnClick(string sceneName){
		Time.timeScale = 1;
		LoadSceneIn();
		StartCoroutine(LoadScene(sceneName));
	}

	public IEnumerator LoadScene(string sceneName){
		yield return new WaitUntil(() => !IsPlayAnimate());

		// Debug.Log("Start Load Scene: " + sceneName);

		loadProgressText.gameObject.SetActive(true);
		var scene = SceneManager.LoadSceneAsync(sceneName);

		while (!scene.isDone)
		{	
			var progressValue = Mathf.Clamp01(scene.progress / 0.9f);
			loadProgressText.text = (progressValue * 100f).ToString("0") + "%";
			yield return null;
		}

		loadProgressText.gameObject.SetActive(false);
		StartCoroutine(IsMainMenuScene());
	}

	public void LoadSceneAnimate(string animName){
		loadGameScene.SetActive(true);
		loadGameScene.GetComponent<Animation>().Play(animName);
	}

	public IEnumerator IsMainMenuScene(){
		if (SceneManager.GetActiveScene().buildIndex == 0){
			yield return new WaitForSeconds(2);
			HideSceneLoad();
			mainMenu.SetActive(true);
		}
		else {
			mainMenu.SetActive(false);
		}
	}

	public bool IsPlayAnimate(){
		return loadGameScene.GetComponent<Animation>().isPlaying;
	}

	public void LoadSceneIn(){
		LoadSceneAnimate(_animLoadSceneFadeIn);
	}

	public void LoadSceneOut(){
		LoadSceneAnimate(_animLoadSceneFadeOut);
	}

	public void HideSceneLoad(){
		loadGameScene.SetActive(false);
	}


}
